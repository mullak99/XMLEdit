using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ScintillaNET;
using AutocompleteMenuNS;
using ScintillaNET_FindReplaceDialog;

namespace XMLEdit
{
    public class NotepadPage : TabPage
    {
        private Scintilla _scintillaText;
        private AutocompleteMenu _autocompleteMenu = new AutocompleteMenu();
        private FindReplace _findDiag;
        private Encoding _textEncoding = Encoding.UTF8;
        private Theme _theme = null;
        private Font _font = null;

        private string _fileName, _filePath;
        private bool _switchToNewTab;

        private const int MaxTabTitleLength = 11;

        public NotepadPage(string fileName, Font font, Theme theme, bool switchToNewTab = true, string filePath = null)
        {
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

            if (_fileName.Length > (MaxTabTitleLength + 3))
                this.Text = _fileName.Substring(0, MaxTabTitleLength).TrimEnd(' ', '.') + "...";
            else
                this.Text = _fileName;

            UpdateFilePath();

            this.Controls.Add(_scintillaText);

            _autocompleteMenu.TargetControlWrapper = new ScintillaWrapper(_scintillaText);
            _autocompleteMenu.SetAutocompleteItems(new DynamicCollection(_scintillaText));
            _autocompleteMenu.AppearInterval = 1000;
            _autocompleteMenu.AllowsTabKey = true;

            _findDiag = new FindReplace(_scintillaText);

            _scintillaText.UpdateUI += NotepadPage_UpdateUI;

            Focus();
            Refresh();
        }

        public void ShowFinder()
        {
            _findDiag.ShowFind();
        }

        public void ShowReplace()
        {
            _findDiag.ShowReplace();
        }

        public void ShowIncSearch()
        {
            _findDiag.ShowIncrementalSearch();
        }

        public void ShowGoTo()
        {
            GoTo goTo = new GoTo(_scintillaText);
            goTo.ShowGoToDialog();
        }

        public void FindNext()
        {
            _findDiag.Window.FindNext();
        }

        public void FindPrev()
        {
            _findDiag.Window.FindPrevious();
        }

        private void UpdateFilePath()
        {
            if (_filePath != null)
                this.ToolTipText = Path.GetFullPath(_filePath);
            else
                this.ToolTipText = _fileName;
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
            _scintillaText.Margins[0].Type = MarginType.Number;

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

            _scintillaText.CaretForeColor = theme.StandardTextColor;

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

            _scintillaText.Styles[Style.BraceLight].ForeColor = theme.BraceGoodColor;
            _scintillaText.Styles[Style.BraceBad].ForeColor = theme.BraceGoodColor;

            _scintillaText.SetFoldMarginHighlightColor(true, theme.FolderMarkerHighlightColor);
            _scintillaText.SetFoldMarginColor(true, theme.FolderMarkerBackgroundColor);

            _scintillaText.SetSelectionBackColor(true, theme.SelectionColor);

            Colors autoCompleteColors = new Colors();

            autoCompleteColors.BackColor = theme.LineNumberBackgroundColor;
            autoCompleteColors.ForeColor = theme.StandardTextColor;
            autoCompleteColors.HighlightingColor = theme.BraceGoodColor;
            autoCompleteColors.SelectedBackColor = theme.SelectionColor;
            autoCompleteColors.SelectedBackColor2 = theme.StandardBackgroundColor;
            autoCompleteColors.SelectedForeColor = theme.StandardTextColor;

            _autocompleteMenu.Colors = autoCompleteColors;
        }

        public void ResetTheme()
        {
            ApplyTheme(new Theme());
        }

        public new bool Focus()
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
            set  { _theme = value; ApplyTheme(_theme); }
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
            get { return this.Text; }
            set
            {
                if (value.Length > (MaxTabTitleLength + 3))
                    this.Text = value.Substring(0, MaxTabTitleLength).TrimEnd(' ', '.') + "...";
                else
                    this.Text = value;
            }
        }

        public string TabToolTip
        {
            get { return this.ToolTipText; }
            set { this.ToolTipText = value; }
        }
        
        public new Font Font
        {
            get { return _font; }
            set { _font = value; ApplyTheme(_theme); }
        }

        public string TextboxText
        {
            get { return _scintillaText.Text; }
            set { _scintillaText.Text = value; }
        }

        public string SelectedText
        {
            get { return _scintillaText.SelectedText; }
        }

        public int Zoom
        {
            get { return _scintillaText.Zoom; }
            set { _scintillaText.Zoom = value; }
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

                File.WriteAllText(_filePath, this.TextboxText, _textEncoding);

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

                this.Text = _fileName;
                this.ToolTipText = _fileName;

                _scintillaText.SetSavePoint();

                File.WriteAllText(_filePath, this.TextboxText, _textEncoding);

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
                return Open(openFileDiag.FileName);

            return false;
        }

        public bool Open(string path)
        {
            if (Path.GetExtension(path) == ".xml")
            {
                _fileName = Path.GetFileName(path);
                _filePath = Path.GetFullPath(path);
                TabTitle = _fileName;
                TabToolTip = _filePath;

                UpdateFilePath();

                _scintillaText.Text = File.ReadAllText(_filePath, _textEncoding);

                Focus();

                _scintillaText.SetSavePoint();
                return true;
            }
            return false;
        }

        private static bool IsBrace(int c)
        {
            switch (c)
            {
                case '(':
                case ')':
                case '[':
                case ']':
                case '{':
                case '}':
                case '<':
                case '>':
                    return true;
            }
            return false;
        }

        int lastCaretPos = 0;

        private void NotepadPage_UpdateUI(object sender, UpdateUIEventArgs e)
        {
            // Has the caret changed position?
            var caretPos = _scintillaText.CurrentPosition;
            if (lastCaretPos != caretPos)
            {
                lastCaretPos = caretPos;
                var bracePos1 = -1;
                var bracePos2 = -1;

                // Is there a brace to the left or right?
                if (caretPos > 0 && IsBrace(_scintillaText.GetCharAt(caretPos - 1)))
                    bracePos1 = (caretPos - 1);
                else if (IsBrace(_scintillaText.GetCharAt(caretPos)))
                    bracePos1 = caretPos;

                if (bracePos1 >= 0)
                {
                    // Find the matching brace
                    bracePos2 = _scintillaText.BraceMatch(bracePos1);
                    if (bracePos2 == Scintilla.InvalidPosition)
                        _scintillaText.BraceBadLight(bracePos1);
                    else
                        _scintillaText.BraceHighlight(bracePos1, bracePos2);
                }
                else
                {
                    // Turn off brace matching
                    _scintillaText.BraceHighlight(Scintilla.InvalidPosition, Scintilla.InvalidPosition);
                }
            }
        }
    }

    public class Theme
    {
        private string _themeID, _themeName;
        private string _themeAuthor = "Unnamed Author";

        private Color _standardText, _standardBack, _lineNumText, _lineNumBack, _folderMarkerText, _folderMarkerBack, _folderMarkerHigh, _xmlAttFore, _xmlAttBack,
            _xmlEntFore, _xmlEntBack, _xmlComFore, _xmlComBack, _xmlTagFore, _xmlTagBack, _xmlTagEndFore, _xmlTagEndBack, _xmlDsFore, _xmlDsBack, 
            _xmlSsFore, _xmlSsBack, _braceGood, _braceBad, _selection;

        public Theme(string themeID, string themeName) : this()
        {
            _themeID = themeID;
            _themeName = themeName;
        }

        public Theme()
        {
            {
                int randInt = new Random().Next(0, 8192);

                if (String.IsNullOrWhiteSpace(_themeName) && !String.IsNullOrWhiteSpace(_themeID))
                    _themeName = _themeID;
                else if (String.IsNullOrWhiteSpace(_themeName) && String.IsNullOrWhiteSpace(_themeID))
                {
                    _themeID = ((DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString() + "_" + randInt;
                    _themeName = "Unnamed Theme " + randInt;
                }              
            }

            _standardText = Color.Black;
            _standardBack = Color.White;

            _lineNumText = Color.FromArgb(69, 69, 69);
            _lineNumBack = Color.FromArgb(240, 240, 240);

            _folderMarkerText = Color.White;
            _folderMarkerBack = Color.FromArgb(255, 255, 255);
            _folderMarkerHigh = Color.FromArgb(240, 240, 240);

            _xmlAttFore = Color.DarkCyan;
            _xmlAttBack = Color.White;

            _xmlEntFore = Color.DarkCyan;
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

            _braceGood = Color.MediumSeaGreen;
            _braceBad = Color.Red;

            _selection = Color.FromArgb(51, 153, 255);
        }

        public string ThemeID
        {
            get { return _themeID; }
            set { _themeID = value; }
        }

        public string ThemeName
        {
            get { return _themeName; }
            set { _themeName = value; }
        }

        public string ThemeAuthor
        {
            get { return _themeAuthor; }
            set { _themeAuthor = value; }
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

        public Color SelectionColor
        {
            get { return _selection; }
            set { _selection = value; }
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

        public Color BraceGoodColor
        {
            get { return _braceGood; }
            set { _braceGood = value; }
        }

        public Color BraceBadColor
        {
            get { return _braceBad; }
            set { _braceBad = value; }
        }
    }

    internal class DynamicCollection : IEnumerable<AutocompleteItem>
    {
        private Scintilla tb;

        public DynamicCollection(Scintilla tb)
        {
            this.tb = tb;
        }

        public IEnumerator<AutocompleteItem> GetEnumerator()
        {
            return BuildList().GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private IEnumerable<AutocompleteItem> BuildList()
        {
            //find all words of the text
            var words = new Dictionary<string, string>();
            foreach (Match m in Regex.Matches(tb.Text, @"\b\w+\b"))
                words[m.Value] = m.Value;

            //return autocomplete items
            foreach (var word in words.Keys)
                yield return new AutocompleteItem(word);
        }
    }
}
