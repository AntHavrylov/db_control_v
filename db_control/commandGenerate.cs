using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
//using System.Xml;
//using System.Xml.Linq;

namespace db_control
{
    class commandGenerate
    {
        public string database_name { get; set; } // = "his";
        public string mysqlExePath { get; set; }
        public string source_url { get; set; } // = "192.168.1.2:3306";
        public string source_username { get; set; } // = "sniper";
        public string source_userpassword { get; set; } // = "sniper123!";
        public string destination_url { get; set; } // = "localhost:3306";
        public string destination_username { get; set; } // = "root";
        public string destination_userpassword { get; set; } // = "123456";



        //path to Java Driver ,Log File , Dump Procedures File
        const string driverPath = @"liquibase/mysql-connector-java-8.0.16.jar";
        const string logPath = @"db.changelog.xml";
        //const string dumpPath = @"storedProceduresDump.sql";


        //define liquibase process
        Process liquibaseDiffProcess;
        string consoleData;

        Process mysqlProcess;

        //any process
        Process process;

        public commandGenerate() { }
        public commandGenerate(string mysqlFilePath, string dbName, string sUrl, string sName, string sPass, string dUrl, string dName, string dPass) {
            //set all data
            this.mysqlExePath = mysqlFilePath;
            this.database_name = dbName;
            this.source_url = sUrl;
            this.source_username = sName;
            this.source_userpassword = sPass;
            this.destination_url = dUrl;
            this.destination_username = dName;
            this.destination_userpassword = dPass;

            //define liquibase process properties
            liquibaseDiffProcess = new Process();
            liquibaseDiffProcess.StartInfo.CreateNoWindow = true;
            liquibaseDiffProcess.StartInfo.UseShellExecute = false;
            liquibaseDiffProcess.StartInfo.RedirectStandardError = true;
            liquibaseDiffProcess.StartInfo.RedirectStandardOutput = true;
            liquibaseDiffProcess.StartInfo.FileName = @"./liquibase/liquibase.bat";

            
        }

        //generation of change log between databases
        public string[] generateDiffChangeLog()
        {
            //Delete old difference log file if exists
            if (File.Exists(logPath))
            {
                // If file found, delete it    
                File.Delete(logPath);
                Console.WriteLine("Old diff. file deleted.");
                consoleData += "Old diff. file deleted.";
            }

            //command for generation liquibase diffChangeLog
            string liquibase_diffChangeLog_arguments = " --driver=com.mysql.jdbc.Driver --classpath=" + driverPath + " --changeLogFile=" + logPath + " --url=\"jdbc:mysql://" + destination_url + ":3306/" + database_name + "?useUnicode=true&useJDBCCompliantTimezoneShift=true&useLegacyDatetimeCode=false&serverTimezone=UTC\" --username=" + destination_username + " --password=" + destination_userpassword + " --referenceUrl=\"jdbc:mysql://" + source_url + "/" + database_name + "?useUnicode=true&useJDBCCompliantTimezoneShift=true&useLegacyDatetimeCode=false&serverTimezone=UTC\" --referenceUsername=" + source_username + " --referencePassword=" + source_userpassword + " diffChangeLog";

            //Start liquibase Diff Process process
            liquibaseDiffProcess.StartInfo.Arguments = liquibase_diffChangeLog_arguments;
            liquibaseDiffProcess.Start();

            //write output
            Console.WriteLine("Process diffChangeLog started with arguments: " + liquibase_diffChangeLog_arguments);
            string liquibase_diffChangeLog_output = liquibaseDiffProcess.StandardOutput.ReadToEnd();
            liquibaseDiffProcess.WaitForExit();
            Console.WriteLine(liquibase_diffChangeLog_output);
            return liquibase_diffChangeLog_output.Split('\n');
        }

        //updating destination database
        public string[] updateDestinationDatabase()
        {
            //command for applying destination database
            string liquibase_update_arguments = " --driver=com.mysql.jdbc.Driver --classpath=" + driverPath + " --changeLogFile=" + logPath + " --url=\"jdbc:mysql://" + destination_url + ":3306/" + database_name + "?useUnicode=true&useJDBCCompliantTimezoneShift=true&useLegacyDatetimeCode=false&serverTimezone=UTC\" --username=" + destination_username + " --password=" + destination_userpassword + " update";
            Console.WriteLine("Start liquibase update Process process: " + liquibase_update_arguments);
            //Start liquibase update Process process
            liquibaseDiffProcess.StartInfo.Arguments = liquibase_update_arguments;
            liquibaseDiffProcess.Start();

            //write output
            Console.WriteLine("Process liquibase update of " + destination_url + " started with arguments: " + liquibase_update_arguments);
            string liquibase_diffChangeLog_output = liquibaseDiffProcess.StandardOutput.ReadToEnd();
            liquibaseDiffProcess.WaitForExit();
            Console.WriteLine(liquibase_diffChangeLog_output);
            return liquibase_diffChangeLog_output.Split('\n');
        }

        /// <summary>
        /// create dump MySQL procedures & views
        /// </summary>
        /// <param name="dbName"></param>
        /// <param name="url"></param>
        /// <param name="userName"></param>
        /// <param name="userPassword"></param>
        /// <param name="outputFile"></param>
        /// <returns></returns>
        public bool dumpMysqlProcedures(string dbName, string url, string userName, string userPassword, string outputFile)
        {
            bool wasDumpError = false;
            string outP = String.Empty;
            string path = outputFile + ".sql";

            //Delete old dump file if exists
            if (File.Exists(path))
            {
                // If file found, delete it    
                File.Delete(path);
                Console.WriteLine("Old dump. file deleted.");
            }

            string outDump = outputFile + "_out.sql";
            if (File.Exists(outDump))
            {
                File.Delete(outDump);
            }
            //CREATE DUMP WITH NO DATA , ONLY STORED PROCEDURES
            string argu = String.Format(" -h {0} -u {1} -p{2} --routines --no-data {3} > {4}", url, userName, userPassword, dbName, outDump);
            wasDumpError = runCmdProcess(mysqlExePath ,"mysqldump.exe" ,argu ,outP);

            if (!wasDumpError)
            {
                using (StreamWriter sw = File.CreateText(mysqlExePath + "\\" + path))
                {
                    using (StreamReader sr = new StreamReader(mysqlExePath + "\\" + outDump))
                    {
                        string line;
                        int spaceCounter = 0;
                        bool inHead = false;
                        bool inView = false;
                        bool inProcedure = false;
                        int counter = 0;

                        //Read all lines
                        while ((line = sr.ReadLine())!=null)
                        {             
                            //HEADER ADDITION
                            if (line.Contains("-- MySQL dump"))
                            {
                                inHead = true;
                            }
                            else if (inHead == true && line.Trim().Equals("") && spaceCounter == 0)
                            {
                                spaceCounter++;
                            }
                            else if (inHead == true && line.Trim().Equals("") && spaceCounter == 1)
                            {
                                spaceCounter = 0;
                                inHead = false;
                                sw.WriteLine(" ");
                            }
                            if (inHead)
                            {                                
                                sw.WriteLine(line);
                            }

                            //STORED PROCEDURES ADDITION
                            if (line.Contains("DROP PROCEDURE IF EXISTS"))
                            {
                                inProcedure = true;
                            }
                            else if (line.Contains("SET collation_connection  = @saved_col_connection"))
                            {
                                inProcedure = false;
                                sw.WriteLine(" ");
                            }
                            if (inProcedure)
                            {                                
                                sw.WriteLine(line);
                            }

                            //VIEW ADDITION
                            if (line.Contains("DROP VIEW IF EXISTS"))
                            {
                                inView = true;
                                counter++;                                
                            }
                            else if (inView == true && line.Trim().Equals(""))
                            {
                                inView = false;
                                sw.WriteLine(" ");
                            }
                            if (inView)
                            {
                                sw.WriteLine(line);
                            }
                        }
                        sr.Close();
                    }
                    sw.Close();
                }
            }
            return wasDumpError;
        }
        
        /// <summary>
        /// RUNNING CMD PROCESSES WITH ARGUMENTS
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <param name="argumentLine"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        public bool runCmdProcess(string path, string fileName, string argumentLine, string output)
        {
            bool wasError = true;
            string batPath = "run.bat";
            try
            {
                if (File.Exists(batPath))
                {
                    File.Delete(batPath);
                }
                using (FileStream fs = File.Create(batPath))
                {
                    Byte[] script = new UTF8Encoding(true).GetBytes("cd " + path + "\n"+ fileName + " " + argumentLine);
                    fs.Write(script, 0, script.Length);
                }
                wasError = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("BAT Creation error:" + ex);
            }
            //define process settings and run it
            using (process = new Process())
            {
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.Verb = "runas";
                process.StartInfo.FileName = batPath;
                Console.WriteLine("Running process: {0} with args: {1} ",fileName ,argumentLine);
                process.Start();
                string outPut = process.StandardOutput.ReadToEnd();
                Console.WriteLine(outPut);
                process.WaitForExit();
                if (process.ExitCode != 0)
                {
                    wasError = true;
                    string error = process.StandardError.ReadToEnd();
                    Console.WriteLine(error);
                    
                }
            }
            return wasError;
        }

        //Restore stored procedures and views from dump
        public void restoreDumpMusqlProcedures(string url, string username, string userpassword, string dbname, string fileName) {

            string batPath = "runMysqlCmd.bat";
            string argu = String.Format(" -f -h {0} -u {1} -p{2} {3} {4} {5}", url, username, userpassword, dbname, "<", fileName + ".sql");
            try
            {
                if (File.Exists(batPath))
                {
                    File.Delete(batPath);
                }

                using (FileStream fs = File.Create(batPath))
                {
                    Byte[] script = new UTF8Encoding(true).GetBytes("cd " + mysqlExePath + "\nmysql.exe" + argu);
                    fs.Write(script, 0, script.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            //define mysql process properties
            mysqlProcess = new Process();
            mysqlProcess.StartInfo.CreateNoWindow = true;
            mysqlProcess.StartInfo.UseShellExecute = false;
            mysqlProcess.StartInfo.RedirectStandardError = true;
            mysqlProcess.StartInfo.RedirectStandardOutput = true;
            mysqlProcess.StartInfo.FileName = batPath;

            Console.WriteLine("STARTING dump : " + argu);
            mysqlProcess.Start();

            string mysqlOutput = mysqlProcess.StandardOutput.ReadToEnd();
            Console.WriteLine(mysqlOutput);
            mysqlProcess.WaitForExit();
            Console.WriteLine("ENDING dump : " + argu);

            //string path = fileName + ".sql";
            //string constring = "server=" + url + ";user=" + username + ";pwd=" + userpassword + ";database=" + dbname + ";";
            //Console.WriteLine("Starting dump from file to database: " + constring);

            //using (MySqlConnection conn = new MySqlConnection(constring))
            //{
            //    using (MySqlCommand cmd = new MySqlCommand())
            //    {
            //        using (MySqlBackup mb = new MySqlBackup(cmd))
            //        {
            //            cmd.Connection = conn;
            //            conn.Open();
            //            mb.ImportInfo.IgnoreSqlError = true;
            //            mb.ImportFromFile(path);
            //            conn.Close();
            //            Console.WriteLine("connection status: " + conn.State);
            //            Console.WriteLine("Dump loaded successfull.From file: " + path);
            //        }
            //    }
            //}
        }

        //DELETE ALL VIEWS FROM DEST PC it need cause liquibase have problems vith our views
        public void remAllViews(string url, string username, string userpassword, string dbname)
        {
            string constring = "server=" + url + ";user=" + username + ";pwd=" + userpassword + ";database=" + dbname + ";";
            using (MySqlConnection conn = new MySqlConnection(constring))
            {
                StringBuilder viewList = new StringBuilder();
                try
                {
                    conn.Open();
                    string sql = "SELECT TABLE_SCHEMA, TABLE_NAME FROM information_schema.tables WHERE TABLE_TYPE LIKE 'VIEW' AND TABLE_SCHEMA=\"" + database_name + "\" ;";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        viewList.Append(" " + database_name + "." + rdr[1] + ",");

                    }
                    rdr.Close();

                    Console.WriteLine("viewList: " + viewList.ToString());
                    
                    if (viewList.Length > 0)
                    {
                        viewList.Length--;
                        sql = "DROP VIEW IF EXISTS" + viewList.ToString() + ";";
                        Console.WriteLine(sql);
                        cmd = new MySqlCommand(sql, conn);
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        Console.WriteLine("viewList length: " + viewList.Length);
                        Console.WriteLine("THERE ARE NO VIEWS TO DELETE");
                    }

                    //sql = "SET FOREIGN_KEY_CHECKS=1;";
                    //cmd = new MySqlCommand(sql, conn);
                    //cmd.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                conn.Close();
            }

            remAllProcedures(url, username, userpassword, dbname);
        }


        //DROP ALL PROCEDURES
        public void remAllProcedures(string url, string username, string userpassword, string dbname)
        {
            StringBuilder queryBuilder = new StringBuilder();
            string query = "SELECT CONCAT('DROP ', ROUTINE_TYPE, ' `', ROUTINE_SCHEMA, '`.`', ROUTINE_NAME, '`;') as stmt FROM information_schema.ROUTINES WHERE ROUTINE_SCHEMA = '"+ dbname + "';";
            //build one query to remove all stored procedures
            string querys = runMysqlQuery(url, username, userpassword, dbname, query);
            runMysqlQuery(url, username, userpassword, dbname, querys);
        }

        //GET DATABASE SCHEMAS LIST
        public List<string> getSchemasList(string url, string username, string userpassword)
        {
            List<string> schemasList = new List<string>();

            string myConnectionString = "SERVER="+url+";UID='"+username+"';" + "PASSWORD='"+userpassword+"';";
             
            MySqlConnection connection = new MySqlConnection(myConnectionString);
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SHOW DATABASES;";
            MySqlDataReader Reader;
            try
            {
                connection.Open();
                Reader = command.ExecuteReader();
                string row;
                while (Reader.Read())
                {
                    for (int i = 0; i < Reader.FieldCount; i++)
                    {
                        row = Reader.GetValue(i).ToString();
                        schemasList.Add(row);
                    }  
                }
                connection.Close();
                return schemasList;
            }
            catch (Exception e)
            {
                MessageBox.Show("There is a connetcion error.\nCan't get schemas list.", "connection error");
                return schemasList;
            }
        }


        //CHECK CONNECTION
        public bool checkConnection(string url, string username, string userpassword, string dbname)
        {

            string constring = "server=" + url + ";user=" + username + ";pwd=" + userpassword + ";database=" + dbname + ";";
            using (MySqlConnection conn = new MySqlConnection(constring))
            {
                try
                {
                    conn.Open();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }
            return true;
        }


        //SET FOREIGN_KEY_CHECKS <= SELECTOR value; 
        public void setForeignKeyCheck(string url, string username, string userpassword, string dbname,int selector)
        {
            string constring = "server=" + url + ";user=" + username + ";pwd=" + userpassword + ";database=" + dbname + ";";
            using (MySqlConnection conn = new MySqlConnection(constring))
            {
                try
                {
                    conn.Open();
                    string sql = "SET FOREIGN_KEY_CHECKS=" + selector + ";";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                    
                    Console.WriteLine(constring + " | " + sql + "executed");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }


        //RUN QUERY ON SERVER 
        public string runMysqlQuery(string url, string username, string userpassword, string dbname, string query)
        {
            string constring = "server=" + url + ";user=" + username + ";pwd=" + userpassword + ";database=" + dbname + ";";
            Console.WriteLine("\nRUN QUERY:\n\t\t" + query);
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
                        result.Append(rdr[0] + " ");
                    }
                    rdr.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                conn.Close();
                return result.ToString();
            }
        }

    }
}
