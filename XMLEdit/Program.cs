using System;
using System.Reflection;
using System.Windows.Forms;

namespace XMLEdit
{
    static class Program
    {
        private const bool IsPreReleaseBuild = true;
        private const string PreReleaseTag = "DEV_200108";
               
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new XMLEdit());
        }

        /// <summary>
        /// Used to get the current version of SSM
        /// </summary>
        /// <returns>The version number of SSM</returns>
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
