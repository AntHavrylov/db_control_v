using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Xml.Linq;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;

namespace db_control
{
    public partial class ChangeLogForm : Form
    {
        //Path to liquibase differences log
        const string logPath = @"db.changelog.xml";
        public bool acceptChanges = false;
        public string dbName { get; set; }
        public string sourceUrl { get; set; }
        public string sourceUName { get; set; }
        public string sourceUPass { get; set; }
        public string destinationUrl { get; set; }
        public string destinationUName { get; set; }
        public string destinationUPass { get; set; }

        public List<string> renameData = new List<string>();

        public List<string> dropQuery = new List<string>();
        public List<string> addFKList = new List<string>();
        public List<String[]> renameFKQuery = new List<string[]>();
        public List<string> constraintFailsSolution = new List<string>();

        public ChangeLogForm(string schema)
        {
            this.TopMost = false;
            this.TopLevel = true;
            InitializeComponent();
            dbName = schema;
            labelDbNameData.Text = "Differences for database: \'" + dbName + "\'";
            parceChangeLog();
        }

        public ChangeLogForm(string schema, string sUrl, string sName, string sPass, string dUrl, string dName, string dPass)
        {
            InitializeComponent();
            TopMost = true;

            dbName = schema;
            sourceUrl = sUrl;
            sourceUName = sName;
            sourceUPass = sPass;
            destinationUrl = dUrl;
            destinationUName = dName;
            destinationUPass = dPass;
            labelDbNameData.Text = "Differences for database: \'" + dbName + "\'";

            parceChangeLog();
        }

        /// <summary>
        /// Parse difChangeLog and print to list
        /// </summary>
        public void parceChangeLog()
        {
            if (File.Exists(logPath))
            {
               
                var lines = System.IO.File.ReadAllLines(logPath);
                lines[0] = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\"?>";
                System.IO.File.WriteAllLines(logPath, lines); ;
               
                XmlDocument doc = new XmlDocument();
                doc.Load(logPath);
                List<string> addColumnList = new List<string>();

                foreach (XmlNode node in doc.DocumentElement)
                {
                    string author = node.Attributes[0].InnerText;
                    string id = node.Attributes[1].InnerText;
                    string nodeName = node.Name;
                    //Console.WriteLine(nodeName + " author: " + author + " id: " + id);

                    //PARSING XML NODES
                    foreach (XmlNode child in node.ChildNodes)
                    {
                        //PARSE ADD FOREING KEYS NODES
                        if (child.Name.Contains("addForeignKeyConstraint"))
                        {
                            //db name -> referencedTableCatalogName
                            string query;
                            bool sameDb = true;
                            //sameDb flag shows if referense scheme is the same as source scheme
                            try
                            {
                                query = "SET FOREIGN_KEY_CHECKS = 0;SET SQL_SAFE_UPDATES = 0;ALTER TABLE `" + dbName + "`.`" + child.Attributes["baseTableName"].Value + "` ADD CONSTRAINT `" + child.Attributes["constraintName"].Value + "` FOREIGN KEY (`" + child.Attributes["baseColumnNames"].Value
                                    + "`)  REFERENCES `" + child.Attributes["referencedTableCatalogName"].Value + "`.`" + child.Attributes["referencedTableName"].Value + "` (`" + child.Attributes["referencedColumnNames"].Value + "`)";
                                sameDb = false;
                            }
                            catch (Exception ex)
                            {
                                query = "SET FOREIGN_KEY_CHECKS = 0;SET SQL_SAFE_UPDATES = 0;ALTER TABLE `" + dbName + "`.`" + child.Attributes["baseTableName"].Value + "` ADD CONSTRAINT `" + child.Attributes["constraintName"].Value + "` FOREIGN KEY (`" + child.Attributes["baseColumnNames"].Value
                                    + "`)  REFERENCES `" + dbName + "`.`" + child.Attributes["referencedTableName"].Value + "` (`" + child.Attributes["referencedColumnNames"].Value + "`)";
                            }

                            foreach (XmlAttribute xa in child.Attributes)
                            {
                                switch (xa.Name)
                                {
                                    case "onDelete":
                                        query += " ON DELETE " + child.Attributes["onDelete"].Value + " ";
                                        break;
                                    case "onUpdate":
                                        query += " ON UPDATE " + child.Attributes["onUpdate"].Value + " ";
                                        break;
                                }
                            }
                            query += ";SET SQL_SAFE_UPDATES = 1;SET FOREIGN_KEY_CHECKS = 1;";

                            //BUILD "a foreign key constraint fails" solution query
                            string constraintFailsQuery;
                            if (!sameDb)
                            {
                                constraintFailsQuery = "SET FOREIGN_KEY_CHECKS = 0;SET SQL_SAFE_UPDATES = 0;DELETE FROM `" + dbName + "`.`" + child.Attributes["baseTableName"].Value + "` WHERE " + child.Attributes["baseColumnNames"].Value + " not in (select `" + child.Attributes["referencedColumnNames"].Value + "` from `" + child.Attributes["referencedTableCatalogName"].Value + "`.`" + child.Attributes["referencedTableName"].Value + "`); SET SQL_SAFE_UPDATES = 1;SET FOREIGN_KEY_CHECKS = 1;";
                                constraintFailsSolution.Add(constraintFailsQuery);
                            }
                            else
                            {
                                constraintFailsQuery = "SET FOREIGN_KEY_CHECKS = 0;SET SQL_SAFE_UPDATES = 0;DELETE FROM `" + dbName + "`.`" + child.Attributes["baseTableName"].Value + "` WHERE " + child.Attributes["baseColumnNames"].Value + " not in (select `" + child.Attributes["referencedColumnNames"].Value + "` from `" + dbName + "`.`" + child.Attributes["referencedTableName"].Value + "`); SET SQL_SAFE_UPDATES = 1;SET FOREIGN_KEY_CHECKS = 1;";
                                constraintFailsSolution.Add(constraintFailsQuery);
                            }
                            //Console.WriteLine("Add FK query \t\t"+query);
                            addFKList.Add(query);
                            child.ParentNode.RemoveAll();
                        }

                        //PARSE DROP FOREIGN KEYS NODES
                        else if (child.Name.Contains("dropForeignKeyConstraint"))
                        {
                            //< dropForeignKeyConstraint baseTableName = "sites" constraintName = "FK_DboUserIdCreator_as" />
                            string query = " ALTER TABLE `" + dbName + "`.`" + child.Attributes["baseTableName"].Value + "` DROP FOREIGN KEY `" + child.Attributes["constraintName"].Value + "`; ";
                            //Console.WriteLine("drop FK: " + query);
                            dropQuery.Add(query);

                            // ALTER TABLE `ac`.`access_levels_to_doors_group` DROP FOREIGN KEY `fk_access_doorGroup`; 
                            // SELECT REFERENCED_TABLE_SCHEMA, REFERENCED_TABLE_NAME, REFERENCED_COLUMN_NAME FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE WHERE CONSTRAINT_NAME = 'fk_access_doorGroup';

                            string constring = "server=" + destinationUrl + ";user=" + destinationUName + ";pwd=" + destinationUPass + ";database=" + dbName + ";";
                            string getFKdataQuery = "SELECT REFERENCED_TABLE_SCHEMA, REFERENCED_TABLE_NAME, REFERENCED_COLUMN_NAME FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE WHERE CONSTRAINT_NAME = '"+ child.Attributes["constraintName"].Value + "';";
                            using (MySqlConnection conn = new MySqlConnection(constring))
                            {
                                StringBuilder result = new StringBuilder();
                                try
                                {
                                    conn.Open();
                                    MySqlCommand cmd = new MySqlCommand(getFKdataQuery, conn);
                                    MySqlDataReader rdr = cmd.ExecuteReader();
                                    while (rdr.Read())
                                    {
                                        //ALTER TABLE `ac`.`access_levels_to_doors_group` ADD CONSTRAINT `fk_access_doorGroup` FOREIGN KEY (`s11`)  REFERENCES `ac`.`door_groups` (`DoorGroupId`) ON DELETE NO ACTION  ON UPDATE CASCADE;
                                        renameFKQuery.Add(new string[3]{ "ALTER TABLE `"+ dbName + "`.`"+ child.Attributes["baseTableName"].Value+ "` ADD CONSTRAINT `"+ child.Attributes["constraintName"].Value + "` FOREIGN KEY (`",
                                            "",
                                            "`)  REFERENCES `"+rdr[0].ToString()+"`.`"+rdr[1].ToString()+"` (`"+rdr[2].ToString()+"`) ON DELETE CASCADE ON UPDATE CASCADE;" });
                                    }
                                    rdr.Close();
                                }
                                catch (Exception ex)
                                {                                    
                                    Console.WriteLine(ex.ToString());
                                }
                                conn.Close();                                 
                            }
                                                        
                            child.ParentNode.RemoveAll();
                        }
                        //REMOVE COMMENTS
                        else if (child.Name.Contains("setColumnRemarks"))
                        {
                            child.ParentNode.RemoveChild(child);
                        }
                        //add columns 
                        else if (child.Name.Contains("addColumn"))
                        {
                            foreach (XmlNode cc in child.ChildNodes)
                            {
                                addColumnList.Add(cc.Attributes["name"].Value);
                            }
                        }


                        //IF SOMEBODY WANTS REMOVE PRIMARY KEY NEED THAT AUTOINCREMENT WILL BE TURNED OFF
                        else if (child.Name.Contains("dropPrimaryKey"))
                        {
                            string query = "";
                            query += "SELECT COLUMN_NAME FROM information_schema.table_constraints t " +
                                " JOIN information_schema.key_column_usage k" +
                                " USING(constraint_name, table_schema, table_name)" +
                                " WHERE t.constraint_type = 'PRIMARY KEY'" +
                                " AND t.table_schema = '" + dbName + "'" +
                                " AND t.table_name = '" + child.Attributes["tableName"].Value + "';";

                            string pk = runMysqlQuery(destinationUrl, destinationUName, destinationUPass, dbName, query);
                            string[] pk_columns = pk.Split(' ');
                            foreach (string pk_column in pk_columns)
                            {
                                if (!pk_column.Equals(""))
                                {
                                    query = "SELECT DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = '"+dbName+ "' AND TABLE_NAME = '"+ child.Attributes["tableName"].Value + "' And Column_name = '"+pk_column+"';";
                                    string pk_type = runMysqlQuery(destinationUrl, destinationUName, destinationUPass, dbName, query).Trim();
                                    //SET FOREIGN_KEY_CHECKS = 0;SET SQL_SAFE_UPDATES = 0
                                    if (pk_type.Equals("int")) { pk_type = "INT(11)"; }
                                    else if (pk_type.Equals("varchar")) { pk_type = "VARCHAR(45)"; }
                                    
                                    query = "SET FOREIGN_KEY_CHECKS=0; alter table `"+dbName+"`.`"+child.Attributes["tableName"].Value+"` MODIFY `" + pk_column + "` "+ pk_type+"; SET FOREIGN_KEY_CHECKS = 1;";
                                    runMysqlQuery(destinationUrl, destinationUName, destinationUPass, dbName, query);
                                }

                            }
                        }

                        
                        
                        XmlDocument docc = node.OwnerDocument;
                        XmlAttribute attrA = docc.CreateAttribute("author");
                        attrA.Value = author;
                        node.Attributes.SetNamedItem(attrA);
                        XmlAttribute attrId = docc.CreateAttribute("id");
                        attrId.Value = id;
                        node.Attributes.SetNamedItem(attrId);
                    }
                }
                doc.Save(logPath);

                //If list drop foreign keys is not empty trying to run every query
                if (dropQuery.Count > 0)
                {
                    foreach (string dropFKQuery in dropQuery)
                    {
                        try
                        {
                            runMysqlQuery(destinationUrl, destinationUName, destinationUPass, dbName, dropFKQuery);
                        }
                        catch (MySqlException ex)
                        {
                            Console.WriteLine("\nDROP\nForeign key error"+ex);
                        }
                    }
                }

                doc.Load(logPath);

                //DATAGRID HEADER
                dataGridViewChanges.ColumnCount = 3;
                dataGridViewChanges.Columns[0].Name = "Action";
                dataGridViewChanges.Columns[0].Width = 150;
                dataGridViewChanges.Columns[1].Name = "Changes";
                dataGridViewChanges.Columns[1].Width = 300;
                dataGridViewChanges.Columns[2].Name = "User action";
                dataGridViewChanges.Columns[2].Width = 150;
                DataGridViewCheckBoxColumn chBox = new DataGridViewCheckBoxColumn() { Width = 50 } ;
                chBox.HeaderText = "Execute";
                chBox.Name = "checkBox";
              
                dataGridViewChanges.Columns.Add(chBox);
                
                //Parse XML And build DataGridTable
                foreach (XmlNode node in doc.DocumentElement)
                {
                    foreach (XmlNode child in node.ChildNodes)
                    {

                        if (child.Name == "addColumn")
                        {
                            DataGridViewRow row = new DataGridViewRow() ;

                            DataGridViewTextBoxCell c1 = new DataGridViewTextBoxCell();
                            DataGridViewTextBoxCell c2 = new DataGridViewTextBoxCell();

                            //dataGridViewChanges.Columns["Action"] = child.Name;
                            c1.Value = child.Name + " | " + node.Attributes[1].InnerText;
                            try
                            {
                                c2.Value = child.Attributes["tableName"].Value + ";" + child.ChildNodes[0].Attributes["name"].Value + ";" + child.ChildNodes[0].Attributes["type"].Value + ";nullable=" + child.ChildNodes[0].ChildNodes[0].Attributes["nullable"].Value;
                            }
                            catch (Exception e)
                            {
                                c2.Value = child.Attributes["tableName"].Value + ";" + child.ChildNodes[0].Attributes["name"].Value + ";" + child.ChildNodes[0].Attributes["type"].Value + ";NULL DEFAULT NULL";
                            }

                            row.Cells.Add(c1);
                            row.Cells.Add(c2);
                            renameData.Add(c2.Value.ToString());
                            dataGridViewChanges.Rows.Add(row);
                            row.Cells["checkBox"].Value = true;
                        }
                        else if (child.Name == "dropColumn")
                        {
                            //dataLine.Append(child.Name + ": " + child.Attributes["columnName"].Value);
                            DataGridViewRow row = new DataGridViewRow();
                            DataGridViewTextBoxCell c1 = new DataGridViewTextBoxCell();
                            DataGridViewTextBoxCell c2 = new DataGridViewTextBoxCell();
                            c1.Value = child.Name + " | " + node.Attributes[1].InnerText;
                            c2.Value = child.Attributes["tableName"].Value +"."+ child.Attributes["columnName"].Value;
                            DataGridViewComboBoxCell cellBox = new DataGridViewComboBoxCell();
                            cellBox.Items.Add("remove");
                            foreach (string addColumnData in renameData)
                            {
                                string[] dat = addColumnData.Split(';');
                                cellBox.Items.Add(dat[1]);
                            }
                            cellBox.Value = "remove";
                            row.Cells.Add(c1);
                            row.Cells.Add(c2);
                            row.Cells.Add(cellBox);
                            dataGridViewChanges.Rows.Add(row);
                            row.Cells["checkBox"].Value = true;
                        }                       
                        else
                        {
                            StringBuilder att = new StringBuilder();
                            foreach (XmlAttribute xa in child.Attributes)
                            {
                                att.Append(xa.Value + " ");
                            }
                           
                            DataGridViewRow row = new DataGridViewRow();
                            DataGridViewTextBoxCell c1 = new DataGridViewTextBoxCell();
                            DataGridViewTextBoxCell c2 = new DataGridViewTextBoxCell();
                            c1.Value = child.Name + " | " + node.Attributes[1].InnerText;
                            c2.Value = att.ToString();
                            row.Cells.Add(c1);
                            row.Cells.Add(c2);
                            dataGridViewChanges.Rows.Add(row);
                            row.Cells["checkBox"].Value = true;
                        }
                    }
                }
                dataGridViewChanges.AllowUserToAddRows = false;                
            }
            else
            {
                buttonApply.Enabled = false;
            }
        }

        private void ButtonNext_Click(object sender, EventArgs e)
        {

            //REMOVE FROM DIFFERENCE LOG UNCHECKED ELEMENTS
            XmlDocument doc = new XmlDocument();
            doc.Load(logPath);
            foreach (DataGridViewRow row in dataGridViewChanges.Rows)
            {
                string actionId = row.Cells[0].Value.ToString().Split('|')[1].Trim();
                if (Convert.ToBoolean(row.Cells["checkBox"].Value) == false)
                {
                    Console.WriteLine("changeset with id {0} will be removed", actionId);

                    foreach (XmlNode node in doc.DocumentElement)
                    {
                        string author = node.Attributes[0].InnerText;
                        string id = node.Attributes[1].InnerText.Trim();
                        string nodeName = node.Name;

                        if (id.Equals(actionId))
                        {
                            node.RemoveAll();
                        }

                        XmlDocument docc = node.OwnerDocument;
                        XmlAttribute attrA = docc.CreateAttribute("author");
                        attrA.Value = author;
                        node.Attributes.SetNamedItem(attrA);
                        XmlAttribute attrId = docc.CreateAttribute("id");
                        attrId.Value = id;
                        node.Attributes.SetNamedItem(attrId);
                    }
                }
            }
            doc.Save(logPath);

            foreach (DataGridViewRow row in dataGridViewChanges.Rows)
            {
                Console.WriteLine("row: {0} cell:{1}{2} ", row.Index, row.Cells[0].Value, row.Cells[1].Value);
                if (row.Cells[0].Value.ToString().Contains("dropColumn") && (!row.Cells[2].Value.Equals("remove")))
                {
                    Console.WriteLine("need to rename " + row.Cells[1].Value + " to " + row.Cells[2].Value);
                    foreach (string addColumn in renameData)
                    {
                        string[] cDat = addColumn.Split(';');
                        string attributes;

                        try
                        {
                            cDat[3].Split('=')[1].Equals("nullable");
                            attributes = "NOT NULL";
                        }
                        catch (Exception ex)
                        {
                            attributes = "NULL DEFAULT NULL";
                        }

                        if (cDat[2].ToString().Contains("BYTE"))
                        {
                            cDat[2] = cDat[2].Replace(" ", string.Empty);
                            cDat[2] = cDat[2].Replace("BYTE", string.Empty);
                        }
                        else if (cDat[2].ToString().Contains("INT"))
                        {
                            cDat[2] = "INT(11)";
                        }
                        
                        if (row.Cells[2].Value.Equals(cDat[1]))
                        {
                            string queryRenameKey;
                            renameColumn(dbName, destinationUrl, destinationUName, destinationUPass, cDat[0], row.Cells[1].Value.ToString(), row.Cells[2].Value.ToString(), cDat[2], attributes);
                            removeAddColumnAfterRename(cDat[0], cDat[1], row.Cells[1].Value.ToString());
                            foreach (string[] s in renameFKQuery)
                            {
                                s[1] = cDat[1];
                                queryRenameKey = s[0] + s[1] + s[2];
                                runMysqlQuery(destinationUrl,destinationUName,destinationUPass,dbName,queryRenameKey);
                            }

                        }
                    }
                }
            }
            acceptChanges = true;
            this.Close();
        }

        private void ButtonCansel_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        //RENAME COLUMN
        public void renameColumn(string dbname, string url, string username, string userpassword, string tableName,string oldName, string newName, string type , string attributes)
        {
            string constring = "server=" + sourceUrl + ";user=" + sourceUName + ";pwd=" + sourceUPass + ";database=" + dbname + ";";
            string query = "SHOW COLUMNS FROM ac.access_levels_to_user_groups WHERE Field='"+ newName + "';";
            using (MySqlConnection conn = new MySqlConnection(constring))
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        if (rdr[5].ToString().Trim().Equals("auto_increment"))
                        {
                            attributes += " AUTO_INCREMENT";
                        }
                        if (rdr[3].ToString().Trim().Equals("PRI"))
                        {
                            XmlDocument doc = new XmlDocument();
                            doc.Load(logPath);
                            foreach (XmlNode node in doc.DocumentElement)
                            {
                                string author = node.Attributes[0].InnerText;
                                string id = node.Attributes[1].InnerText.Trim();
                                string nodeName = node.Name;

                                foreach (XmlNode chN in node.ChildNodes)
                                {
                                    if (chN.Name.Equals("dropPrimaryKey"))
                                    {                                        
                                        if (chN.Attributes[0].Name.Equals("tableName") && chN.Attributes[0].Value.Equals(tableName))
                                        {
                                            node.RemoveAll();
                                        }
                                        
                                    }
                                }

                                XmlDocument docc = node.OwnerDocument;
                                XmlAttribute attrA = docc.CreateAttribute("author");
                                attrA.Value = author;
                                node.Attributes.SetNamedItem(attrA);
                                XmlAttribute attrId = docc.CreateAttribute("id");
                                attrId.Value = id;
                                node.Attributes.SetNamedItem(attrId);

                                doc.Save(logPath);
                            }
                        }
                        rdr.Close();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                conn.Close();                
            }

            query = "ALTER TABLE `" + dbname + "`.`"+tableName+ "` CHANGE COLUMN `"+oldName.ToString().Split('.')[1]+"` `"+ newName + "` " + type + " " + attributes+"; ";
            runMysqlQuery(url, username, userpassword, dbname, query);
        }

        
        //RUN QUERY ON SERVER 
        public string runMysqlQuery(string url, string username, string userpassword, string dbname, string query)
        {
            string constring = "server=" + url + ";user=" + username + ";pwd=" + userpassword + ";database=" + dbname + ";";
            Console.WriteLine("\nRUN QUERY:\n\t\t"+query);
            using (MySqlConnection conn = new MySqlConnection(constring))
            {
                StringBuilder result = new StringBuilder();
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        result.Append(rdr[0]+" ");
                    }
                    rdr.Close();
                }
                catch (Exception ex)
                {
                    result.Append(ex.ToString().Split('\n')[0]);
                    Console.WriteLine(ex.ToString());
                }
                conn.Close();
                return result.ToString();
            }
        }


        //REMOVE ADD AND DROP NODES in diff log after renaming column
        public void removeAddColumnAfterRename(string tabName,string colName,string oldColName)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(logPath);
            List<string> addColumnList = new List<string>();

            foreach (XmlNode node in doc.DocumentElement)
            {
                string author = node.Attributes[0].InnerText;
                string id = node.Attributes[1].InnerText;
                string nodeName = node.Name;
                //listBoxChangeLog.Items.Add(nodeName + " author: " + author + " id: " + id);
                foreach (XmlNode child in node.ChildNodes)
                {
                    //DELETE addcolumn
                    if (child.Name.Contains("addColumn"))
                    {
                        try
                        {
                            if (child.ChildNodes[0].Attributes["name"].Value.Equals(colName) && child.Attributes["tableName"].Value.Equals(tabName))
                            {
                                Console.WriteLine("!!FOUND!! " + tabName + "." + colName + " for removing");
                                child.ParentNode.RemoveAll();
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("ERROR DELETING");
                        }
                    }
                    //DELETE dropcolumn
                    else if (child.Name.Contains("dropColumn"))
                    {
                        if (child.Attributes["columnName"].Value.Equals(oldColName.ToString().Split('.')[1]) && child.Attributes["tableName"].Value.Equals(tabName))
                        {
                            child.ParentNode.RemoveAll();
                        }
                    }

                    XmlDocument docc = node.OwnerDocument;
                    XmlAttribute attrA = docc.CreateAttribute("author");
                    attrA.Value = author;
                    node.Attributes.SetNamedItem(attrA);
                    XmlAttribute attrId = docc.CreateAttribute("id");
                    attrId.Value = id;
                    node.Attributes.SetNamedItem(attrId);
                    doc.Save(logPath);
                }
            }
        }

    }
}
