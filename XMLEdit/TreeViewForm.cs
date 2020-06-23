using System;
using System.Windows.Forms;
using System.Xml;

namespace XMLEdit
{
    public partial class TreeViewForm : Form
    {
        NotepadPage xmlPage;

        public TreeViewForm()
        {
            InitializeComponent();
        }

        public TreeViewForm(ref NotepadPage npPage) : this()
        {
            xmlPage = npPage;

            this.Text = npPage.FileName + " | Tree View";
            InitialiseTree();
        }

        private void InitialiseTree()
        {
            try
            {
                XmlDataDocument xmldoc = new XmlDataDocument();
                XmlNode xmlnode;
                xmldoc.LoadXml(xmlPage.TextboxText);
                xmlnode = xmldoc.ChildNodes[1];
                treeView.Nodes.Clear();
                treeView.Nodes.Add(new TreeNode(xmldoc.DocumentElement.Name));
                TreeNode tNode;
                tNode = treeView.Nodes[0];
                AddTreeNode(xmlnode, tNode);
            }
            catch
            {
                MessageBox.Show("Tree View generation failed!\nPlease ensure the XML document is well-formed and try again!", "Tree View generation failed!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddTreeNode(XmlNode inXmlNode, TreeNode inTreeNode)
        {
            XmlNode xNode;
            TreeNode tNode;
            XmlNodeList nodeList;
            int i = 0;
            if (inXmlNode.HasChildNodes)
            {
                nodeList = inXmlNode.ChildNodes;
                for (i = 0; i <= nodeList.Count - 1; i++)
                {
                    xNode = inXmlNode.ChildNodes[i];
                    inTreeNode.Nodes.Add(new TreeNode(xNode.Name));
                    tNode = inTreeNode.Nodes[i];
                    AddTreeNode(xNode, tNode);
                }
            }
            else
            {
                inTreeNode.Text = inXmlNode.InnerText.ToString();
            }
        }

        private void refreshXML_Click(object sender, EventArgs e)
        {
            InitialiseTree();
        }
    }
}
