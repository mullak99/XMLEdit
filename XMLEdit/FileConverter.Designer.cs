namespace XMLEdit
{
    partial class FileConverter
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileConverter));
            this.sourceFileNameLabel = new System.Windows.Forms.Label();
            this.sourceFileName = new System.Windows.Forms.Label();
            this.overrideSource = new System.Windows.Forms.CheckBox();
            this.arrowLabel = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.sourceFileTypePanel = new System.Windows.Forms.Panel();
            this.sourceJsonFiletypeCheckBox = new System.Windows.Forms.CheckBox();
            this.sourceXmlFiletypeCheckBox = new System.Windows.Forms.CheckBox();
            this.exportFileTypePanel = new System.Windows.Forms.Panel();
            this.exportJsonFiletypeCheckBox = new System.Windows.Forms.CheckBox();
            this.exportXmlFiletypeCheckBox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.convertFile = new System.Windows.Forms.Button();
            this.openAfterExport = new System.Windows.Forms.CheckBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.sourceFileTypePanel.SuspendLayout();
            this.exportFileTypePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // sourceFileNameLabel
            // 
            this.sourceFileNameLabel.AutoSize = true;
            this.sourceFileNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sourceFileNameLabel.Location = new System.Drawing.Point(12, 9);
            this.sourceFileNameLabel.Name = "sourceFileNameLabel";
            this.sourceFileNameLabel.Size = new System.Drawing.Size(89, 13);
            this.sourceFileNameLabel.TabIndex = 0;
            this.sourceFileNameLabel.Text = "Source Filename:";
            // 
            // sourceFileName
            // 
            this.sourceFileName.Location = new System.Drawing.Point(102, 9);
            this.sourceFileName.Name = "sourceFileName";
            this.sourceFileName.Size = new System.Drawing.Size(156, 13);
            this.sourceFileName.TabIndex = 1;
            // 
            // overrideSource
            // 
            this.overrideSource.AutoSize = true;
            this.overrideSource.Location = new System.Drawing.Point(12, 121);
            this.overrideSource.Name = "overrideSource";
            this.overrideSource.Size = new System.Drawing.Size(137, 17);
            this.overrideSource.TabIndex = 3;
            this.overrideSource.Text = "Override source filetype";
            this.toolTip.SetToolTip(this.overrideSource, "Overriding the default source filetype can result in invalid files! Only use if t" +
        "he filetype is incorrect.");
            this.overrideSource.UseVisualStyleBackColor = true;
            this.overrideSource.CheckedChanged += new System.EventHandler(this.overrideSource_CheckedChanged);
            // 
            // arrowLabel
            // 
            this.arrowLabel.AutoSize = true;
            this.arrowLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.arrowLabel.Location = new System.Drawing.Point(121, 75);
            this.arrowLabel.Name = "arrowLabel";
            this.arrowLabel.Size = new System.Drawing.Size(26, 26);
            this.arrowLabel.TabIndex = 4;
            this.arrowLabel.Text = ">";
            // 
            // sourceFileTypePanel
            // 
            this.sourceFileTypePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.sourceFileTypePanel.Controls.Add(this.sourceJsonFiletypeCheckBox);
            this.sourceFileTypePanel.Controls.Add(this.sourceXmlFiletypeCheckBox);
            this.sourceFileTypePanel.Enabled = false;
            this.sourceFileTypePanel.Location = new System.Drawing.Point(12, 62);
            this.sourceFileTypePanel.Name = "sourceFileTypePanel";
            this.sourceFileTypePanel.Size = new System.Drawing.Size(100, 53);
            this.sourceFileTypePanel.TabIndex = 5;
            // 
            // sourceJsonFiletypeCheckBox
            // 
            this.sourceJsonFiletypeCheckBox.AutoSize = true;
            this.sourceJsonFiletypeCheckBox.Location = new System.Drawing.Point(3, 26);
            this.sourceJsonFiletypeCheckBox.Name = "sourceJsonFiletypeCheckBox";
            this.sourceJsonFiletypeCheckBox.Size = new System.Drawing.Size(54, 17);
            this.sourceJsonFiletypeCheckBox.TabIndex = 1;
            this.sourceJsonFiletypeCheckBox.Text = "JSON";
            this.sourceJsonFiletypeCheckBox.UseVisualStyleBackColor = true;
            this.sourceJsonFiletypeCheckBox.CheckedChanged += new System.EventHandler(this.sourceAndExportConfig_CheckedChanged);
            // 
            // sourceXmlFiletypeCheckBox
            // 
            this.sourceXmlFiletypeCheckBox.AutoSize = true;
            this.sourceXmlFiletypeCheckBox.Location = new System.Drawing.Point(3, 3);
            this.sourceXmlFiletypeCheckBox.Name = "sourceXmlFiletypeCheckBox";
            this.sourceXmlFiletypeCheckBox.Size = new System.Drawing.Size(48, 17);
            this.sourceXmlFiletypeCheckBox.TabIndex = 0;
            this.sourceXmlFiletypeCheckBox.Text = "XML";
            this.sourceXmlFiletypeCheckBox.UseVisualStyleBackColor = true;
            this.sourceXmlFiletypeCheckBox.CheckedChanged += new System.EventHandler(this.sourceAndExportConfig_CheckedChanged);
            // 
            // exportFileTypePanel
            // 
            this.exportFileTypePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.exportFileTypePanel.Controls.Add(this.exportJsonFiletypeCheckBox);
            this.exportFileTypePanel.Controls.Add(this.exportXmlFiletypeCheckBox);
            this.exportFileTypePanel.Location = new System.Drawing.Point(154, 62);
            this.exportFileTypePanel.Name = "exportFileTypePanel";
            this.exportFileTypePanel.Size = new System.Drawing.Size(100, 53);
            this.exportFileTypePanel.TabIndex = 6;
            // 
            // exportJsonFiletypeCheckBox
            // 
            this.exportJsonFiletypeCheckBox.AutoSize = true;
            this.exportJsonFiletypeCheckBox.Location = new System.Drawing.Point(3, 26);
            this.exportJsonFiletypeCheckBox.Name = "exportJsonFiletypeCheckBox";
            this.exportJsonFiletypeCheckBox.Size = new System.Drawing.Size(54, 17);
            this.exportJsonFiletypeCheckBox.TabIndex = 1;
            this.exportJsonFiletypeCheckBox.Text = "JSON";
            this.exportJsonFiletypeCheckBox.UseVisualStyleBackColor = true;
            this.exportJsonFiletypeCheckBox.CheckedChanged += new System.EventHandler(this.sourceAndExportConfig_CheckedChanged);
            // 
            // exportXmlFiletypeCheckBox
            // 
            this.exportXmlFiletypeCheckBox.AutoSize = true;
            this.exportXmlFiletypeCheckBox.Location = new System.Drawing.Point(3, 3);
            this.exportXmlFiletypeCheckBox.Name = "exportXmlFiletypeCheckBox";
            this.exportXmlFiletypeCheckBox.Size = new System.Drawing.Size(48, 17);
            this.exportXmlFiletypeCheckBox.TabIndex = 0;
            this.exportXmlFiletypeCheckBox.Text = "XML";
            this.exportXmlFiletypeCheckBox.UseVisualStyleBackColor = true;
            this.exportXmlFiletypeCheckBox.CheckedChanged += new System.EventHandler(this.sourceAndExportConfig_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Source Filetype";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(166, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Exported Filetype";
            // 
            // convertFile
            // 
            this.convertFile.Location = new System.Drawing.Point(12, 187);
            this.convertFile.Name = "convertFile";
            this.convertFile.Size = new System.Drawing.Size(116, 23);
            this.convertFile.TabIndex = 9;
            this.convertFile.Text = "Convert File";
            this.convertFile.UseVisualStyleBackColor = true;
            this.convertFile.Click += new System.EventHandler(this.convertFile_Click);
            // 
            // openAfterExport
            // 
            this.openAfterExport.AutoSize = true;
            this.openAfterExport.Checked = true;
            this.openAfterExport.CheckState = System.Windows.Forms.CheckState.Checked;
            this.openAfterExport.Location = new System.Drawing.Point(12, 164);
            this.openAfterExport.Name = "openAfterExport";
            this.openAfterExport.Size = new System.Drawing.Size(132, 17);
            this.openAfterExport.TabIndex = 10;
            this.openAfterExport.Text = "Open after Conversion";
            this.openAfterExport.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(138, 187);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(116, 23);
            this.cancelButton.TabIndex = 11;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // FileConverter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(266, 218);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.openAfterExport);
            this.Controls.Add(this.convertFile);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.exportFileTypePanel);
            this.Controls.Add(this.sourceFileTypePanel);
            this.Controls.Add(this.arrowLabel);
            this.Controls.Add(this.overrideSource);
            this.Controls.Add(this.sourceFileName);
            this.Controls.Add(this.sourceFileNameLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FileConverter";
            this.Text = "PLACEHOLDER | Converter";
            this.TopMost = true;
            this.sourceFileTypePanel.ResumeLayout(false);
            this.sourceFileTypePanel.PerformLayout();
            this.exportFileTypePanel.ResumeLayout(false);
            this.exportFileTypePanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label sourceFileNameLabel;
        private System.Windows.Forms.Label sourceFileName;
        private System.Windows.Forms.CheckBox overrideSource;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label arrowLabel;
        private System.Windows.Forms.Panel sourceFileTypePanel;
        private System.Windows.Forms.CheckBox sourceJsonFiletypeCheckBox;
        private System.Windows.Forms.CheckBox sourceXmlFiletypeCheckBox;
        private System.Windows.Forms.Panel exportFileTypePanel;
        private System.Windows.Forms.CheckBox exportJsonFiletypeCheckBox;
        private System.Windows.Forms.CheckBox exportXmlFiletypeCheckBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button convertFile;
        private System.Windows.Forms.CheckBox openAfterExport;
        private System.Windows.Forms.Button cancelButton;
    }
}