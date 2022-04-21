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
        string LocalVersionFilePath = Environment.CurrentDirectory + @"\Version.txt";
        string DownloadPath = Environment.GetEnvironmentVariable("USERPROFILE") + @"\Downloads\Setup.zip";
        string CurrentPath = Path.Combine(Environment.CurrentDirectory);//Environment.GetEnvironmentVariable("USERPROFILE") + @"\Downloads\Setup\";
        string ExePath = Environment.GetEnvironmentVariable("USERPROFILE") + @"\Downloads\AutoUpdater\AutoUpdater.exe";
        private string CmdFilePath = Path.Combine(Environment.CurrentDirectory, "Install.cmd");
        private string SetupUrl = "https://raw.githubusercontent.com/HamzaRahman/UpdateChannel/main/publish/Setup.zip";
        private string UpdateVersionUrl = "https://raw.githubusercontent.com/HamzaRahman/UpdateChannel/main/publish/Version.txt";

        public bool CheckForNewVersion()
        {
            SetupUrl = GetNewVersionUrl();
            return SetupUrl.Length > 0;
        }

        public void DownloadNewVersion()
        {
            DownloadNewVersion(SetupUrl);
            UnpackZip();
            //CreateCmdFile();
            //RunCmdFile();
            //ExitApplication();
        }

        private string GetNewVersionUrl()
        {
            var currentversio = System.Reflection.Assembly.GetEntryAssembly().GetName().Version;
            string appVersion = $"{currentversio.Major}.{currentversio.Minor}";
            WebClient wc = new WebClient();
            if (!File.Exists(LocalVersionFilePath))
            {
                File.Create(LocalVersionFilePath);
                MessageBox.Show(LocalVersionFilePath);
                //File.WriteAllText(LocalVersionFilePath, "0.0.0.0");
            }
            if (wc.DownloadString(UpdateVersionUrl) != File.ReadAllText(LocalVersionFilePath))
            {
                MessageBox.Show("Update Available");
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
            //if (Directory.Exists(CurrentPath))
            //{
            //    string[] files = Directory.GetFiles(CurrentPath);
            //    foreach (string file in files)
            //    {
            //        File.Delete(file);
            //        Console.WriteLine($"{file} is deleted.");
            //    }
            //    Directory.Delete(CurrentPath, true);
            //}
            //ZipFile.ExtractToDirectory(DownloadPath, CurrentPath);
            Process.Start(ExePath,CurrentPath+" "+DownloadPath);
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
