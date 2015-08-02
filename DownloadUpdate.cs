using System;
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

        public DownloadUpdate()
        {
            InitializeComponent();
        }

        // Clicking update, initiate file updates
        public void testUpdate_Click(object sender, EventArgs e)
        {
            foreach (UrlList.UrlUpdateList url in UrlList.urls)
            {
                // If a file already exists by this name
                if (File.Exists(url.fileName))
                {
                    MessageBox.Show("A file already exists" + url.fileName);
                    // Check the checksum for SHA1 of file to match download SHA1
                    if (CheckForUpdate.CheckSum(url.fileName) != url.sha1)
                    {
                        try
                        {
                            // If it doesn't match up, download the file
                            DownloadFile(url.uri, url.fileName, url.newName, url.sha1);
                            MessageBox.Show(CheckForUpdate.CheckSum(url.fileName) + "AND" + url.sha1);
                        }
                        catch(Exception err)
                        {
                            MessageBox.Show("Somethings wrong : " + err);
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
                        if (CheckForUpdate.CheckSum(url.newName) != url.sha1)
                        {
                            // If theres a new filename present but old, Delete it and replace with new one
                            DownloadFile(url.uri, url.fileName, url.newName, url.sha1);
                        }
                    }
                    else if (!File.Exists(url.newName)) {
                        // An old file, or new file does not exist, so download the update
                        DownloadFile(url.uri, url.fileName, url.newName, url.sha1);
                    }
                }
            }
        }

        public void DownloadFile(string url, string fileName, string changeName, string checkSHA1)
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

        // Update UI with the progress of downloads
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

        // The event that will trigger when the WebClient is completed
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
        
        private void button1_Click(object sender, EventArgs e)
        {
            Process.Start("CnCNetClientYR.exe", "-STAGING");
        }

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
            catch (Exception err)
            {
                MessageBox.Show("Could not rename file " + oldname + " to " + newname + err);
            }
        }
    }
}