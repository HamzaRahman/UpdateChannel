using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TestUpdateWPF
{
    //public class Update
    //{
    //    string ExeFile; // the program that called the auto update
    //    string RemoteUri; // the web location of the files
    //    string[] Files; // the list of files to be updated
    //    string Key; // the key used by the program when called back 
    //                // to know that the program was launched by the 
    //                // Auto Update program
    //    string CommandLine; // the command line passed to the original 
    //                        // program if is the case
    //    WebClient myWebClient = new WebClient(); // the web client
    //    public void Check()
    //    {
    //        try
    //        {
    //            // Get the parameters sent by the application
    //            string[] param ;
    //            ExeFile = param[0];
    //            RemoteUri = param[1];
    //            // the files to be updated should be separeted by "?"
    //            Files = Strings.Split(param[2], "?");
    //            Key = param[3];
    //            CommandLine = param[4];
    //        }
    //        catch (Exception ex)
    //        {
    //            // if the parameters wasn't right just terminate the program
    //            // this will happen if the program wasn't called by the system 
    //            // to be updated
    //            return;
    //        }
    //        try
    //        {
    //            // Process each file 
    //            for (int i = 0; i <= Files.Length - 1; i++)
    //            {
    //                try
    //                {
    //                    // try to rename the current file before download the new one
    //                    // this is a good procedure since the file can be in use
    //                    File.Move(Application.StartupPath + @"\" + Files[i], Application.StartupPath + @"\" + DateTime.Now.TimeOfDay.TotalMilliseconds + ".old");
    //                }
    //                catch (Exception ex)
    //                {
    //                }
    //                // download the new version
    //                myWebClient.DownloadFile(RemoteUri + Files[i], Application.StartupPath + @"\" + Files[i]);
    //            }
    //            // Call back the system with the original command line 
    //            // with the key at the end
    //            System.Diagnostics.Process.Start(ExeFile, CommandLine + Key);
    //            // do some clean up -  delete all .old files (if possible) 
    //            // in the current directory
    //            // if some file stays it will be cleaned next time
    //            string S = Directory.GetFiles(Application..Current.StartupUri + @"\*.old");
    //            while (S != "")
    //            {
    //                try
    //                {
    //                    File.Delete(Application.Current.StartupUri + @"\" + S);
    //                }
    //                catch (Exception ex)
    //                {
    //                }
    //                S = FileSystem.Dir();
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            // something went wrong... 
    //            MessageBox.Show("There was a problem runing the Auto Update." + "Please Contact [contact info]" );
    //        }
    //    }
    //}
}
