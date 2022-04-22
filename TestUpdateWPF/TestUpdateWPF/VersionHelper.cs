using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows;
using System.Xml;
using System.Xml.Linq;

namespace TestUpdateWPF
{
    public class VersionHelper
    {
        string LocalVersionFilePath = Environment.CurrentDirectory + @"\Version.txt";
        string DownloadPath = Environment.GetEnvironmentVariable("USERPROFILE") + @"\Downloads\Setup.zip";
        string CurrentPath = Path.Combine(Environment.CurrentDirectory);//Environment.GetEnvironmentVariable("USERPROFILE") + @"\Downloads\Setup\";
        string EctractorExePath = Environment.CurrentDirectory + @"\AutoUpdater.exe";
        private string SetupUrl = "https://raw.githubusercontent.com/HamzaRahman/UpdateChannel/main/publish/Setup.zip";
        private string UpdateVersionUrl = "https://raw.githubusercontent.com/HamzaRahman/UpdateChannel/main/publish/Version.txt";
        Version currentversion = System.Reflection.Assembly.GetEntryAssembly().GetName().Version;
        


        public void DownloadNewVersion()
        {
            DownloadNewVersion(SetupUrl);
            UnpackZip();
            ExitApplication();
        }
        public void CreateVersionFile()
        {
            using (FileStream fs = File.Create(LocalVersionFilePath))
            {
                byte[] info = new UTF8Encoding(true).GetBytes(currentversion.ToString());
                fs.Write(info, 0, info.Length);
            }
        }
        public bool NewVersionAvailable()
        {
            WebClient wc = new WebClient();
            Version UpdateVersion = new Version(wc.DownloadString(UpdateVersionUrl));
            if (currentversion != UpdateVersion)
            {
                return true;
            }
            MessageBox.Show(currentversion.ToString() + " = " + UpdateVersion.ToString());
            return false;
        }

        private void DownloadNewVersion(string url)
        {
            //delete existing zip.
            while (File.Exists(DownloadPath))
            {
                File.Delete(DownloadPath);
            }
            //download new zip.
            using (var client = new WebClient())
            {
                client.DownloadFile(url, DownloadPath);
            }
        }
        private void UnpackZip()
        {
            Process.Start(EctractorExePath,CurrentPath+" "+DownloadPath);
        }
        private void ExitApplication()
        {//exit the app.
            Application.Current.Shutdown();
        }
    }
}
