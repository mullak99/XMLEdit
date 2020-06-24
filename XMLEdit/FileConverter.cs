using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace XMLEdit
{
    public partial class FileConverter : Form
    {
        private NotepadPage _npPage;
        private XMLEdit _parent;

        public FileConverter()
        {
            InitializeComponent();
        }

        public FileConverter(ref NotepadPage npPage, XMLEdit xmlEdit) : this()
        {
            _parent = xmlEdit;
            _npPage = npPage;
            this.Text = npPage.FileName + " | Converter";
            sourceFileName.Text = npPage.FileName;
            toolTip.SetToolTip(sourceFileName, npPage.FilePath);

            switch (npPage.GetFileType)
            {
                case FileType.XML:
                    {
                        sourceXmlFiletypeCheckBox.Checked = true;
                        sourceJsonFiletypeCheckBox.Checked = false;

                        exportXmlFiletypeCheckBox.Checked = false;
                        exportJsonFiletypeCheckBox.Checked = true;

                        break;
                    }
                case FileType.JSON:
                    {
                        sourceXmlFiletypeCheckBox.Checked = false;
                        sourceJsonFiletypeCheckBox.Checked = true;

                        exportXmlFiletypeCheckBox.Checked = true;
                        exportJsonFiletypeCheckBox.Checked = false;

                        break;
                    }
                default:
                    {
                        sourceXmlFiletypeCheckBox.Checked = false;
                        sourceJsonFiletypeCheckBox.Checked = false;

                        exportXmlFiletypeCheckBox.Checked = false;
                        exportJsonFiletypeCheckBox.Checked = false;

                        break;
                    }
            }
        }

        private void sourceAndExportConfig_CheckedChanged(object sender, EventArgs e)
        {
            sourceXmlFiletypeCheckBox.Checked = !sourceJsonFiletypeCheckBox.Checked;
            exportXmlFiletypeCheckBox.Checked = !exportJsonFiletypeCheckBox.Checked;

            if ((sourceXmlFiletypeCheckBox.Checked && exportJsonFiletypeCheckBox.Checked) || (sourceJsonFiletypeCheckBox.Checked && exportXmlFiletypeCheckBox.Checked))
                convertFile.Enabled = true;
            else
                convertFile.Enabled = false;
        }

        private void overrideSource_CheckedChanged(object sender, EventArgs e)
        {
            sourceFileTypePanel.Enabled = overrideSource.Checked;
        }

        private void convertFile_Click(object sender, EventArgs e)
        {
            if (sourceXmlFiletypeCheckBox.Checked && exportJsonFiletypeCheckBox.Checked)
            {
                //XML to JSON
                SaveFileDialog saveFileDiag = new SaveFileDialog();
                saveFileDiag.Title = "Save File As";
                saveFileDiag.Filter = "JSON Files (*.json)|*.json";

                if (saveFileDiag.ShowDialog() == DialogResult.OK)
                {
                    string fileName = Path.GetFileName(saveFileDiag.FileName);
                    string filePath = Path.GetFullPath(saveFileDiag.FileName);

                    string jsonFile = ConvertXmlToJson(_npPage.TextboxText);
                    File.WriteAllText(filePath, jsonFile);

                    if (openAfterExport.Checked) _parent.OpenTab(filePath);
                    this.Close();
                }
            }
            else if (sourceJsonFiletypeCheckBox.Checked && exportXmlFiletypeCheckBox.Checked)
            {
                //JSON to XML
                SaveFileDialog saveFileDiag = new SaveFileDialog();
                saveFileDiag.Title = "Save File As";
                saveFileDiag.Filter = "XML Files (*.xml)|*.xml";

                if (saveFileDiag.ShowDialog() == DialogResult.OK)
                {
                    string fileName = Path.GetFileName(saveFileDiag.FileName);
                    string filePath = Path.GetFullPath(saveFileDiag.FileName);

                    ConvertJsonToXml(_npPage.TextboxText, filePath);

                    if (openAfterExport.Checked) _parent.OpenTab(filePath);
                    this.Close();
                }
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private string ConvertXmlToJson(string rawXml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(rawXml);
            string jsonRaw = JsonConvert.SerializeXmlNode(doc);

            JToken jt = JToken.Parse(jsonRaw);
            return jt.ToString(Newtonsoft.Json.Formatting.Indented);
        }

        private void ConvertJsonToXml(string rawJson, string outputFilename)
        {
            XmlDocument xmlDoc = JsonConvert.DeserializeXmlNode(rawJson);
            xmlDoc.Save(outputFilename);
        }
    }
}
