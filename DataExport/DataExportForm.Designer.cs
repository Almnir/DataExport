namespace DataExport
{
    partial class DataExportForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataExportForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.clearAllLabel = new System.Windows.Forms.LinkLabel();
            this.tablesList = new System.Windows.Forms.ListView();
            this.allLabel = new System.Windows.Forms.LinkLabel();
            this.settingsButton = new System.Windows.Forms.Button();
            this.startExport = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.clearAllLabel);
            this.groupBox1.Controls.Add(this.tablesList);
            this.groupBox1.Controls.Add(this.allLabel);
            this.groupBox1.Location = new System.Drawing.Point(13, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(493, 258);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Выбор таблиц";
            // 
            // clearAllLabel
            // 
            this.clearAllLabel.AutoSize = true;
            this.clearAllLabel.Location = new System.Drawing.Point(106, 20);
            this.clearAllLabel.Name = "clearAllLabel";
            this.clearAllLabel.Size = new System.Drawing.Size(58, 13);
            this.clearAllLabel.TabIndex = 3;
            this.clearAllLabel.TabStop = true;
            this.clearAllLabel.Text = "Снять все";
            this.clearAllLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.clearAllLabel_LinkClicked);
            // 
            // tablesList
            // 
            this.tablesList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tablesList.CheckBoxes = true;
            this.tablesList.Location = new System.Drawing.Point(7, 37);
            this.tablesList.Name = "tablesList";
            this.tablesList.Size = new System.Drawing.Size(480, 215);
            this.tablesList.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.tablesList.TabIndex = 1;
            this.tablesList.UseCompatibleStateImageBehavior = false;
            this.tablesList.View = System.Windows.Forms.View.List;
            // 
            // allLabel
            // 
            this.allLabel.AutoSize = true;
            this.allLabel.Location = new System.Drawing.Point(7, 20);
            this.allLabel.Name = "allLabel";
            this.allLabel.Size = new System.Drawing.Size(77, 13);
            this.allLabel.TabIndex = 1;
            this.allLabel.TabStop = true;
            this.allLabel.Text = "Отметить все";
            this.allLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.allLabel_LinkClicked);
            // 
            // settingsButton
            // 
            this.settingsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.settingsButton.Location = new System.Drawing.Point(10, 291);
            this.settingsButton.Name = "settingsButton";
            this.settingsButton.Size = new System.Drawing.Size(75, 23);
            this.settingsButton.TabIndex = 2;
            this.settingsButton.Text = "Настройки";
            this.settingsButton.UseVisualStyleBackColor = true;
            this.settingsButton.Click += new System.EventHandler(this.settingsButton_Click);
            // 
            // startExport
            // 
            this.startExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.startExport.Location = new System.Drawing.Point(431, 291);
            this.startExport.Name = "startExport";
            this.startExport.Size = new System.Drawing.Size(75, 23);
            this.startExport.TabIndex = 3;
            this.startExport.Text = "Экспорт";
            this.startExport.UseVisualStyleBackColor = true;
            this.startExport.Click += new System.EventHandler(this.startExport_Click);
            // 
            // DataExportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(518, 331);
            this.Controls.Add(this.startExport);
            this.Controls.Add(this.settingsButton);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(200, 200);
            this.Name = "DataExportForm";
            this.Text = "Экспорт данных";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.LinkLabel allLabel;
        private System.Windows.Forms.Button settingsButton;
        private System.Windows.Forms.Button startExport;
        private System.Windows.Forms.ListView tablesList;
        private System.Windows.Forms.LinkLabel clearAllLabel;
    }
}