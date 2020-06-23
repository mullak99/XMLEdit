namespace XMLEdit
{
    partial class AboutForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
            this.nameLabel = new System.Windows.Forms.Label();
            this.versionLabel = new System.Windows.Forms.Label();
            this.aboutLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.homeLinkLabel = new System.Windows.Forms.LinkLabel();
            this.authorLabel = new System.Windows.Forms.Label();
            this.logoXMLEdit = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.logoXMLEdit)).BeginInit();
            this.SuspendLayout();
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(112, 9);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(47, 13);
            this.nameLabel.TabIndex = 1;
            this.nameLabel.Text = "XMLEdit";
            // 
            // versionLabel
            // 
            this.versionLabel.AutoSize = true;
            this.versionLabel.Location = new System.Drawing.Point(156, 9);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(37, 13);
            this.versionLabel.TabIndex = 2;
            this.versionLabel.Text = "v1.0.0";
            // 
            // aboutLabel
            // 
            this.aboutLabel.Location = new System.Drawing.Point(112, 48);
            this.aboutLabel.Name = "aboutLabel";
            this.aboutLabel.Size = new System.Drawing.Size(160, 58);
            this.aboutLabel.TabIndex = 3;
            this.aboutLabel.Text = "A simple XML Editor";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 114);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Home:";
            // 
            // homeLinkLabel
            // 
            this.homeLinkLabel.AutoSize = true;
            this.homeLinkLabel.Location = new System.Drawing.Point(45, 114);
            this.homeLinkLabel.Name = "homeLinkLabel";
            this.homeLinkLabel.Size = new System.Drawing.Size(187, 13);
            this.homeLinkLabel.TabIndex = 5;
            this.homeLinkLabel.TabStop = true;
            this.homeLinkLabel.Text = "https://github.com/mullak99/XMLEdit";
            this.homeLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.homeLinkLabel_LinkClicked);
            // 
            // authorLabel
            // 
            this.authorLabel.AutoSize = true;
            this.authorLabel.Location = new System.Drawing.Point(112, 24);
            this.authorLabel.Name = "authorLabel";
            this.authorLabel.Size = new System.Drawing.Size(138, 13);
            this.authorLabel.TabIndex = 6;
            this.authorLabel.Text = "by Kallum Blakey (mullak99)";
            // 
            // logoXMLEdit
            // 
            this.logoXMLEdit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.logoXMLEdit.Image = global::XMLEdit.Properties.Resources.XmlEdit_Icon;
            this.logoXMLEdit.Location = new System.Drawing.Point(6, 6);
            this.logoXMLEdit.Name = "logoXMLEdit";
            this.logoXMLEdit.Size = new System.Drawing.Size(100, 100);
            this.logoXMLEdit.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.logoXMLEdit.TabIndex = 0;
            this.logoXMLEdit.TabStop = false;
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 137);
            this.Controls.Add(this.authorLabel);
            this.Controls.Add(this.homeLinkLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.aboutLabel);
            this.Controls.Add(this.versionLabel);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.logoXMLEdit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "AboutForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "XMLEdit - About";
            this.Load += new System.EventHandler(this.AboutForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.logoXMLEdit)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox logoXMLEdit;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.Label aboutLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel homeLinkLabel;
        private System.Windows.Forms.Label authorLabel;
    }
}