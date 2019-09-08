namespace db_control
{
    partial class ChangeLogForm
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
            this.buttonApply = new System.Windows.Forms.Button();
            this.buttonCansel = new System.Windows.Forms.Button();
            this.labelDbNameData = new System.Windows.Forms.Label();
            this.dataGridViewChanges = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewChanges)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonApply
            // 
            this.buttonApply.Location = new System.Drawing.Point(440, 360);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(91, 27);
            this.buttonApply.TabIndex = 1;
            this.buttonApply.Text = "Apply Changes";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.ButtonNext_Click);
            // 
            // buttonCansel
            // 
            this.buttonCansel.Location = new System.Drawing.Point(537, 360);
            this.buttonCansel.Name = "buttonCansel";
            this.buttonCansel.Size = new System.Drawing.Size(91, 27);
            this.buttonCansel.TabIndex = 2;
            this.buttonCansel.Text = "Cansel";
            this.buttonCansel.UseVisualStyleBackColor = true;
            this.buttonCansel.Click += new System.EventHandler(this.ButtonCansel_Click);
            // 
            // labelDbNameData
            // 
            this.labelDbNameData.AutoSize = true;
            this.labelDbNameData.Location = new System.Drawing.Point(16, 10);
            this.labelDbNameData.Name = "labelDbNameData";
            this.labelDbNameData.Size = new System.Drawing.Size(61, 13);
            this.labelDbNameData.TabIndex = 4;
            this.labelDbNameData.Text = "Differences";
            // 
            // dataGridViewChanges
            // 
            this.dataGridViewChanges.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewChanges.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewChanges.Location = new System.Drawing.Point(14, 26);
            this.dataGridViewChanges.Name = "dataGridViewChanges";
            this.dataGridViewChanges.Size = new System.Drawing.Size(610, 325);
            this.dataGridViewChanges.TabIndex = 5;
            // 
            // ChangeLogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(638, 399);
            this.Controls.Add(this.dataGridViewChanges);
            this.Controls.Add(this.labelDbNameData);
            this.Controls.Add(this.buttonCansel);
            this.Controls.Add(this.buttonApply);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ChangeLogForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ChangeLogForm";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewChanges)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonApply;
        private System.Windows.Forms.Button buttonCansel;
        private System.Windows.Forms.Label labelDbNameData;
        private System.Windows.Forms.DataGridView dataGridViewChanges;
    }
}