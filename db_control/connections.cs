using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;


namespace db_control
{
    [Serializable()]
    public class Connection
    {

        public string name { get; set; }        //name of connection
        public string url { get; set; }         //url for connection
        public string userName { get; set; }    //connection username
        public string password { get; set; }    //connection password

        public Connection() { }
        public Connection(string cName, string cUrl, string uName, string uPass)
        {
            name = cName;
            url = cUrl;
            userName = uName;
            password = uPass;
        }

        //overriding toString method for print object data
        public override string ToString()
        {
            return String.Format("name:{0} url:{1} username: {2} password: {3}", name, url, userName, password);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("name", name);
            info.AddValue("url", url);
            info.AddValue("userName", userName);
            info.AddValue("password", password);
        }

        //connection object
        public Connection(SerializationInfo info, StreamingContext context)
        {
            name = (string)info.GetValue("name", typeof(string));
            url = (string)info.GetValue("url", typeof(string));
            userName = (string)info.GetValue("userName", typeof(string));
            password = (string)info.GetValue("password", typeof(string));
        }
    }

    //Class for list of connections 
    class connections
    {
        //data file
        string connectionListPath = "connections.dat";
        //list of connection objects
        public List<Connection> connectionList = new List<Connection>();
       
        public connections()
        {
            //update connection list
            getConnections();
        }
        
        //save changes to data file
        public void saveConnections()
        {
            using (Stream stream = File.Open(connectionListPath, FileMode.Create))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(stream, connectionList);
            }
        }

        //get connections from file
        public void getConnections()
        {
            using (Stream readStream = File.Open(connectionListPath, FileMode.Open))
            {
                BinaryFormatter bf = new BinaryFormatter();
                connectionList = (List<Connection>)bf.Deserialize(readStream);
                readStream.Close(); 
            }
        }
    }
}
