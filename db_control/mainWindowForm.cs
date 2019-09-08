using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;


namespace db_control
{
    public partial class mainWindowForm : Form
    {
        //Dump file names
        public string sourceDumpFile { get; set; } = "sourceDump";
        public string destinationDumpFile { get; set; } = "destinationDump";
        //Schema Name which will work with
        public string databseName { get; set; }
        //Accept changes flag
        public bool acceptChanges=false;
        //path to mysql.exe file
        public string mysqlFilePath { get; set; }

        //functions class
        commandGenerate commandGen;
        connections cl;


        public mainWindowForm()
        {
            //this.TopMost = true;
            this.TopLevel = true;
            InitializeComponent();
            //commandGen = new commandGenerate(textBoxDatabase.Text, textBoxSourceUrl.Text, textBoxSourceUName.Text, textBoxSourceUPassword.Text, textBoxDestinationUrl.Text, textBoxDestinationUName.Text, textBoxDestionationUPassword.Text);
            cl = new connections();
            updateConnectionList();

            //By default choosing localhost if exists
            if (cl.connectionList.Exists(con => con.name == "localhost"))
            {
                Console.WriteLine("have localhost");
                comboBoxSourceUrl.SelectedIndex = comboBoxSourceUrl.FindStringExact("localhost");
            }

            //MessageBox.Show("Software under development.\n!! PLEASE MAKE DUMP OF SOURCE && DESTINATION TABLE BEFORE USING THIS SOFT !!", "!!CAUTION!!");

        }

        //refresh comboBoxes with connection names
        public void updateConnectionList()
        {
            Console.WriteLine("updateConnectionList()");
            cl.getConnections();
            comboBoxSourceUrl.Items.Clear();
            comboBoxSourceUrl.ResetText();
            comboBoxDestinationUrl.Items.Clear();
            comboBoxDestinationUrl.ResetText();

            foreach (Connection conn in cl.connectionList)
            {                
                    comboBoxSourceUrl.Items.Add(conn.name);
                    comboBoxDestinationUrl.Items.Add(conn.name);                
            }
        }
        

        //START BUTTON CLICK FUNCTION
        private void ButtonNext_Click(object sender, EventArgs e)
        {
            commandGenerate cg = new commandGenerate();
            
            //check filds
            if(checkFieldsAndConnections())
            {
                databseName = comboBoxDBName.Text;
                mysqlFilePath = textBoPathToMysql.Text;
                commandGen = new commandGenerate(mysqlFilePath, databseName, textBoxSourceUrl.Text, textBoxSourceUName.Text, textBoxSourceUPassword.Text, textBoxDestinationUrl.Text, textBoxDestinationUName.Text, textBoxDestionationUPassword.Text);
                //make dump of source and destination servers , bool value show if was errors
                outputConsole.TopIndex = outputConsole.Items.Add("Start dump from : \'" + textBoxSourceUrl.Text + "\' database: \'" + databseName + "\' user name: \'" + textBoxSourceUName.Text + "\' password: \'" + textBoxSourceUPassword.Text + "\'");
                bool sourseError = commandGen.dumpMysqlProcedures(databseName, textBoxSourceUrl.Text, textBoxSourceUName.Text, textBoxSourceUPassword.Text, sourceDumpFile);
                if (sourseError) { outputConsole.Items.Add("Dump error."); } else { outputConsole.Items.Add("Done."); }
                outputConsole.TopIndex = outputConsole.Items.Add("Start dump: \'" + textBoxDestinationUrl.Text + "\' database: \'" + databseName + "\' user name: \'" + textBoxDestinationUName.Text + "\' password: \'" + textBoxDestionationUPassword.Text+ "\'");
                bool destinationError = commandGen.dumpMysqlProcedures(databseName, textBoxDestinationUrl.Text, textBoxDestinationUName.Text, textBoxDestionationUPassword.Text, destinationDumpFile);
                if (destinationError) { outputConsole.Items.Add("Dump error."); } else { outputConsole.Items.Add("Done."); }
                
                //remove view if was no error
                if (!sourseError && !destinationError)
                {
                    outputConsole.TopIndex = outputConsole.Items.Add("Remove views from: \'" + textBoxSourceUrl.Text + "\' database: \' " + databseName + "\'" );
                    commandGen.remAllViews(textBoxSourceUrl.Text, textBoxSourceUName.Text, textBoxSourceUPassword.Text, databseName);
                    outputConsole.TopIndex = outputConsole.Items.Add("Remove views from: \'" + textBoxDestinationUrl.Text + "\' database: \' " + databseName + "\'");
                    commandGen.remAllViews(textBoxDestinationUrl.Text, textBoxDestinationUName.Text, textBoxDestionationUPassword.Text, databseName);
                }

                //liquibase GENERATE LOG
                string[] genlog = commandGen.generateDiffChangeLog();
                outputConsole.Items.Add("");
                foreach (string line in genlog)
                {
                    outputConsole.TopIndex = outputConsole.Items.Add("\n"+line);                    
                }
                
                //Invoke ChangeLog window
                ChangeLogForm chLogForm = new ChangeLogForm(databseName,textBoxSourceUrl.Text, textBoxSourceUName.Text, textBoxSourceUPassword.Text, textBoxDestinationUrl.Text, textBoxDestinationUName.Text, textBoxDestionationUPassword.Text);
                this.Invoke(new MethodInvoker(() =>
                {
                    chLogForm.ShowDialog();
                }));
                
                
                if (chLogForm.acceptChanges)
                {
                    //liquibase UPDATE DESTINATION
                    string[] upLog = commandGen.updateDestinationDatabase();
                    foreach (string line in upLog)
                    {
                        outputConsole.TopIndex = outputConsole.Items.Add(line);
                    }

                    if (chLogForm.addFKList.Count > 0)
                    {
                        foreach (string addFKQuery in chLogForm.addFKList)
                        {
                            string res = chLogForm.runMysqlQuery(textBoxDestinationUrl.Text, textBoxDestinationUName.Text, textBoxDestionationUPassword.Text, databseName, addFKQuery);
                            if (res.Contains("Exception"))
                            {
                                if (res.Contains("a foreign key constraint fails"))
                                {
                                    string addInfo = "\nClick \"Yes\" if you want to delete rows that cause of error\nClick \"No\" to Do nothing"; 
                                    DialogResult dialogResult = MessageBox.Show(res+addInfo,"ADD FOREIGN KEY ERROR", MessageBoxButtons.YesNo);
                                    //IF USER CHOSE YES THEN REMOVE ALL LINES CAUSED OF ERROR AND RUN AGAIN QUERY
                                    if (dialogResult == DialogResult.Yes)
                                    {
                                        int index = chLogForm.addFKList.FindIndex(i => i.Contains(addFKQuery));
                                        string constraintFailSolutionQuery = chLogForm.constraintFailsSolution[index];
                                        chLogForm.runMysqlQuery(textBoxDestinationUrl.Text, textBoxDestinationUName.Text, textBoxDestionationUPassword.Text, databseName, constraintFailSolutionQuery);
                                        chLogForm.runMysqlQuery(textBoxDestinationUrl.Text, textBoxDestinationUName.Text, textBoxDestionationUPassword.Text, databseName, addFKQuery);
                                    }
                                }
                            }                            
                        }
                    }
                    
                    //restore source views and stored procedures && update destination views and stored procedures
                    outputConsole.TopIndex = outputConsole.Items.Add("Restoring Views and Stored procedures on source database.");
                    commandGen.restoreDumpMusqlProcedures(textBoxSourceUrl.Text, textBoxSourceUName.Text, textBoxSourceUPassword.Text, databseName, sourceDumpFile);
                    outputConsole.TopIndex = outputConsole.Items.Add("Updating Views and Stored procedures on destination database.");
                    commandGen.restoreDumpMusqlProcedures(textBoxDestinationUrl.Text, textBoxDestinationUName.Text, textBoxDestionationUPassword.Text, databseName, sourceDumpFile);
                }
                else
                {
                    //restore source && destination views and stored procedures 
                    outputConsole.TopIndex = outputConsole.Items.Add("Restoring Views and Stored procedures on source database.");
                    commandGen.restoreDumpMusqlProcedures(textBoxSourceUrl.Text, textBoxSourceUName.Text, textBoxSourceUPassword.Text, databseName, sourceDumpFile);
                    outputConsole.TopIndex = outputConsole.Items.Add("Done.");
                    outputConsole.TopIndex = outputConsole.Items.Add("Restoring Views and Stored procedures on destination database.");
                    commandGen.restoreDumpMusqlProcedures(textBoxDestinationUrl.Text, textBoxDestinationUName.Text, textBoxDestionationUPassword.Text, databseName, destinationDumpFile);
                    outputConsole.TopIndex = outputConsole.Items.Add("Done.");
                }

                //remove changelog database tables created by liquibase
                string removeChangelogQuery = "DROP TABLE IF EXISTS " + databseName + ".DATABASECHANGELOGLOCK; "
                                + "DROP TABLE IF EXISTS " + databseName + ".databasechangeloglock; "
                                + "DROP TABLE IF EXISTS " + databseName + ".DATABASECHANGELOG; "
                                + "DROP TABLE IF EXISTS " + databseName + ".databasechangelog; ";

                commandGen.runMysqlQuery(textBoxDestinationUrl.Text , textBoxDestinationUName.Text, textBoxDestionationUPassword.Text, databseName, removeChangelogQuery);
                outputConsole.TopIndex = outputConsole.Items.Add("Removing liquibase change log tables.");
                
                //end of operations message
                outputConsole.TopIndex = outputConsole.Items.Add("\tAll operations finished.");
                outputConsole.TopIndex = outputConsole.Items.Add("");

            }
        }

        //choose Source connection
        private void ComboBoxSourceUrl_SelectedIndexChanged(object sender, EventArgs e)
        {
            Connection cn = cl.connectionList.Find(c => c.name == comboBoxSourceUrl.SelectedItem.ToString());
            textBoxSourceUrl.Text = cn.url;
            textBoxSourceUName.Text = cn.userName;
            textBoxSourceUPassword.Text = cn.password;

        }
        //choose Destination conncection
        private void ComboBoxDestinationUrl_SelectedIndexChanged(object sender, EventArgs e)
        {
            Connection cn = cl.connectionList.Find(c => c.name == comboBoxDestinationUrl.SelectedItem.ToString());
            textBoxDestinationUrl.Text = cn.url;
            textBoxDestinationUName.Text = cn.userName;
            textBoxDestionationUPassword.Text = cn.password;
        }

        private void ButtonEditConnections_Click(object sender, EventArgs e)
        {
            editConnectionForm editConn = new editConnectionForm();

            editConn.ShowDialog();
            updateConnectionList();
        }

        //GETTING SCHEMA LIST FROM SOURCE SERVER
        private void ButtonGetSchemasList_Click(object sender, EventArgs e)
        {
            //check filds
            if (
                textBoxSourceUrl.Text.Length == 0 || 
                textBoxSourceUName.Text.Length == 0 || 
                textBoxSourceUPassword.Text.Length == 0
                )
            {
                MessageBox.Show("Fill all Source server fields", "error");
            }           
            else
            {               
                commandGen = new commandGenerate();
                comboBoxDBName.Items.Clear();
                comboBoxDBName.ResetText();
                List<string> dbs = commandGen.getSchemasList(textBoxSourceUrl.Text, textBoxSourceUName.Text, textBoxSourceUPassword.Text);
                if (dbs.IsEmpty())
                {
                    outputConsole.TopIndex = outputConsole.Items.Add("Schema list get error.");
                }
                else
                {
                    foreach (string dbName in dbs)
                    {
                        if (!dbName.Equals("information_schema") && !dbName.Equals("performance_schema") && !dbName.Equals("mysql") && !dbName.Equals("sys"))
                        {
                            comboBoxDBName.Items.Add(dbName);
                        }
                    }
                    outputConsole.TopIndex = outputConsole.Items.Add("Schema list get success.");
                }
                commandGen = null;
            }
        }
        
        //check empty fields and connections returns true if all fields are not empty and connections are working well
        public bool checkFieldsAndConnections()
        {
            commandGenerate cg = new commandGenerate();
            if (
                comboBoxDBName.Text.Trim().Length == 0 ||
                textBoxSourceUrl.Text.Trim().Length == 0 ||
                textBoxSourceUName.Text.Trim().Length == 0 ||
                textBoxDestinationUrl.Text.Trim().Length == 0 ||
                textBoxSourceUPassword.Text.Trim().Length == 0 ||
                textBoxDestinationUName.Text.Trim().Length == 0 ||
                textBoxDestionationUPassword.Text.Trim().Length == 0 || 
                textBoPathToMysql.Text.Trim().Length== 0
                )
            {
                MessageBox.Show("Fill all fields", "error");
                return false;
            }
            //check connection with source server
            else if (!cg.checkConnection(textBoxSourceUrl.Text, textBoxSourceUName.Text, textBoxSourceUPassword.Text, comboBoxDBName.Text))
            {
                MessageBox.Show("There is an error with connection to source database!\nPlease check connection data.", "error");
                return false;
            }
            //check connection with destination server
            else if (!cg.checkConnection(textBoxDestinationUrl.Text, textBoxDestinationUName.Text, textBoxDestionationUPassword.Text, comboBoxDBName.Text))
            {
                MessageBox.Show("There is an error with connection to destination database!\nPlease check connection data.\nMaybe you need to create schema.", "error");
                return false;
            }
            //start working
            else
            {
                return true;
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            outputConsole.Items.Clear();
        }

        private void ButtonGetMySqlPath_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog() { Description = "Select mysql.exe path." })
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    textBoPathToMysql.Text = fbd.SelectedPath;
                }
            }
        }

        
    }
}
