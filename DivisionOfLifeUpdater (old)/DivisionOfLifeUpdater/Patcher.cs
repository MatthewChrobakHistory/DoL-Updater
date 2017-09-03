using System;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace DivisionOfLifeUpdater
{
    public class Patcher
    {
        private bool _continue = false;
        private string _currentFile = "";
        private string _currentFilePath = "";
        private string _rawFile = "";
        private int _count = 0;
        private bool _error = false;
        private int _start = 0;
        private WebClient _client = new WebClient();
        private string baseURL = "http://divisionoflife.com/update/";

        public void Begin() {

            // Necessary paths and URLs.
            string startupPath = AppDomain.CurrentDomain.BaseDirectory;
            string revisionFile = startupPath + "Revision.txt";
            string hostFile = startupPath + "Host.txt";
            

            // Check the revision file. If it does not exist,
            // leave the revision at 0 to signify a lack of files.
            int clientRevision = 0;
            if (File.Exists(revisionFile)) {
                clientRevision = Int32.Parse(File.ReadAllText(revisionFile));
            }

            // Download and convert the folder list into a byte stream and create 
            // the necessary folders.
            var folderList = new DataBuffer(DownloadFile(baseURL + "FolderList.dat"));
            int folderCount = folderList.ReadInt();
            for (int i = 0; i < folderCount; i++) {
                Directory.CreateDirectory(startupPath + folderList.ReadString());
                Application.DoEvents();
            }

            // Download and convert the file list into a byte stream.
            var fileList = new DataBuffer(DownloadFile(baseURL + "FileList.dat"));

            // Get the current product revision as determined by the FileIndexer
            // by the number of patch folders, and store it for later use.
            int serverRevision = fileList.ReadInt();

            // Create a webclient for downloading files and instantly saving them
            // onto the disk.
            _client.DownloadFileCompleted += client_DownloadFileCompleted;
            _client.DownloadProgressChanged += client_DownloadProgressChanged;

            // Loop through the file list and retrieve information about the required files.
            int fileCount = fileList.ReadInt();
            for (int i = 0; i < fileCount; i++) {

                decimal value = Decimal.Divide(i + 1, fileCount);
                int prcn = (int)(value * 100);

                Program.Menu.Invoke((MethodInvoker)delegate {
                    Program.Menu.lblTotal.Text = "Files Remaining: " + (fileCount - i).ToString();
                    Program.Menu.prgTotal.Value = prcn;
                });

                string clientpath = fileList.ReadString();
                string serverpath = fileList.ReadString();
                int revision = fileList.ReadInt();

                // If the file already exists and the revision it was introduced in
                // is older than our current version, then continue to the next
                // file in the file list.
                if (File.Exists(startupPath + clientpath)) {
                    Console.WriteLine(revision);
                    if (revision <= clientRevision) {
                        continue;
                    }
                }

                // If the file doesn't exist, or if the file exists but was 
                // introduced in a newer version than our current version, then download the file.
                Console.WriteLine("Downloading " + clientpath);

                try {
                    _continue = false;
                    Uri url = new Uri(baseURL + serverpath);
                    _currentFile = baseURL + serverpath;
                    _rawFile = _currentFile;
                    while (_rawFile.Contains("\\")) {
                        if (_rawFile.StartsWith("\\")) {
                            _rawFile = _rawFile.Remove(0, 1);
                        } else {
                            _rawFile = _rawFile.Remove(0, _rawFile.IndexOf("\\"));
                        }
                    }
                    _currentFilePath = startupPath + clientpath;
                    _start = Environment.TickCount;
                    _client.DownloadFileAsync(url, startupPath + clientpath);
                } catch {
                    Program.Menu.Invoke((MethodInvoker)delegate {
                        Program.Menu.lblCurrent.Text = "Unable to download " + _currentFile + " attempt " + _count + "/5";
                    });
                }

                while (!_continue) {
                    Application.DoEvents();
                }
            }

            // Store the current revision we installed, and terminate the program
            // with a message of success.
            File.WriteAllText(revisionFile, serverRevision.ToString());

            Program.Menu.Invoke((MethodInvoker)delegate {

                if (_error) {
                    Program.Menu.lblRetry.Visible = true;
                } else {
                    Program.Menu.lblPlay.Visible = true;
                }

                Program.Menu.lblCurrent.Visible = false;
                Program.Menu.lblTotal.Visible = false;
                Program.Menu.prgCurrent.Visible = false;
                Program.Menu.prgTotal.Visible = false;
            });
        }

        void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e) {
            int percentage = e.ProgressPercentage;

            decimal Received = Decimal.Parse((e.BytesReceived / 1024d / 1024d).ToString("0.00"));
            decimal Total = Decimal.Parse((e.TotalBytesToReceive / 1024d / 1024d).ToString("0.00"));
            int secondselated = (_start - Environment.TickCount) + 1;
            string time = (e.BytesReceived / -1024d / secondselated).ToString("0.00");
            

            Program.Menu.Invoke((MethodInvoker)delegate {
                Program.Menu.lblCurrent.Text = _rawFile + ": (" + time + "mb/s)" + (Total - Received) + " MB remaining";
                Program.Menu.prgCurrent.Value = percentage;
            });
        }

        void client_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e) {
            if (e.Error != null) {
                _count++;
                if (_count >= 5) {
                    if (File.Exists(_currentFilePath)) {
                        File.Delete(_currentFilePath);
                    }
                    _continue = true;
                    _count = 0;
                    _error = true;
                    return;
                } else {
                    Program.Menu.Invoke((MethodInvoker)delegate {
                        Program.Menu.lblCurrent.Text = "Unable to download " + _currentFile + " attempt " + _count + "/5";
                    });
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(1000);
                    _client.DownloadFileAsync(new Uri(_currentFile), _currentFilePath);
                    return;
                }
            }

            _continue = true;
            _count = 0;
        }

        private byte[] DownloadFile(string url) {
            try {
                var request = FileWebRequest.Create(url);
                using (var response = request.GetResponse()) {
                    using (var stream = response.GetResponseStream()) {
                        using (var memory = new MemoryStream()) {
                            stream.CopyTo(memory);
                            return memory.ToArray();
                        }
                    }
                }
            } catch {
                _count++;
                if (_count <= 5) {
                    Program.Menu.Invoke((MethodInvoker)delegate {
                        Program.Menu.lblCurrent.Text = "Unable to download the core file " + url + " attempt " + _count + "/5";
                    });
                    Application.DoEvents();
                    return DownloadFile(url);
                }
                Program.Menu.Invoke((MethodInvoker)delegate {
                    Program.Menu.lblTotal.Visible = false;
                    Program.Menu.prgCurrent.Visible = false;
                    Program.Menu.prgTotal.Visible = false;
                    Program.Menu.lblCurrent.Text = "Error: Please relaunch the updater.";
                });
                while (Program.Menu.Visible) {
                    Application.DoEvents();
                }
            }

            return null;
        }
    }
}
