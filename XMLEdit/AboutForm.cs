using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace XMLEdit
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();

            versionLabel.Text = Program.GetVersion();
        }

        private void AboutForm_Load(object sender, System.EventArgs e)
        {
            versionLabel.Text = Program.GetVersion();

            if (Owner != null)
                Location = new Point(Owner.Location.X + Owner.Width / 2 - Width / 2, Owner.Location.Y + Owner.Height / 2 - Height / 2);
        }

        private void homeLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process.Start("https://gitlab.mullak99.co.uk/mullak99/xmledit/");
            }
            catch { }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (e.CloseReason == CloseReason.WindowsShutDown) return;

            e.Cancel = true;
            Hide();
        }
    }
}
