using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using ScintillaNET;

namespace XMLEdit
{
    public class NotepadPage
    {
        private TabControl _tabControl;
        private TabPage _tabPage;
        private Scintilla _scintillaText;
        private Encoding _textEncoding = Encoding.UTF8;
        private Theme _theme = null;
        private Font _font = null;

        private string _fileName, _filePath;
        private bool _switchToNewTab;

        private const int MaxTabTitleLength = 11;

        public NotepadPage(ref TabControl tabControl, string fileName, Font font, Theme theme, bool switchToNewTab = true, string filePath = null)
        {
            _tabControl = tabControl;
            _fileName = fileName;
            _filePath = filePath;
            _switchToNewTab = switchToNewTab;
            _font = font;
            _theme = theme;

            _scintillaText = new Scintilla();
            _scintillaText.Dock = DockStyle.Fill;
            _scintillaText.WrapMode = WrapMode.None;
            _scintillaText.IndentationGuides = IndentView.LookBoth;
            _scintillaText.Lexer = Lexer.Xml;

            _scintillaText.ScrollWidth = 1;
            _scintillaText.ScrollWidthTracking = true;

            ApplyTheme(theme);

            _tabPage = new TabPage();

            if (_fileName.Length > (MaxTabTitleLength + 3))
                _tabPage.Text = _fileName.Substring(0, MaxTabTitleLength).TrimEnd(' ', '.') + "...";
            else
                _tabPage.Text = _fileName;

            UpdateFilePath();

            _tabPage.Controls.Add(_scintillaText);

            _tabControl.Controls.Add(_tabPage);
            _tabControl.SelectedTab = _tabPage;
            _tabControl.Refresh();

            Focus();
        }

        private void UpdateFilePath()
        {
            if (_filePath != null)
                _tabPage.ToolTipText = Path.GetFullPath(_filePath);
            else
                _tabPage.ToolTipText = _fileName;
        }

        private void ApplyTheme(Theme theme)
        {
            if (_theme != theme) _theme = theme;

            // Reset the styles
            _scintillaText.StyleResetDefault();
            _scintillaText.Styles[Style.Default].Font = _font.Name;
            _scintillaText.Styles[Style.Default].Size = Convert.ToInt32(_font.Size);
            _scintillaText.Styles[Style.Default].Italic = _font.Italic;
            _scintillaText.Styles[Style.Default].Bold = _font.Bold;
            _scintillaText.StyleClearAll();

            // Set the XML Lexer
            _scintillaText.Lexer = Lexer.Xml;

            // Show line numbers
            _scintillaText.Margins[0].Width = 25;

            // Enable folding
            _scintillaText.SetProperty("fold", "1");
            _scintillaText.SetProperty("fold.compact", "1");
            _scintillaText.SetProperty("fold.html", "1");

            // Use Margin 2 for fold markers
            _scintillaText.Margins[2].Type = MarginType.Symbol;
            _scintillaText.Margins[2].Mask = Marker.MaskFolders;
            _scintillaText.Margins[2].Sensitive = true;
            _scintillaText.Margins[2].Width = 20;

            // Reset folder markers
            for (int i = Marker.FolderEnd; i <= Marker.FolderOpen; i++)
            {
                _scintillaText.Markers[i].SetForeColor(theme.FolderMarkerTextColor);
                _scintillaText.Markers[i].SetBackColor(Color.FromArgb(160, 160, 160));
            }

            // Style the folder markers
            _scintillaText.Markers[Marker.Folder].Symbol = MarkerSymbol.BoxPlus;
            _scintillaText.Markers[Marker.Folder].SetBackColor(Color.Black);
            _scintillaText.Markers[Marker.FolderOpen].Symbol = MarkerSymbol.BoxMinus;
            _scintillaText.Markers[Marker.FolderEnd].Symbol = MarkerSymbol.BoxPlusConnected;
            _scintillaText.Markers[Marker.FolderEnd].SetBackColor(Color.Black);
            _scintillaText.Markers[Marker.FolderMidTail].Symbol = MarkerSymbol.TCorner;
            _scintillaText.Markers[Marker.FolderOpenMid].Symbol = MarkerSymbol.BoxMinusConnected;
            _scintillaText.Markers[Marker.FolderSub].Symbol = MarkerSymbol.VLine;
            _scintillaText.Markers[Marker.FolderTail].Symbol = MarkerSymbol.LCorner;

            // Enable automatic folding
            _scintillaText.AutomaticFold = AutomaticFold.Show | AutomaticFold.Click | AutomaticFold.Change;

            // Set the Styles
            _scintillaText.StyleResetDefault();

            _scintillaText.Styles[Style.Default].Font = _font.Name;
            _scintillaText.Styles[Style.Default].Size = Convert.ToInt32(_font.Size);
            _scintillaText.Styles[Style.Default].Italic = _font.Italic;
            _scintillaText.Styles[Style.Default].Bold = _font.Bold;

            _scintillaText.Styles[Style.Default].ForeColor = theme.StandardTextColor;
            _scintillaText.Styles[Style.Default].BackColor = theme.StandardBackgroundColor;

            _scintillaText.StyleClearAll();

            _scintillaText.Styles[Style.LineNumber].ForeColor = theme.LineNumberTextColor;
            _scintillaText.Styles[Style.LineNumber].BackColor = theme.LineNumberBackgroundColor;

            _scintillaText.Styles[Style.Xml.Attribute].ForeColor = theme.XmlAttributeTextColor;
            _scintillaText.Styles[Style.Xml.Attribute].BackColor = theme.XmlAttributeBackgroundColor;

            _scintillaText.Styles[Style.Xml.Entity].ForeColor = theme.XmlEntityTextColor;
            _scintillaText.Styles[Style.Xml.Entity].BackColor = theme.XmlEntityBackgroundColor;

            _scintillaText.Styles[Style.Xml.Comment].ForeColor = theme.XmlCommentTextColor;
            _scintillaText.Styles[Style.Xml.Comment].BackColor = theme.XmlCommentBackgroundColor;

            _scintillaText.Styles[Style.Xml.Tag].ForeColor = theme.XmlTagTextColor;
            _scintillaText.Styles[Style.Xml.Tag].BackColor = theme.XmlTagBackgroundColor;

            _scintillaText.Styles[Style.Xml.TagEnd].ForeColor = theme.XmlTagEndTextColor;
            _scintillaText.Styles[Style.Xml.TagEnd].BackColor = theme.XmlTagEndBackgroundColor;

            _scintillaText.Styles[Style.Xml.DoubleString].ForeColor = theme.XmlDoubleStringTextColor;
            _scintillaText.Styles[Style.Xml.DoubleString].BackColor = theme.XmlDoubleStringBackgroundColor;

            _scintillaText.Styles[Style.Xml.SingleString].ForeColor = theme.XmlSingleStringTextColor;
            _scintillaText.Styles[Style.Xml.SingleString].BackColor = theme.XmlSingleStringBackgroundColor;

            _scintillaText.SetFoldMarginHighlightColor(true, theme.FolderMarkerHighlightColor);
            _scintillaText.SetFoldMarginColor(true, theme.FolderMarkerBackgroundColor);
        }

        public void ResetTheme()
        {
            ApplyTheme(new Theme());
        }

        public bool Focus()
        {
            if (_switchToNewTab) return _scintillaText.Focus();

            return false;
        }

        public void Undo()
        {
            _scintillaText.Undo();
        }

        public void Redo()
        {
            _scintillaText.Redo();
        }

        public Theme Theme
        {
            get { return _theme; }
            set 
            {
                _theme = value;
                ApplyTheme(_theme);
            }
        }

        public string FileName
        {
            get { return _fileName; }
        }

        public string FilePath
        {
            get { return _filePath; }
        }

        public string TabTitle
        {
            get { return _tabPage.Text; }
            set
            {
                if (value.Length > (MaxTabTitleLength + 3))
                    _tabPage.Text = value.Substring(0, MaxTabTitleLength).TrimEnd(' ', '.') + "...";
                else
                    _tabPage.Text = value;
            }
        }

        public string TabToolTip
        {
            get { return _tabPage.ToolTipText; }
            set { _tabPage.ToolTipText = value; }
        }
        
        public Font Font
        {
            get { return _font; }
            set { _font = value; ApplyTheme(_theme); }
        }

        public string Text
        {
            get { return _scintillaText.Text; }
            set { _scintillaText.Text = value; }
        }

        public string SelectedText
        {
            get { return _scintillaText.SelectedText; }
        }

        public void Cut()
        {
            _scintillaText.Cut();
        }

        public void Copy()
        {
            _scintillaText.Copy();
        }

        public void Paste()
        {
            _scintillaText.Paste();
        }

        public void Delete()
        {
            _scintillaText.DeleteRange(_scintillaText.SelectionStart, _scintillaText.SelectionEnd);
        }

        public void SelectAll()
        {
            _scintillaText.SelectAll();
        }

        public LineCollection Lines
        {
            get { return _scintillaText.Lines; }
        }

        public Encoding Encoding
        {
            get { return _textEncoding; }
            set { _textEncoding = value; }
        }

        public string EncodingString
        {
            get
            {
                if (Encoding == Encoding.Default) return "ANSI";
                else if (Encoding == Encoding.ASCII) return "ASCII";
                else if (Encoding == Encoding.UTF8) return "UTF-8";
                else if (Encoding == Encoding.Unicode) return "UTF-16 (LE)";
                else if (Encoding == Encoding.BigEndianUnicode) return "UTF-16 (BE)";
                else if (Encoding == Encoding.UTF7) return "UTF-7";
                else if (Encoding == Encoding.UTF32) return "UTF-32";
                else return Encoding.WebName;
            }
        }

        public bool Saved
        {
            get { return !_scintillaText.Modified; }
        }

        public bool Save()
        {
            if (!String.IsNullOrEmpty(_filePath))
            {
                _scintillaText.SetSavePoint();

                File.WriteAllText(_filePath, this.Text, _textEncoding);

                return true;
            }
            else return SaveAs();
        }

        public bool SaveAs()
        {
            SaveFileDialog saveFileDiag = new SaveFileDialog();
            saveFileDiag.Title = "Save File As";
            saveFileDiag.Filter = "XML Files (*.xml)|*.xml";

            if (saveFileDiag.ShowDialog() == DialogResult.OK)
            {
                _fileName = Path.GetFileName(saveFileDiag.FileName);
                _filePath = Path.GetFullPath(saveFileDiag.FileName);

                _tabPage.Text = _fileName;
                _tabPage.ToolTipText = _fileName;

                _scintillaText.SetSavePoint();

                File.WriteAllText(_filePath, this.Text, _textEncoding);

                return true;
            }
            return false;
        }

        public bool Open()
        {
            OpenFileDialog openFileDiag = new OpenFileDialog();
            openFileDiag.Title = "Open File";
            openFileDiag.Filter = "XML Files (*.xml)|*.xml";

            if (openFileDiag.ShowDialog() == DialogResult.OK)
            {
                _fileName = openFileDiag.SafeFileName;
                _filePath = openFileDiag.FileName;
                TabTitle = _fileName;
                TabToolTip = _filePath;

                UpdateFilePath();

                _tabControl.Refresh();

                _scintillaText.Text = File.ReadAllText(_filePath, _textEncoding);

                Focus();

                _scintillaText.SetSavePoint();
                return true;
            }
            return false;
        }
    }

    public class Theme
    {
        private Color _standardText, _standardBack, _lineNumText, _lineNumBack, _folderMarkerText, _folderMarkerBack, _folderMarkerHigh, _xmlAttFore, _xmlAttBack,
            _xmlEntFore, _xmlEntBack, _xmlComFore, _xmlComBack, _xmlTagFore, _xmlTagBack, _xmlTagEndFore, _xmlTagEndBack, _xmlDsFore, _xmlDsBack, 
            _xmlSsFore, _xmlSsBack;

        public Theme()
        {
            _standardText = Color.Black;
            _standardBack = Color.White;

            _lineNumText = Color.FromArgb(69, 69, 69);
            _lineNumBack = Color.FromArgb(240, 240, 240);

            _folderMarkerText = Color.White;
            _folderMarkerBack = Color.FromArgb(255, 255, 255);
            _folderMarkerHigh = Color.FromArgb(240, 240, 240);

            _xmlAttFore = Color.Red;
            _xmlAttBack = Color.White;

            _xmlEntFore = Color.Red;
            _xmlEntBack = Color.White;

            _xmlComFore = Color.Green;
            _xmlComBack = Color.White;

            _xmlTagFore = Color.Blue;
            _xmlTagBack = Color.White;

            _xmlTagEndFore = Color.Blue;
            _xmlTagEndBack = Color.White;

            _xmlDsFore = Color.DeepPink;
            _xmlDsBack = Color.White;

            _xmlSsFore = Color.DeepPink;
            _xmlSsBack = Color.White;
        }

        public void UnifyBackgrounds(Color color)
        {
            _standardBack = color;
            _xmlAttBack = color;
            _xmlEntBack = color;
            _xmlComBack = color;
            _xmlTagBack = color;
            _xmlTagEndBack = color;
            _xmlDsBack = color;
            _xmlSsBack = color;
        }

        public Color StandardTextColor
        {
            get { return _standardText; }
            set { _standardText = value; }
        }

        public Color StandardBackgroundColor
        {
            get { return _standardBack; }
            set { _standardBack = value; }
        }

        public Color LineNumberTextColor
        {
            get { return _lineNumText; }
            set { _lineNumText = value; }
        }

        public Color LineNumberBackgroundColor
        {
            get { return _lineNumBack; }
            set { _lineNumBack = value; }
        }

        public Color FolderMarkerTextColor
        {
            get { return _folderMarkerText; }
            set { _folderMarkerText = value; }
        }

        public Color FolderMarkerBackgroundColor
        {
            get { return _folderMarkerBack; }
            set { _folderMarkerBack = value; }
        }

        public Color FolderMarkerHighlightColor
        {
            get { return _folderMarkerHigh; }
            set { _folderMarkerHigh = value; }
        }

        public Color XmlAttributeTextColor
        {
            get { return _xmlAttFore; }
            set { _xmlAttFore = value; }
        }

        public Color XmlAttributeBackgroundColor
        {
            get { return _xmlAttBack; }
            set { _xmlAttBack = value; }
        }

        public Color XmlEntityTextColor
        {
            get { return _xmlEntFore; }
            set { _xmlEntFore = value; }
        }

        public Color XmlEntityBackgroundColor
        {
            get { return _xmlEntBack; }
            set { _xmlEntBack = value; }
        }

        public Color XmlCommentTextColor
        {
            get { return _xmlComFore; }
            set { _xmlComFore = value; }
        }

        public Color XmlCommentBackgroundColor
        {
            get { return _xmlComBack; }
            set { _xmlComBack = value; }
        }

        public Color XmlTagTextColor
        {
            get { return _xmlTagFore; }
            set { _xmlTagFore = value; }
        }

        public Color XmlTagBackgroundColor
        {
            get { return _xmlTagBack; }
            set { _xmlTagBack = value; }
        }

        public Color XmlTagEndTextColor
        {
            get { return _xmlTagEndFore; }
            set { _xmlTagEndFore = value; }
        }

        public Color XmlTagEndBackgroundColor
        {
            get { return _xmlTagEndBack; }
            set { _xmlTagEndBack = value; }
        }

        public Color XmlDoubleStringTextColor
        {
            get { return _xmlDsFore; }
            set { _xmlDsFore = value; }
        }

        public Color XmlDoubleStringBackgroundColor
        {
            get { return _xmlDsBack; }
            set { _xmlDsBack = value; }
        }

        public Color XmlSingleStringTextColor
        {
            get { return _xmlSsFore; }
            set { _xmlSsFore = value; }
        }

        public Color XmlSingleStringBackgroundColor
        {
            get { return _xmlSsBack; }
            set { _xmlSsBack = value; }
        }
    }
}
