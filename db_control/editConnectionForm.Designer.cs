namespace db_control
{
    partial class editConnectionForm
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
            this.comboBoxConnectionName = new System.Windows.Forms.ComboBox();
            this.textBoxUrl = new System.Windows.Forms.TextBox();
            this.textBoxUName = new System.Windows.Forms.TextBox();
            this.textBoxUPassword = new System.Windows.Forms.TextBox();
            this.buttonEdit = new System.Windows.Forms.Button();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.buttonCheckConnection = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // comboBoxConnectionName
            // 
            this.comboBoxConnectionName.FormattingEnabled = true;
            this.comboBoxConnectionName.Location = new System.Drawing.Point(11, 28);
            this.comboBoxConnectionName.Name = "comboBoxConnectionName";
            this.comboBoxConnectionName.Size = new System.Drawing.Size(231, 21);
            this.comboBoxConnectionName.TabIndex = 0;
            this.comboBoxConnectionName.SelectedIndexChanged += new System.EventHandler(this.ComboBoxConnectionName_SelectedIndexChanged);
            // 
            // textBoxUrl
            // 
            this.textBoxUrl.Location = new System.Drawing.Point(11, 68);
            this.textBoxUrl.Name = "textBoxUrl";
            this.textBoxUrl.Size = new System.Drawing.Size(230, 20);
            this.textBoxUrl.TabIndex = 1;
            // 
            // textBoxUName
            // 
            this.textBoxUName.Location = new System.Drawing.Point(11, 108);
            this.textBoxUName.Name = "textBoxUName";
            this.textBoxUName.Size = new System.Drawing.Size(230, 20);
            this.textBoxUName.TabIndex = 2;
            // 
            // textBoxUPassword
            // 
            this.textBoxUPassword.Location = new System.Drawing.Point(11, 147);
            this.textBoxUPassword.Name = "textBoxUPassword";
            this.textBoxUPassword.Size = new System.Drawing.Size(230, 20);
            this.textBoxUPassword.TabIndex = 3;
            // 
            // buttonEdit
            // 
            this.buttonEdit.Location = new System.Drawing.Point(262, 67);
            this.buttonEdit.Name = "buttonEdit";
            this.buttonEdit.Size = new System.Drawing.Size(88, 21);
            this.buttonEdit.TabIndex = 4;
            this.buttonEdit.Text = "Save ";
            this.buttonEdit.UseVisualStyleBackColor = true;
            this.buttonEdit.Click += new System.EventHandler(this.ButtonEdit_Click);
            // 
            // buttonRemove
            // 
            this.buttonRemove.Location = new System.Drawing.Point(262, 108);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(90, 21);
            this.buttonRemove.TabIndex = 5;
            this.buttonRemove.Text = "Remove";
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new System.EventHandler(this.ButtonRemove_Click);
            // 
            // buttonCheckConnection
            // 
            this.buttonCheckConnection.Location = new System.Drawing.Point(263, 147);
            this.buttonCheckConnection.Name = "buttonCheckConnection";
            this.buttonCheckConnection.Size = new System.Drawing.Size(87, 20);
            this.buttonCheckConnection.TabIndex = 7;
            this.buttonCheckConnection.Text = "Check";
            this.buttonCheckConnection.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Connection name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "URL:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 92);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "User Name:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 131);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Password:";
            // 
            // editConnectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(370, 192);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonCheckConnection);
            this.Controls.Add(this.buttonRemove);
            this.Controls.Add(this.buttonEdit);
            this.Controls.Add(this.textBoxUPassword);
            this.Controls.Add(this.textBoxUName);
            this.Controls.Add(this.textBoxUrl);
            this.Controls.Add(this.comboBoxConnectionName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "editConnectionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "editConnectionForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxConnectionName;
        private System.Windows.Forms.TextBox textBoxUrl;
        private System.Windows.Forms.TextBox textBoxUName;
        private System.Windows.Forms.TextBox textBoxUPassword;
        private System.Windows.Forms.Button buttonEdit;
        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.Button buttonCheckConnection;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}