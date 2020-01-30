using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace XMLEdit
{
    public partial class XMLEdit : Form
    {
        List<string> tabFileNames = new List<string>();

        AboutForm _aboutForm = new AboutForm();

        Font _defaultFont = new Font("Consolas", 10);
        UserThemes _uThemes = new UserThemes();

        Font _globalFont;
        Theme _globalTheme;

        public XMLEdit()
        {
            InitializeComponent();
            _aboutForm.Owner = this;

            Init();
        }

        public XMLEdit(string[] paths) : this()
        {
            foreach (string path in paths)
                OpenTab(path);
        }

        #region Methods

        private void Init()
        {
            Properties.Settings.Default.Upgrade();

            this.Size = Properties.Settings.Default.appSize;
            _globalFont = _defaultFont;

            PopulateThemes();
            SelectTheme(Properties.Settings.Default.selectedTheme);
            OpenCachedTabs();
        }

        private void PopulateThemes()
        {
            themeToolStripMenuItem.DropDownItems.Clear();

            foreach (Theme theme in _uThemes.GetAllThemes())
            {
                ToolStripMenuItem item = new ToolStripMenuItem();
                item.Name = theme.ThemeID + "_toolStripMenuItem";
                item.Text = theme.ThemeName;
                item.ToolTipText = theme.ThemeName + " by " + theme.ThemeAuthor;
                item.Click += delegate { SelectTheme(theme.ThemeID); };

                themeToolStripMenuItem.DropDownItems.Add(item);
            }
            _globalTheme = _uThemes.SelectThemeWithID("default");
        }

        private void OpenCachedTabs()
        {
            try
            {
                StringCollection paths = Properties.Settings.Default.openFiles;

                string[] cachedTabs = paths.Cast<string>().ToArray();

                foreach (string tab in cachedTabs)
                    OpenTab(tab, true);
            }
            catch { }
        }

        private void SelectTheme(string themeID)
        {
            if (_uThemes.DoesThemeExist(themeID))
            {
                foreach (ToolStripMenuItem item in themeToolStripMenuItem.DropDownItems)
                {
                    if (item.Name == (themeID + "_toolStripMenuItem")) item.Checked = true;
                    else item.Checked = false;
                }

                _globalTheme = _uThemes.SelectThemeWithID(themeID);

                Properties.Settings.Default.selectedTheme = themeID;

                Properties.Settings.Default.Save();
                Properties.Settings.Default.Reload();
            }
            else
            {
                _globalTheme = _uThemes.SelectThemeWithID("default");
                Properties.Settings.Default.selectedTheme = "default";
            }

            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload();

            ApplyTheme(_globalTheme);
        }

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
            foreach (NotepadPage npPage in TabbedNotepad.Controls)
            {
                npPage.Theme = theme;
            }
        }

        private void CacheOpenFiles()
        {
            NotepadPage[] notepadPages = TabbedNotepad.Controls.OfType<NotepadPage>().ToArray();

            StringCollection paths = new StringCollection();
            paths.AddRange(notepadPages.Select(i => i.FilePath).ToArray());

            Properties.Settings.Default.openFiles = paths;
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload();
        }

        #endregion
        #region Tabs

        private void ForceRemoveTabAt(int index)
        {
            NotepadPage notepadPage = TabbedNotepad.Controls[index] as NotepadPage;
            string fileName = notepadPage.FileName;

            TabbedNotepad.Controls.RemoveAt(index);
            TabbedNotepad.TabPages.RemoveAt(index);
            tabFileNames.Remove(fileName);
            CacheOpenFiles();

            if (TabbedNotepad.TabCount > 0) TabbedNotepad.SelectedIndex = (TabbedNotepad.TabCount - 1);
        }

        private void AddTab(NotepadPage notepadPage, bool skipAdd = false)
        {
            if (!skipAdd) TabbedNotepad.Controls.Add(notepadPage);
            notepadPage.Focus();

            CacheOpenFiles();
        }

        private void OpenTab(string path = null, bool requirePath = false)
        {
            if ((!String.IsNullOrWhiteSpace(path) && File.Exists(path)) || String.IsNullOrWhiteSpace(path) && !requirePath)
            {
                NotepadPage notepadPage;
                string oldTabFileName = "";
                bool skipAdd = false;

                if ((TabbedNotepad.Controls.Count > 0 && !String.IsNullOrWhiteSpace(TabbedNotepad.Controls[TabbedNotepad.SelectedIndex].Text)) || TabbedNotepad.Controls.Count == 0)
                    notepadPage = new NotepadPage("", _globalFont, _globalTheme);
                else
                {
                    NotepadPage lastNotepadPage = TabbedNotepad.Controls[TabbedNotepad.Controls.Count - 1] as NotepadPage;
                    oldTabFileName = lastNotepadPage.FileName;
                    notepadPage = lastNotepadPage;
                    skipAdd = true;
                }

                bool opened;
                if (String.IsNullOrWhiteSpace(path)) opened = notepadPage.Open();
                else opened = notepadPage.Open(path);

                if (opened)
                {
                    if (!String.IsNullOrWhiteSpace(oldTabFileName)) tabFileNames.Remove(oldTabFileName);
                    AddTab(notepadPage, skipAdd);
                }
                else CloseTabAt(TabbedNotepad.SelectedIndex);
            }
        }

        private bool CloseTabAt(int index)
        {
            bool ret = false;
            try
            {
                NotepadPage[] notepadPages = TabbedNotepad.Controls.OfType<NotepadPage>().ToArray();

                if (notepadPages[index].Saved)
                {
                    if (TabbedNotepad.TabCount > 1 || !String.IsNullOrEmpty(notepadPages[index].TextboxText))
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
            NotepadPage[] notepadPages = TabbedNotepad.Controls.OfType<NotepadPage>().ToArray();
            NotepadPage[] tempPages = notepadPages;

            foreach (NotepadPage npPage in tempPages)
            {
                if (!CloseTabAt(Array.IndexOf(notepadPages, npPage)) && !npPage.Saved)
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
            foreach (NotepadPage npPage in TabbedNotepad.Controls)
            {
                npPage.Focus();
            }
        }

        #endregion
        #region MenuToolStrip

        private void NewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NotepadPage notepadPage = new NotepadPage(GetNewFileName(), _globalFont, _globalTheme);
            AddTab(notepadPage);
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenTab();
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NotepadPage npPage = TabbedNotepad.Controls[TabbedNotepad.SelectedIndex] as NotepadPage;
            npPage.Save();
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NotepadPage npPage = TabbedNotepad.Controls[TabbedNotepad.SelectedIndex] as NotepadPage;
            npPage.SaveAs();
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

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NotepadPage npPage = TabbedNotepad.Controls[TabbedNotepad.SelectedIndex] as NotepadPage;
            npPage.ShowFinder();
        }

        private void findNextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NotepadPage npPage = TabbedNotepad.Controls[TabbedNotepad.SelectedIndex] as NotepadPage;
            npPage.FindNext();
        }

        private void findPreviousToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NotepadPage npPage = TabbedNotepad.Controls[TabbedNotepad.SelectedIndex] as NotepadPage;
            npPage.FindPrev();
        }

        private void replaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NotepadPage npPage = TabbedNotepad.Controls[TabbedNotepad.SelectedIndex] as NotepadPage;
            npPage.ShowReplace();
        }

        private void incrementalSearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NotepadPage npPage = TabbedNotepad.Controls[TabbedNotepad.SelectedIndex] as NotepadPage;
            npPage.ShowIncSearch();
        }

        private void goToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NotepadPage npPage = TabbedNotepad.Controls[TabbedNotepad.SelectedIndex] as NotepadPage;
            npPage.ShowGoTo();
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NotepadPage npPage = TabbedNotepad.Controls[TabbedNotepad.SelectedIndex] as NotepadPage;
            npPage.Cut();
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NotepadPage npPage = TabbedNotepad.Controls[TabbedNotepad.SelectedIndex] as NotepadPage;
            npPage.Copy();
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NotepadPage npPage = TabbedNotepad.Controls[TabbedNotepad.SelectedIndex] as NotepadPage;
            npPage.Paste();
        }

        private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NotepadPage npPage = TabbedNotepad.Controls[TabbedNotepad.SelectedIndex] as NotepadPage;
            npPage.Delete();
        }

        private void SelectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NotepadPage npPage = TabbedNotepad.Controls[TabbedNotepad.SelectedIndex] as NotepadPage;
            npPage.SelectAll();
        }

        private void UndoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NotepadPage npPage = TabbedNotepad.Controls[TabbedNotepad.SelectedIndex] as NotepadPage;
            npPage.Undo();
        }

        private void RedoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NotepadPage npPage = TabbedNotepad.Controls[TabbedNotepad.SelectedIndex] as NotepadPage;
            npPage.Redo();
        }

        private void FontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontDialog fontDiag = new FontDialog();
            fontDiag.ShowColor = false;
            fontDiag.ShowEffects = false;
            fontDiag.AllowScriptChange = false;
            fontDiag.Font = _globalFont;
            
            DialogResult result = fontDiag.ShowDialog();

            if (result != DialogResult.Cancel)
            {
                _globalFont = fontDiag.Font;

                foreach (NotepadPage npPage in TabbedNotepad.Controls)
                {
                    npPage.Font = _globalFont;
                }
            }
        }

        private void ZoomLevel_Click(object sender, EventArgs e)
        {
            NotepadPage npPage = TabbedNotepad.Controls[TabbedNotepad.SelectedIndex] as NotepadPage;
            npPage.Zoom = 0;
        }

        private void SetEncodingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NotepadPage npPage = TabbedNotepad.Controls[TabbedNotepad.SelectedIndex] as NotepadPage;
            if (sender == ANSIToolStripMenuItem)
            {
                npPage.Encoding = Encoding.Default;
                ANSIToolStripMenuItem.Checked = true;
                ASCIIToolStripMenuItem.Checked = false;
                UTF8ToolStripMenuItem.Checked = false;
            }
            else if (sender == ASCIIToolStripMenuItem)
            {
                npPage.Encoding = Encoding.ASCII;
                ANSIToolStripMenuItem.Checked = false;
                ASCIIToolStripMenuItem.Checked = true;
                UTF8ToolStripMenuItem.Checked = false;
            }
            else if (sender == UTF8ToolStripMenuItem)
            {
                npPage.Encoding = Encoding.UTF8;
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

        private void XMLEdit_SizeChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.appSize = this.Size;

            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload();
        }

        private void XMLEdit_Load(object sender, EventArgs e)
        {
            if (TabbedNotepad.Controls.Count == 0) NewToolStripMenuItem_Click(sender, e);
        }

        private void Runtime_Tick(object sender, EventArgs e)
        {
            try
            {
                if (TabbedNotepad.TabCount == 0) NewToolStripMenuItem_Click(sender, e);

                NotepadPage npPage = TabbedNotepad.Controls[TabbedNotepad.SelectedIndex] as NotepadPage;

                char[] delimiters = new char[] { ' ', '\r', '\n' };

                int textLen = npPage.TextboxText.Length;
                int lineLen = npPage.Lines.Count;
                int wordCount = npPage.TextboxText.Split(delimiters, StringSplitOptions.RemoveEmptyEntries).Length;
                int zoom = npPage.Zoom;

                TextLengthLabel.Text = "Length: " + textLen;

                if (lineLen > 0) LineTotalLabel.Text = "Lines: " + lineLen;
                else LineTotalLabel.Text = "Lines: 1";

                WordTotalLabel.Text = "Words: " + wordCount;

                TextLengthLabel.ToolTipText = String.Format("Total characters in {0}: {1}", npPage.FileName, textLen);
                LineTotalLabel.ToolTipText = String.Format("Total lines in {0}: {1}", npPage.FileName, lineLen);
                WordTotalLabel.ToolTipText = String.Format("Total words in {0}: {1}", npPage.FileName, wordCount);

                if (zoom > -10)
                    ZoomLevel.Text = String.Format("Zoom: {0}0%", (10 + zoom));
                else
                    ZoomLevel.Text = "Zoom: 0%";

                string TabTitle, TabToolTip;
                if (npPage.Saved)
                {
                    try
                    {
                        TabTitle = npPage.FileName.Substring(0, 11).TrimEnd(' ', '.') + "...";
                    }
                    catch
                    {
                        TabTitle = npPage.FileName;
                    }

                    TabToolTip = npPage.FileName;
                    this.Text = String.Format("{0} - XMLEdit", npPage.FileName);
                }
                else
                {
                    try
                    {
                        TabTitle = ("(*) " + npPage.FileName).Substring(0, 11).TrimEnd(' ', '.') + "...";
                    }
                    catch
                    {
                        TabTitle = ("(*) " + npPage.FileName);
                    }
                    TabToolTip = "(Unsaved) " + npPage.FileName;
                    this.Text = String.Format("(*) {0} - XMLEdit", npPage.FileName);
                }

                if (TabTitle != null && TabToolTip != null && npPage.TabTitle != TabTitle || npPage.TabToolTip != TabToolTip)
                {
                    npPage.TabTitle = TabTitle;
                    npPage.TabToolTip = TabToolTip;

                    TabbedNotepad.Refresh();
                    npPage.Focus();
                }

                EncodingLabel.Text = npPage.EncodingString.ToUpper();

                if (npPage.Encoding == Encoding.Default && !ANSIToolStripMenuItem.Checked)
                    SetEncodingToolStripMenuItem_Click(ANSIToolStripMenuItem, e);
                else if (npPage.Encoding == Encoding.ASCII && !ASCIIToolStripMenuItem.Checked)
                    SetEncodingToolStripMenuItem_Click(ASCIIToolStripMenuItem, e);
                else if (npPage.Encoding == Encoding.UTF8 && !UTF8ToolStripMenuItem.Checked)
                    SetEncodingToolStripMenuItem_Click(UTF8ToolStripMenuItem, e);
            }
            catch { }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (e.CloseReason == CloseReason.WindowsShutDown) return;

            _aboutForm.Dispose();
            this.Dispose();
        }

        #endregion
    }
}
