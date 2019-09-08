using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace db_control
{
    public partial class editConnectionForm : Form
    {
        //Declare connection list
        connections cl;

        public editConnectionForm()
        {
            InitializeComponent();
            cl = new connections();
            updateConnectionList();
        }

        //Update Connection names Combobox
        public void updateConnectionList()
        {
            cl.getConnections();
            comboBoxConnectionName.Items.Clear();
            comboBoxConnectionName.ResetText();
            foreach (Connection cn in cl.connectionList)
            {
                comboBoxConnectionName.Items.Add(cn.name);
            }
        }

        //choosing connection
        private void ComboBoxConnectionName_SelectedIndexChanged(object sender, EventArgs e)
        {
            Connection cn = cl.connectionList.Find(c => c.name == comboBoxConnectionName.SelectedItem.ToString());
            textBoxUrl.Text = cn.url;
            textBoxUName.Text = cn.userName;
            textBoxUPassword.Text = cn.password;
            
        }

        //removing connections from connection list and save list to file
        private void ButtonRemove_Click(object sender, EventArgs e)
        {
            if (comboBoxConnectionName.SelectedIndex > -1)
            {
                Connection cn = cl.connectionList.Find(c => c.name == comboBoxConnectionName.SelectedItem.ToString());
                DialogResult res = MessageBox.Show("Are you sure you want to Delete connection: " + cn.name , "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (res == DialogResult.OK)
                {
                    cl.connectionList.Remove(cn);
                    cl.saveConnections();
                    updateConnectionList();
                    clearFields();
                }                             
            }
            else
            {
                MessageBox.Show("You have to choose connection.");
            }

            
        }

        private void ButtonEdit_Click(object sender, EventArgs e)
        {
            if (!(comboBoxConnectionName.Text.Length > 0) ||
                 !(textBoxUrl.Text.Length > 0) ||
                 !(textBoxUName.Text.Length > 0) ||
                 !(textBoxUPassword.Text.Length > 0))
            {
                MessageBox.Show("You have to fill all fields.");
            }
            else
            {
               //check if connection with name entered in comboBox text fiel already exists so get index
                var itemIndex = cl.connectionList.FindIndex(con => con.name == comboBoxConnectionName.Text);
                //if exists change connection object fields and save
                if (itemIndex!=-1)
                {
                    cl.connectionList.ElementAt(itemIndex).url = textBoxUrl.Text;
                    cl.connectionList.ElementAt(itemIndex).userName = textBoxUName.Text;
                    cl.connectionList.ElementAt(itemIndex).password = textBoxUPassword.Text;
                    cl.saveConnections();
                    updateConnectionList();
                    clearFields();
                }
                //if not exists so create a new connection object and save list
                else
                {              
                    Connection cn = new Connection(comboBoxConnectionName.Text, textBoxUrl.Text, textBoxUName.Text, textBoxUPassword.Text);
                    cl.connectionList.Add(cn);
                    cl.saveConnections();
                    updateConnectionList();
                    clearFields();
                }

            }   
        }


        //clear input fields
        public void clearFields()
        {
            textBoxUrl.Text = "";
            textBoxUName.Text = "";
            textBoxUPassword.Text = "";
        }
    }
}
