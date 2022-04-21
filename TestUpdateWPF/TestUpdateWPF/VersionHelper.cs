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
        string ExePath = Environment.GetEnvironmentVariable("USERPROFILE") + @"\Downloads\AutoUpdater\AutoUpdater.exe";
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
            string appVersion = $"{currentversion.Major}.{currentversion.Minor}.{currentversion.Build}.{currentversion.Revision}";
            if (!File.Exists(LocalVersionFilePath))
            {
                File.Create(LocalVersionFilePath);
            }
            Thread.Sleep(1000);
            File.WriteAllText(LocalVersionFilePath, appVersion);
        }
        public bool NewVersionAvailable()
        {
            
            WebClient wc = new WebClient();
            Version newv = new Version(wc.DownloadString(UpdateVersionUrl));
            string appVersion = $"{currentversion.Major}.{currentversion.Minor}.{currentversion.Build}.{currentversion.Revision}";
            if (currentversion.CompareTo(newv)>0)
            {
                return true;
            }

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
            Process.Start(ExePath,CurrentPath+" "+DownloadPath);
        }
        private void ExitApplication()
        {//exit the app.
            Application.Current.Shutdown();
        }
    }
}
