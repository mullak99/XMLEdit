using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace XMLEdit
{
    class UserThemes
    {
        ThemeLoader tLoader = new ThemeLoader();

        List<Theme> themes = new List<Theme>();

        public UserThemes()
        {
            UnloadAllThemes();
            LoadAllThemes();
        }

        public List<Theme> GetAllThemes()
        {
            return themes;
        }
        
        public Theme SelectThemeWithID(string themeID)
        {
            try
            {
                return themes.Single(t => t.ThemeID == themeID);
            }
            catch
            {
                return new Theme("default", "Default");
            }
        }

        public bool DoesThemeExist(string themeID)
        {
            return themes.Any(t => t.ThemeID == themeID);
        }

        public void LoadAllThemes()
        {
            try
            {
                themes.AddRange(tLoader.LoadThemeFolder());
            }
            catch { }
        }

        public void UnloadAllThemes()
        {
            themes.Clear();
            LoadDefaultTheme();
        }

        public void AddTheme(string path)
        {

        }

        public void LoadTheme(string themeName)
        {

        }

        private void LoadDefaultTheme()
        {
            Theme defaultTheme = new Theme("default", "Default");
            defaultTheme.ThemeAuthor = "mullak99";

            themes.Add(defaultTheme);
        }
    }

    class ThemeLoader
    {
        public ThemeLoader()
        { }

        public Theme LoadTheme(string themeName)
        {
            return null;
        }

        public List<Theme> LoadThemeFolder(string themeFolder = "themes")
        {
            try
            {
                string[] themeFiles = System.IO.Directory.GetFiles(themeFolder, "*.xetheme");
                List<Theme> themes = new List<Theme>();

                foreach (string theme in themeFiles)
                {
                    themes.Add(ProcessTheme(theme));
                }

                return themes;

            }
            catch
            {
                return null;
            }
        }

        private Color GetColorFromHex(string hexColor)
        {
            return ColorTranslator.FromHtml(hexColor);
        }

        private Theme ProcessTheme(string themePath)
        {
            string[] tLines = File.ReadAllLines(themePath);

            Theme theme = new Theme(Path.GetFileNameWithoutExtension(themePath), "");

            foreach (string tElement in tLines)
            {
                string[] themeElement = tElement.Split('=');

                if (themeElement[0] == "ThemeName") theme.ThemeName = themeElement[1];
                else if (themeElement[0] == "ThemeAuthor") theme.ThemeAuthor = themeElement[1];
                else if (themeElement[0] == "DefaultTextColour") theme.StandardTextColor = GetColorFromHex(themeElement[1]);
                else if (themeElement[0] == "DefaultBackColour") theme.StandardBackgroundColor = GetColorFromHex(themeElement[1]);
                else if (themeElement[0] == "LineNumTextColour") theme.LineNumberTextColor = GetColorFromHex(themeElement[1]);
                else if (themeElement[0] == "LineNumBackColour") theme.LineNumberBackgroundColor = GetColorFromHex(themeElement[1]);
                else if (themeElement[0] == "FoldMarkForeColour") theme.FolderMarkerTextColor = GetColorFromHex(themeElement[1]);
                else if (themeElement[0] == "FoldMarkBackColour") theme.FolderMarkerBackgroundColor = GetColorFromHex(themeElement[1]);
                else if (themeElement[0] == "FoldMarkHighColour") theme.FolderMarkerHighlightColor = GetColorFromHex(themeElement[1]);
                else if (themeElement[0] == "XmlAttrTextColour") theme.XmlAttributeTextColor = GetColorFromHex(themeElement[1]);
                else if (themeElement[0] == "XmlAttrBackColour") theme.XmlAttributeBackgroundColor = GetColorFromHex(themeElement[1]);
                else if (themeElement[0] == "XmlEntityTextColour") theme.XmlEntityTextColor = GetColorFromHex(themeElement[1]);
                else if (themeElement[0] == "XmlEntityBackColour") theme.XmlEntityBackgroundColor = GetColorFromHex(themeElement[1]);
                else if (themeElement[0] == "XmlTagTextColour") theme.XmlTagTextColor = GetColorFromHex(themeElement[1]);
                else if (themeElement[0] == "XmlTagBackColour") theme.XmlTagBackgroundColor = GetColorFromHex(themeElement[1]);
                else if (themeElement[0] == "XmlTagEndTextColour") theme.XmlTagEndTextColor = GetColorFromHex(themeElement[1]);
                else if (themeElement[0] == "XmlTagEndBackColour") theme.XmlTagEndBackgroundColor = GetColorFromHex(themeElement[1]);
                else if (themeElement[0] == "XmlDsTextColour") theme.XmlDoubleStringTextColor = GetColorFromHex(themeElement[1]);
                else if (themeElement[0] == "XmlDsBackColour") theme.XmlDoubleStringBackgroundColor = GetColorFromHex(themeElement[1]);
                else if (themeElement[0] == "XmlSsTextColour") theme.XmlSingleStringTextColor = GetColorFromHex(themeElement[1]);
                else if (themeElement[0] == "XmlSsBackColour") theme.XmlSingleStringBackgroundColor = GetColorFromHex(themeElement[1]);
                else if (themeElement[0] == "BraceGoodColour") theme.BraceGoodColor = GetColorFromHex(themeElement[1]);
                else if (themeElement[0] == "BraceBadColour") theme.BraceBadColor = GetColorFromHex(themeElement[1]);
            }
            return theme;
        }
    }
}
