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
        public string MSIFilePath = Path.Combine(Environment.CurrentDirectory);
        string DownloadPath = Environment.GetEnvironmentVariable("USERPROFILE") + @"\Downloads\Setup.zip";
        string FolderPath = Environment.GetEnvironmentVariable("USERPROFILE") + @"\Downloads\Setup\";
        string ExePath = Environment.GetEnvironmentVariable("USERPROFILE") + @"\Downloads\AutoUpdater\AutoUpdater.exe";
        private string CmdFilePath = Path.Combine(Environment.CurrentDirectory, "Install.cmd");
        private string MsiUrl = "https://raw.githubusercontent.com/HamzaRahman/UpdateChannel/main/publish/Setup.zip";

        public bool CheckForNewVersion()
        {
            MsiUrl = GetNewVersionUrl();
            return MsiUrl.Length > 0;
        }

        public void DownloadNewVersion()
        {
            DownloadNewVersion(MsiUrl);
            UnpackZip();
            //CreateCmdFile();
            //RunCmdFile();
            //ExitApplication();
        }

        private string GetNewVersionUrl()
        {
            var versio = System.Reflection.Assembly.GetEntryAssembly().GetName().Version;
            string appVersion = $"{versio.Major}.{versio.Minor}";
            MessageBox.Show(appVersion);
            var url = "https://raw.githubusercontent.com/HamzaRahman/UpdateChannel/main/publish/Version.txt";
            //var data = "";
            //var wc = new WebClient();
            //data = wc.DownloadString(url);
            var currentVersion = 1;//Convert.ToInt32(ConfigurationManager.AppSettings["Version"]);
                                   //get xml from url.
            /*var url = "https://raw.githubusercontent.com/HamzaRahman/UpdateChannel/main/publish/";*///ConfigurationManager.AppSettings["VersionUrl"].ToString();
            var builder = new StringBuilder();
            using (var stringWriter = new StringWriter(builder))
            {
                using (var xmlReader = new XmlTextReader(url))
                {
                    var doc = XDocument.Load(xmlReader);
                    //get versions.
                    var versions = from v in doc.Descendants("version")
                                   select new
                                   {
                                       Name = v.Element("name").Value,
                                       Number = Convert.ToInt32(v.Element("number").Value),
                                       URL = v.Element("url").Value,
                                       Date = Convert.ToDateTime(v.Element("date").Value)
                                   };
                    var version = versions.ToList()[0];
                    //check if latest version newer than current version.
                    if (version.Number > currentVersion)
                    {
                        return version.URL;
                    }
                }
            }
            return String.Empty;
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
            if (Directory.Exists(FolderPath))
            {
                string[] files = Directory.GetFiles(FolderPath);
                foreach (string file in files)
                {
                    File.Delete(file);
                    Console.WriteLine($"{file} is deleted.");
                }
                Directory.Delete(FolderPath, true);
            }
            ZipFile.ExtractToDirectory(DownloadPath, FolderPath);
            Process.Start(ExePath);
            Application.Current.Shutdown();
        }
        private void CreateCmdFile()
        {
            //check if file exists.
            if (File.Exists(CmdFilePath))
                File.Delete(CmdFilePath);
            //create new file.
            var fi = new FileInfo(CmdFilePath);
            var fileStream = fi.Create();
            fileStream.Close();
            //write commands to file.
            using (TextWriter writer = new StreamWriter(CmdFilePath))
            {
                writer.WriteLine(@"msiexec /i HoustersCrawler.msi /quiet");
            }
        }

        private void RunCmdFile()
        {//run command file to reinstall app.
            var p = new Process();
            p.StartInfo = new ProcessStartInfo("cmd.exe", "/c Install.cmd");
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            //p.WaitForExit();
        }

        private void ExitApplication()
        {//exit the app.
            Application.Current.Shutdown();
        }
    }
}
