﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace XMLEdit
{
    static class Program
    {
        private const bool IsPreReleaseBuild = false;
        private const string PreReleaseTag = "R1";
               
        [STAThread]
        static void Main(string[] args)
        {
            List<string> filePaths = new List<string>();

            foreach (string arg in args)
            {
                if (File.Exists(arg) && (Path.GetExtension(arg).ToLower() == ".xml" || Path.GetExtension(arg).ToLower() == ".json"))
                {
                    filePaths.Add(arg);
                }
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (filePaths.Count == 0) Application.Run(new XMLEdit());
            else Application.Run(new XMLEdit(filePaths.ToArray()));
        }

        /// <summary>
        /// Used to get the current version of XMLEdit
        /// </summary>
        /// <returns>The version number of XMLEdit</returns>
        public static string GetVersion()
        {
            #pragma warning disable CS0162 //Unreachable code detected
            string[] ver = (typeof(XMLEdit).Assembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version).Split('.');
            if (!IsPreReleaseBuild)
                return "v" + ver[0] + "." + ver[1] + "." + ver[2];
            else
                return "v" + ver[0] + "." + ver[1] + "." + ver[2] + "-" + PreReleaseTag;
            #pragma warning restore CS0162 //Unreachable code detected
        }
    }
}
