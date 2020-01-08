using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace XMLEdit
{
    public partial class XMLEdit : Form
    {
        List<NotepadPage> notepadPages = new List<NotepadPage>();
        List<string> tabFileNames = new List<string>();

        AboutForm _aboutForm = new AboutForm();

        Font _defaultFont = new Font("Consolas", 10);

        Theme _defaultTheme = new Theme();
        Theme _darkTheme = new Theme();
        Theme _pureBlackTheme = new Theme();

        public XMLEdit()
        {
            InitializeComponent();
            _aboutForm.Owner = this;

            #region Default Themes
            _darkTheme.StandardTextColor = Color.White;
            _darkTheme.StandardBackgroundColor = Color.FromArgb(30, 30, 30);
            _darkTheme.LineNumberTextColor = Color.LightGray;
            _darkTheme.LineNumberBackgroundColor = Color.FromArgb(60, 60, 60);
            _darkTheme.FolderMarkerTextColor = Color.LightGray;
            _darkTheme.FolderMarkerBackgroundColor = Color.FromArgb(60, 60, 60);
            _darkTheme.FolderMarkerHighlightColor = Color.FromArgb(75, 75, 75);
            _darkTheme.XmlAttributeTextColor = Color.Crimson;
            _darkTheme.XmlEntityTextColor = Color.Crimson;
            _darkTheme.XmlTagTextColor = Color.LightSkyBlue;
            _darkTheme.XmlTagEndTextColor = Color.LightSkyBlue;
            _darkTheme.UnifyBackgrounds(Color.FromArgb(30, 30, 30));

            _pureBlackTheme.StandardTextColor = Color.White;
            _pureBlackTheme.StandardBackgroundColor = Color.FromArgb(0, 0, 0);
            _pureBlackTheme.LineNumberTextColor = Color.LightGray;
            _pureBlackTheme.LineNumberBackgroundColor = Color.FromArgb(30, 30, 30);
            _pureBlackTheme.FolderMarkerTextColor = Color.LightGray;
            _pureBlackTheme.FolderMarkerBackgroundColor = Color.FromArgb(30, 30, 30);
            _pureBlackTheme.FolderMarkerHighlightColor = Color.FromArgb(45, 45, 45);
            _pureBlackTheme.XmlAttributeTextColor = Color.Crimson;
            _pureBlackTheme.XmlEntityTextColor = Color.Crimson;
            _pureBlackTheme.XmlTagTextColor = Color.LightSkyBlue;
            _pureBlackTheme.XmlTagEndTextColor = Color.LightSkyBlue;
            _pureBlackTheme.UnifyBackgrounds(Color.FromArgb(0, 0, 0));
            #endregion
        }

        #region Methods

        private string GetNewFileName(Int64 newFileNameIntStart = -1)
        {
            Int64 newFileNameInt;
            if (newFileNameIntStart == -1)
                newFileNameInt = 1;
            else newFileNameInt = newFileNameIntStart + 1;

            string tabFileName = String.Format("New {0}", newFileNameInt);

            if (tabFileNames.Exists(s => s == tabFileName))
            {
                tabFileName = String.Format("New {0}", newFileNameInt + 1);
                return GetNewFileName(newFileNameInt);
            }
            else
            {
                tabFileNames.Add(tabFileName);
                return tabFileName;
            }
        }

        private void ApplyTheme(Theme theme)
        {
            foreach (NotepadPage npPage in notepadPages)
            {
                npPage.Theme = theme;
            }
        }

        #endregion
        #region Tabs

        private void ForceRemoveTabAt(int index)
        {
            string fileName = notepadPages[index].FileName;

            notepadPages.RemoveAt(index);
            TabbedNotepad.TabPages.RemoveAt(index);
            tabFileNames.Remove(fileName);            

            if (TabbedNotepad.TabCount > 0) TabbedNotepad.SelectedIndex = (TabbedNotepad.TabCount - 1);
        }

        private void AddTab(NotepadPage notepadPage)
        {
            notepadPages.Add(notepadPage);
            notepadPage.Focus();
        }

        private bool CloseTabAt(int index)
        {
            bool ret = false;
            try
            {
                if (notepadPages[index].Saved)
                {
                    if (TabbedNotepad.TabCount > 1 || !String.IsNullOrEmpty(notepadPages[index].Text))
                    {
                        ForceRemoveTabAt(index);
                        ret = true;
                    }
                }
                else
                {
                    DialogResult diagResult = MessageBox.Show(String.Format("Do you want to close '{0}' without saving?", notepadPages[index].FileName), "Close without saving?", MessageBoxButtons.YesNoCancel);

                    if (diagResult == DialogResult.Yes)
                    {
                        ForceRemoveTabAt(index);
                        ret = true;
                    }
                    else if (diagResult == DialogResult.No && notepadPages[index].Save())
                    {
                        ForceRemoveTabAt(index);
                        ret = true;
                    }

                    if (TabbedNotepad.TabPages.Count == 0) NewToolStripMenuItem_Click(null, null);
                }
            }
            catch { }

            return ret;
        }

        private bool CloseAllTabs()
        {
            bool didAllClose = true;
            NotepadPage[] tempPages = new NotepadPage[notepadPages.Count];
            notepadPages.CopyTo(tempPages);

            foreach (NotepadPage npPage in tempPages)
            {
                if (!CloseTabAt(notepadPages.IndexOf(npPage)) && !npPage.Saved)
                    didAllClose = false;
            }
            return didAllClose;
        }

        private void TabbedNotepad_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.Graphics.DrawString("x", e.Font, Brushes.Black, e.Bounds.Right - 15, e.Bounds.Top + 4);
            e.Graphics.DrawString(this.TabbedNotepad.TabPages[e.Index].Text, e.Font, Brushes.Black, e.Bounds.Left + 12, e.Bounds.Top + 4);
            e.DrawFocusRectangle();
        }

        private void TabbedNotepad_MouseDown(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < this.TabbedNotepad.TabPages.Count; i++)
            {
                Rectangle r = TabbedNotepad.GetTabRect(i);

                Rectangle closeButton = new Rectangle(r.Right - 15, r.Top + 4, 9, 7);
                if (closeButton.Contains(e.Location))
                {
                    CloseTabAt(i);
                    break;
                }
            }
        }

        private void TabbedNotepad_TabIndexChanged(object sender, EventArgs e)
        {
            Runtime_Tick(sender, e);
        }

        private void TabbedNotepad_Click(object sender, EventArgs e)
        {
            foreach (NotepadPage npPage in notepadPages)
            {
                npPage.Focus();
            }
        }

        #endregion
        #region MenuToolStrip

        private void NewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NotepadPage notepadPage = new NotepadPage(ref TabbedNotepad, GetNewFileName(), _defaultFont, _defaultTheme);
            AddTab(notepadPage);
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NotepadPage notepadPage;
            string oldTabFileName = "";

            if (notepadPages.Count > 0 && !String.IsNullOrWhiteSpace(notepadPages[notepadPages.Count - 1].Text))
                notepadPage = new NotepadPage(ref TabbedNotepad, "", _defaultFont, _defaultTheme);
            else
            {
                oldTabFileName = notepadPages[notepadPages.Count - 1].FileName;
                notepadPage = notepadPages[notepadPages.Count - 1];
            }

            bool opened = notepadPage.Open();
            if (opened)
            {
                if (!String.IsNullOrWhiteSpace(oldTabFileName)) tabFileNames.Remove(oldTabFileName);
                AddTab(notepadPage);
            }
            else CloseTabAt(TabbedNotepad.SelectedIndex);
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            notepadPages[TabbedNotepad.SelectedIndex].Save();
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            notepadPages[TabbedNotepad.SelectedIndex].SaveAs();
        }

        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseTabAt(TabbedNotepad.SelectedIndex);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseAllTabs();
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            notepadPages[TabbedNotepad.SelectedIndex].Cut();
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            notepadPages[TabbedNotepad.SelectedIndex].Copy();
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            notepadPages[TabbedNotepad.SelectedIndex].Paste();
        }

        private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            notepadPages[TabbedNotepad.SelectedIndex].Delete();
        }

        private void SelectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            notepadPages[TabbedNotepad.SelectedIndex].SelectAll();
        }

        private void UndoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            notepadPages[TabbedNotepad.SelectedIndex].Undo();
        }

        private void RedoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            notepadPages[TabbedNotepad.SelectedIndex].Redo();
        }

        private void FontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontDialog fontDiag = new FontDialog();
            fontDiag.ShowColor = false;
            fontDiag.ShowEffects = false;
            fontDiag.AllowScriptChange = false;
            fontDiag.Font = _defaultFont;
            
            DialogResult result = fontDiag.ShowDialog();

            if (result != DialogResult.Cancel)
            {
                _defaultFont = fontDiag.Font;

                foreach (NotepadPage npPage in notepadPages)
                {
                    npPage.Font = _defaultFont;
                }
            }
        }

        private void SetDefaultThemeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sender == DefaultToolStripMenuItem)
            {
                ApplyTheme(_defaultTheme);

                DefaultToolStripMenuItem.Checked = true;
                DarkModeToolStripMenuItem.Checked = false;
                PureBlackModeToolStripMenuItem.Checked = false;
            }
            else if (sender == DarkModeToolStripMenuItem)
            {
                ApplyTheme(_darkTheme);

                DefaultToolStripMenuItem.Checked = false;
                DarkModeToolStripMenuItem.Checked = true;
                PureBlackModeToolStripMenuItem.Checked = false;
            }
            else if (sender == PureBlackModeToolStripMenuItem)
            {
                ApplyTheme(_pureBlackTheme);

                DefaultToolStripMenuItem.Checked = false;
                DarkModeToolStripMenuItem.Checked = false;
                PureBlackModeToolStripMenuItem.Checked = true;
            }
        }

        private void SetEncodingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sender == ANSIToolStripMenuItem)
            {
                notepadPages[TabbedNotepad.SelectedIndex].Encoding = Encoding.Default;
                ANSIToolStripMenuItem.Checked = true;
                ASCIIToolStripMenuItem.Checked = false;
                UTF8ToolStripMenuItem.Checked = false;
            }
            else if (sender == ASCIIToolStripMenuItem)
            {
                notepadPages[TabbedNotepad.SelectedIndex].Encoding = Encoding.ASCII;
                ANSIToolStripMenuItem.Checked = false;
                ASCIIToolStripMenuItem.Checked = true;
                UTF8ToolStripMenuItem.Checked = false;
            }
            else if (sender == UTF8ToolStripMenuItem)
            {
                notepadPages[TabbedNotepad.SelectedIndex].Encoding = Encoding.UTF8;
                ANSIToolStripMenuItem.Checked = false;
                ASCIIToolStripMenuItem.Checked = false;
                UTF8ToolStripMenuItem.Checked = true;
            }
        }

        private void AboutXMLEditToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_aboutForm.Visible)
                _aboutForm.BringToFront();
            else
                _aboutForm.Show();
        }

        #endregion
        #region SubToolStrip

        private void NewToolStripButton_Click(object sender, EventArgs e)
        {
            NewToolStripMenuItem_Click(sender, e);
        }

        private void OpenToolStripButton_Click(object sender, EventArgs e)
        {
            OpenToolStripMenuItem_Click(sender, e);
        }

        private void SaveToolStripButton_Click(object sender, EventArgs e)
        {
            SaveToolStripMenuItem_Click(sender, e);
        }

        private void SaveAsToolStripButton_Click(object sender, EventArgs e)
        {
            SaveAsToolStripMenuItem_Click(sender, e);
        }

        private void CutToolStripButton_Click(object sender, EventArgs e)
        {
            CutToolStripMenuItem_Click(sender, e);
        }

        private void CopyStripButton_Click(object sender, EventArgs e)
        {
            CopyToolStripMenuItem_Click(sender, e);
        }

        private void PasteStripButton_Click(object sender, EventArgs e)
        {
            PasteToolStripMenuItem_Click(sender, e);
        }

        private void UndoStripButton_Click(object sender, EventArgs e)
        {
            UndoToolStripMenuItem_Click(sender, e);
        }

        private void RedoStripButton_Click(object sender, EventArgs e)
        {
            RedoToolStripMenuItem_Click(sender, e);
        }

        private void FontToolStripButton_Click(object sender, EventArgs e)
        {
            FontToolStripMenuItem_Click(sender, e);
        }
        private void CloseToolStripButton_Click(object sender, EventArgs e)
        {
            if (ModifierKeys == Keys.Shift)
                CloseAllToolStripMenuItem_Click(sender, e);
            else
                CloseToolStripMenuItem_Click(sender, e);
        }

        private void CloseToolStripButton_MouseHover(object sender, EventArgs e)
        {
            if (ModifierKeys == Keys.Shift)
                CloseToolStripButton.ToolTipText = "Close All";
            else
                CloseToolStripButton.ToolTipText = "Close";
        }

        #endregion
        #region Misc UI

        private void XMLEdit_Load(object sender, EventArgs e)
        {
            if (notepadPages.Count == 0) NewToolStripMenuItem_Click(sender, e);
        }

        private void Runtime_Tick(object sender, EventArgs e)
        {
            try
            {
                if (TabbedNotepad.TabCount == 0) NewToolStripMenuItem_Click(sender, e);

                char[] delimiters = new char[] { ' ', '\r', '\n' };

                int textLen = notepadPages[TabbedNotepad.SelectedIndex].Text.Length;
                int lineLen = notepadPages[TabbedNotepad.SelectedIndex].Lines.Count;
                int wordCount = notepadPages[TabbedNotepad.SelectedIndex].Text.Split(delimiters, StringSplitOptions.RemoveEmptyEntries).Length;

                TextLengthLabel.Text = "Length: " + textLen;

                if (lineLen > 0) LineTotalLabel.Text = "Lines: " + lineLen;
                else LineTotalLabel.Text = "Lines: 1";

                WordTotalLabel.Text = "Words: " + wordCount;

                TextLengthLabel.ToolTipText = String.Format("Total characters in {0}: {1}", notepadPages[TabbedNotepad.SelectedIndex].FileName, textLen);
                LineTotalLabel.ToolTipText = String.Format("Total lines in {0}: {1}", notepadPages[TabbedNotepad.SelectedIndex].FileName, lineLen);
                WordTotalLabel.ToolTipText = String.Format("Total words in {0}: {1}", notepadPages[TabbedNotepad.SelectedIndex].FileName, wordCount);

                string TabTitle, TabToolTip;
                if (notepadPages[TabbedNotepad.SelectedIndex].Saved)
                {
                    try
                    {
                        TabTitle = notepadPages[TabbedNotepad.SelectedIndex].FileName.Substring(0, 11).TrimEnd(' ', '.') + "...";
                    }
                    catch
                    {
                        TabTitle = notepadPages[TabbedNotepad.SelectedIndex].FileName;
                    }

                    TabToolTip = notepadPages[TabbedNotepad.SelectedIndex].FileName;
                    this.Text = String.Format("{0} - XMLEdit", notepadPages[TabbedNotepad.SelectedIndex].FileName);
                }
                else
                {
                    try
                    {
                        TabTitle = ("(*) " + notepadPages[TabbedNotepad.SelectedIndex].FileName).Substring(0, 11).TrimEnd(' ', '.') + "...";
                    }
                    catch
                    {
                        TabTitle = ("(*) " + notepadPages[TabbedNotepad.SelectedIndex].FileName);
                    }
                    TabToolTip = "(Unsaved) " + notepadPages[TabbedNotepad.SelectedIndex].FileName;
                    this.Text = String.Format("(*) {0} - XMLEdit", notepadPages[TabbedNotepad.SelectedIndex].FileName);
                }

                if (TabTitle != null && TabToolTip != null && notepadPages[TabbedNotepad.SelectedIndex].TabTitle != TabTitle || notepadPages[TabbedNotepad.SelectedIndex].TabToolTip != TabToolTip)
                {
                    notepadPages[TabbedNotepad.SelectedIndex].TabTitle = TabTitle;
                    notepadPages[TabbedNotepad.SelectedIndex].TabToolTip = TabToolTip;

                    TabbedNotepad.Refresh();
                    notepadPages[TabbedNotepad.SelectedIndex].Focus();
                }

                EncodingLabel.Text = notepadPages[TabbedNotepad.SelectedIndex].EncodingString.ToUpper();

                if (notepadPages[TabbedNotepad.SelectedIndex].Encoding == Encoding.Default && !ANSIToolStripMenuItem.Checked)
                    SetEncodingToolStripMenuItem_Click(ANSIToolStripMenuItem, e);
                else if (notepadPages[TabbedNotepad.SelectedIndex].Encoding == Encoding.ASCII && !ASCIIToolStripMenuItem.Checked)
                    SetEncodingToolStripMenuItem_Click(ASCIIToolStripMenuItem, e);
                else if (notepadPages[TabbedNotepad.SelectedIndex].Encoding == Encoding.UTF8 && !UTF8ToolStripMenuItem.Checked)
                    SetEncodingToolStripMenuItem_Click(UTF8ToolStripMenuItem, e);
            }
            catch { }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (e.CloseReason == CloseReason.WindowsShutDown) return;

            if (!CloseAllTabs())
                e.Cancel = true;
            else
            {
                _aboutForm.Dispose();
                this.Dispose();
            }
        }

        #endregion
    }
}
