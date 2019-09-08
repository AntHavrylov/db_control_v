namespace db_control
{
    partial class mainWindowForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelDatabase = new System.Windows.Forms.Label();
            this.Source = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxSourceUrl = new System.Windows.Forms.TextBox();
            this.comboBoxSourceUrl = new System.Windows.Forms.ComboBox();
            this.textBoxSourceUPassword = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxSourceUName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxDestinationUrl = new System.Windows.Forms.TextBox();
            this.comboBoxDestinationUrl = new System.Windows.Forms.ComboBox();
            this.textBoxDestionationUPassword = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxDestinationUName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.buttonNext = new System.Windows.Forms.Button();
            this.outputConsole = new System.Windows.Forms.ListBox();
            this.buttonEditConnections = new System.Windows.Forms.Button();
            this.buttonGetSchemasList = new System.Windows.Forms.Button();
            this.comboBoxDBName = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.buttonGetMySqlPath = new System.Windows.Forms.Button();
            this.textBoPathToMysql = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.Source.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelDatabase
            // 
            this.labelDatabase.AutoSize = true;
            this.labelDatabase.Location = new System.Drawing.Point(12, 25);
            this.labelDatabase.Name = "labelDatabase";
            this.labelDatabase.Size = new System.Drawing.Size(84, 13);
            this.labelDatabase.TabIndex = 0;
            this.labelDatabase.Text = "Database Name";
            // 
            // Source
            // 
            this.Source.Controls.Add(this.label7);
            this.Source.Controls.Add(this.textBoxSourceUrl);
            this.Source.Controls.Add(this.comboBoxSourceUrl);
            this.Source.Controls.Add(this.textBoxSourceUPassword);
            this.Source.Controls.Add(this.label3);
            this.Source.Controls.Add(this.textBoxSourceUName);
            this.Source.Controls.Add(this.label2);
            this.Source.Controls.Add(this.label1);
            this.Source.Location = new System.Drawing.Point(16, 99);
            this.Source.Name = "Source";
            this.Source.Size = new System.Drawing.Size(180, 221);
            this.Source.TabIndex = 2;
            this.Source.TabStop = false;
            this.Source.Text = "Source Server";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 24);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(90, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "Connection name";
            // 
            // textBoxSourceUrl
            // 
            this.textBoxSourceUrl.Location = new System.Drawing.Point(10, 87);
            this.textBoxSourceUrl.Name = "textBoxSourceUrl";
            this.textBoxSourceUrl.Size = new System.Drawing.Size(164, 20);
            this.textBoxSourceUrl.TabIndex = 7;
            // 
            // comboBoxSourceUrl
            // 
            this.comboBoxSourceUrl.FormattingEnabled = true;
            this.comboBoxSourceUrl.Location = new System.Drawing.Point(10, 39);
            this.comboBoxSourceUrl.Name = "comboBoxSourceUrl";
            this.comboBoxSourceUrl.Size = new System.Drawing.Size(164, 21);
            this.comboBoxSourceUrl.TabIndex = 6;
            this.comboBoxSourceUrl.SelectedIndexChanged += new System.EventHandler(this.ComboBoxSourceUrl_SelectedIndexChanged);
            // 
            // textBoxSourceUPassword
            // 
            this.textBoxSourceUPassword.Location = new System.Drawing.Point(10, 182);
            this.textBoxSourceUPassword.Name = "textBoxSourceUPassword";
            this.textBoxSourceUPassword.Size = new System.Drawing.Size(164, 20);
            this.textBoxSourceUPassword.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 166);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(115, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Source User Password";
            // 
            // textBoxSourceUName
            // 
            this.textBoxSourceUName.Location = new System.Drawing.Point(10, 132);
            this.textBoxSourceUName.Name = "textBoxSourceUName";
            this.textBoxSourceUName.Size = new System.Drawing.Size(164, 20);
            this.textBoxSourceUName.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 117);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Source User Name";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Source URL";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.textBoxDestinationUrl);
            this.groupBox2.Controls.Add(this.comboBoxDestinationUrl);
            this.groupBox2.Controls.Add(this.textBoxDestionationUPassword);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.textBoxDestinationUName);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Location = new System.Drawing.Point(202, 99);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(180, 221);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Destination Server";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 24);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(90, 13);
            this.label8.TabIndex = 9;
            this.label8.Text = "Connection name";
            // 
            // textBoxDestinationUrl
            // 
            this.textBoxDestinationUrl.Location = new System.Drawing.Point(9, 87);
            this.textBoxDestinationUrl.Name = "textBoxDestinationUrl";
            this.textBoxDestinationUrl.Size = new System.Drawing.Size(164, 20);
            this.textBoxDestinationUrl.TabIndex = 8;
            // 
            // comboBoxDestinationUrl
            // 
            this.comboBoxDestinationUrl.FormattingEnabled = true;
            this.comboBoxDestinationUrl.Location = new System.Drawing.Point(9, 40);
            this.comboBoxDestinationUrl.Name = "comboBoxDestinationUrl";
            this.comboBoxDestinationUrl.Size = new System.Drawing.Size(164, 21);
            this.comboBoxDestinationUrl.TabIndex = 7;
            this.comboBoxDestinationUrl.SelectedIndexChanged += new System.EventHandler(this.ComboBoxDestinationUrl_SelectedIndexChanged);
            // 
            // textBoxDestionationUPassword
            // 
            this.textBoxDestionationUPassword.Location = new System.Drawing.Point(9, 183);
            this.textBoxDestionationUPassword.Name = "textBoxDestionationUPassword";
            this.textBoxDestionationUPassword.Size = new System.Drawing.Size(164, 20);
            this.textBoxDestionationUPassword.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 167);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(115, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Source User Password";
            // 
            // textBoxDestinationUName
            // 
            this.textBoxDestinationUName.Location = new System.Drawing.Point(9, 133);
            this.textBoxDestinationUName.Name = "textBoxDestinationUName";
            this.textBoxDestinationUName.Size = new System.Drawing.Size(164, 20);
            this.textBoxDestinationUName.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 117);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(97, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Source User Name";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 71);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(66, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Source URL";
            // 
            // buttonNext
            // 
            this.buttonNext.Location = new System.Drawing.Point(181, 575);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(117, 38);
            this.buttonNext.TabIndex = 7;
            this.buttonNext.Text = "Start";
            this.buttonNext.UseVisualStyleBackColor = true;
            this.buttonNext.Click += new System.EventHandler(this.ButtonNext_Click);
            // 
            // outputConsole
            // 
            this.outputConsole.FormattingEnabled = true;
            this.outputConsole.HorizontalScrollbar = true;
            this.outputConsole.Location = new System.Drawing.Point(388, 25);
            this.outputConsole.Name = "outputConsole";
            this.outputConsole.Size = new System.Drawing.Size(588, 589);
            this.outputConsole.TabIndex = 8;
            // 
            // buttonEditConnections
            // 
            this.buttonEditConnections.Location = new System.Drawing.Point(261, 326);
            this.buttonEditConnections.Name = "buttonEditConnections";
            this.buttonEditConnections.Size = new System.Drawing.Size(119, 25);
            this.buttonEditConnections.TabIndex = 9;
            this.buttonEditConnections.Text = "Edit Connection List";
            this.buttonEditConnections.UseVisualStyleBackColor = true;
            this.buttonEditConnections.Click += new System.EventHandler(this.ButtonEditConnections_Click);
            // 
            // buttonGetSchemasList
            // 
            this.buttonGetSchemasList.Location = new System.Drawing.Point(16, 72);
            this.buttonGetSchemasList.Name = "buttonGetSchemasList";
            this.buttonGetSchemasList.Size = new System.Drawing.Size(180, 21);
            this.buttonGetSchemasList.TabIndex = 10;
            this.buttonGetSchemasList.Text = "Get Source DataBase list";
            this.buttonGetSchemasList.UseVisualStyleBackColor = true;
            this.buttonGetSchemasList.Click += new System.EventHandler(this.ButtonGetSchemasList_Click);
            // 
            // comboBoxDBName
            // 
            this.comboBoxDBName.FormattingEnabled = true;
            this.comboBoxDBName.Location = new System.Drawing.Point(16, 45);
            this.comboBoxDBName.Name = "comboBoxDBName";
            this.comboBoxDBName.Size = new System.Drawing.Size(180, 21);
            this.comboBoxDBName.TabIndex = 11;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(300, 575);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(74, 38);
            this.button1.TabIndex = 12;
            this.button1.Text = "Clear console";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // buttonGetMySqlPath
            // 
            this.buttonGetMySqlPath.Location = new System.Drawing.Point(323, 357);
            this.buttonGetMySqlPath.Name = "buttonGetMySqlPath";
            this.buttonGetMySqlPath.Size = new System.Drawing.Size(57, 21);
            this.buttonGetMySqlPath.TabIndex = 13;
            this.buttonGetMySqlPath.Text = "Open";
            this.buttonGetMySqlPath.UseVisualStyleBackColor = true;
            this.buttonGetMySqlPath.Click += new System.EventHandler(this.ButtonGetMySqlPath_Click);
            // 
            // textBoPathToMysql
            // 
            this.textBoPathToMysql.Location = new System.Drawing.Point(72, 357);
            this.textBoPathToMysql.Name = "textBoPathToMysql";
            this.textBoPathToMysql.Size = new System.Drawing.Size(246, 20);
            this.textBoPathToMysql.TabIndex = 14;
            this.textBoPathToMysql.Text = "C:\\Program Files\\MySQL\\MySQL Server 8.0\\bin";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(10, 359);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(62, 13);
            this.label9.TabIndex = 15;
            this.label9.Text = "Mysql Path:";
            // 
            // mainWindowForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(996, 631);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.textBoPathToMysql);
            this.Controls.Add(this.buttonGetMySqlPath);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.comboBoxDBName);
            this.Controls.Add(this.buttonGetSchemasList);
            this.Controls.Add(this.buttonEditConnections);
            this.Controls.Add(this.outputConsole);
            this.Controls.Add(this.buttonNext);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.Source);
            this.Controls.Add(this.labelDatabase);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "mainWindowForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "db_control";
            this.Source.ResumeLayout(false);
            this.Source.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label labelDatabase;
        private System.Windows.Forms.GroupBox Source;
        private System.Windows.Forms.TextBox textBoxSourceUPassword;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxSourceUName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBoxDestionationUPassword;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxDestinationUName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button buttonNext;
        private System.Windows.Forms.ListBox outputConsole;
        private System.Windows.Forms.ComboBox comboBoxSourceUrl;
        private System.Windows.Forms.ComboBox comboBoxDestinationUrl;
        private System.Windows.Forms.TextBox textBoxSourceUrl;
        private System.Windows.Forms.TextBox textBoxDestinationUrl;
        private System.Windows.Forms.Button buttonEditConnections;
        private System.Windows.Forms.Button buttonGetSchemasList;
        private System.Windows.Forms.ComboBox comboBoxDBName;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button buttonGetMySqlPath;
        private System.Windows.Forms.TextBox textBoPathToMysql;
        private System.Windows.Forms.Label label9;
    }
}

