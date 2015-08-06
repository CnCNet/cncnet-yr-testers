using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace CnCNetTesters
{
    public partial class DownloadUpdate : Form
    {
        Stopwatch sw = new Stopwatch();
        string[] OnlineVersionTxt = null;

        struct VersionTxtStruct
        {
            public VersionTxtStruct(string[] cSV)
            {
                this.uri = cSV[0];
                this.fileName = cSV[1];
                this.newName = cSV[2];
                this.sha1 = cSV[3];
            }
            public string uri;
            public string fileName;
            public string newName;
            public string sha1;
        }

        public DownloadUpdate()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Fetch remote URL and trigger downloads if checksum is newer
        /// </summary>
        private void CheckForUpdates()
        {
            OnlineVersionTxt = Utils.ReadWebsite("http://grant.cncnet.org/updates/yr/yr-update.txt", Environment.NewLine.ToCharArray());
            var remoteUpdate = new List<VersionTxtStruct>();

            foreach (string line in OnlineVersionTxt)
            {
                if ((line.Length > 0) && (!line.StartsWith(";")))
                {
                    string[] cSV = Utils.ReadCSV(line);
                    if (cSV.Length >= 1) remoteUpdate.Add(new VersionTxtStruct(cSV));
                }
            }

            foreach (var url in remoteUpdate)
            {
                string localFilePath = Program.path += url.fileName;

                if(Utils.GetSHA1Checksum(localFilePath) != url.sha1)
                {
                    Console.WriteLine(Utils.GetSHA1Checksum(localFilePath) + " vs " + url.sha1);

                    // If a file already exists by this name
                    if (File.Exists(url.fileName))
                    {
                        MessageBox.Show("A file already exists" + url.fileName);
                        // Check the checksum for SHA1 of file to match download SHA1
                        if (Utils.GetSHA1Checksum(localFilePath) != url.sha1)
                        {
                            try
                            {
                                // If it doesn't match up, download the file
                                DownloadFile(url.uri, url.fileName, url.newName, url.sha1);
                                MessageBox.Show(Utils.GetSHA1Checksum(url.fileName) + "AND" + url.sha1);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Somethings wrong : " + ex);
                            }
                        }
                    }
                    // If the old file does not exist ...
                    else if (!File.Exists(url.fileName))
                    {
                        // Download the new file, but check new File doesn't exist first
                        if (File.Exists(url.newName))
                        {
                            // Theres a new filename present, checksum this
                            if (Utils.GetSHA1Checksum(url.newName) != url.sha1)
                            {
                                // If theres a new filename present but old, Delete it and replace with new one
                                DownloadFile(url.uri, url.fileName, url.newName, url.sha1);
                            }
                        }
                        else if (!File.Exists(url.newName))
                        {
                            // An old file, or new file does not exist, so download the update
                            DownloadFile(url.uri, url.fileName, url.newName, url.sha1);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Download files based on number of params
        /// </summary>
        /// <param name="url"></param>
        /// <param name="fileName"></param>
        /// <param name="changeName"></param>
        /// <param name="checkSHA1"></param>
        private void DownloadFile(string url, string fileName, string changeName, string checkSHA1)
        {
            // Object for e.Userstate
            var response = new { oldname = fileName, newname = changeName, sha1 = checkSHA1 };
            string remoteUri = url, myStringWebResource = null;

            // Create a new WebClient instance.
            WebClient webClient = new WebClient();

            using (webClient)
            {
                // Concatenate the domain with the Web resource filename.
                myStringWebResource = remoteUri + fileName;
                Uri URL = myStringWebResource.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ? new Uri(remoteUri) : new Uri("http://" + remoteUri);

                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);

                // Start the stopwatch which we will be using to calculate the download speed
                sw.Start();

                // Download list of updates
                try
                {
                    webClient.DownloadFileAsync(URL, fileName, response);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        /// <summary>
        /// Update UI with progress of download %
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            // Calculate download speed and output it to labelSpeed.
            downloadSpeed.Text = string.Format("{0} kb/s", (e.BytesReceived / 1024d / sw.Elapsed.TotalSeconds).ToString("0.00"));

            // Update the progressbar percentage only when the value is not the same.
            progressBar.Value = e.ProgressPercentage;

            // Show the percentage on our label.
            percent.Text = e.ProgressPercentage.ToString() + "%";

            // Update the label with how much data have been downloaded so far and the total size of the file we are currently downloading
            downloadMb.Text = string.Format("{0} MB's / {1} MB's",
                (e.BytesReceived / 1024d / 1024d).ToString("0.00"),
                (e.TotalBytesToReceive / 1024d / 1024d).ToString("0.00"));
        }

        /// <summary>
        /// Event that will trigger when the WebClient is completed downloading
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            object response = e.UserState;

            System.Reflection.PropertyInfo a = response.GetType().GetProperty("oldname");
            String oldname = (String)(a.GetValue(response, null));

            System.Reflection.PropertyInfo b = response.GetType().GetProperty("newname");
            String newname = (String)(b.GetValue(response, null));

            System.Reflection.PropertyInfo c = response.GetType().GetProperty("sha1");
            String sha1 = (String)(c.GetValue(response, null));

            RenameFile(oldname, newname);

            // Reset the stopwatch.
            sw.Reset();
        }

        /// <summary>
        /// Rename, and/or delete old files if determined outofdate
        /// </summary>
        /// <param name="oldname"></param>
        /// <param name="newname"></param>
        private void RenameFile(string oldname, string newname)
        {
            try
            {
                if (File.Exists(newname))
                {
                    File.Delete(newname);
                }
                File.Move(oldname, newname);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not rename file " + oldname + " to " + newname + ex);
            }
        }

        /// <summary>
        /// Launch client with staging parameters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("CnCNetClientYR.exe", "-STAGING");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error launching: " + ex);
            }
        }

        /// <summary>
        /// Get latest update
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void getUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                CheckForUpdates();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating: " + ex);
            }
        }
    }
}