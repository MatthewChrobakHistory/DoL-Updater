using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections.Generic;

namespace DivisionOfLifeUpdater
{
    public class NewPatcher
    {
        private List<GameFile> Files = new List<GameFile>();
        private WebClient _client = new WebClient();
        private string _baseURL = "http://divisionoflife.com/update/";

        private bool _continue = false;
        private string _currentFile = "";
        private string _currentFilePath = "";
        private string _rawFile = "";
        private bool _error = false;
        private int _start = 0;
        private int _dlErrorRetryCount = 0;

        private bool _needsNewLauncher;
        private bool _gotNewLauncher;

        #region Zip Files
        // PDB
        private bool _pdbRequired = false;
        private const string _pdbFile = "\\Game\\pdb.zip";
        private long _pdbSize;

        // Assets
        private bool _assetsRequired = false;
        private const string _assetsPath = "\\Game\\Division Of Life_Data\\";
        private const string _assetsFile = _assetsPath + "assets.zip";
        private long _assetsSize;

        private bool _extractDone = false;
        #endregion

        private int _downloadErrorCount = 0;

        private string _startupPath;
        private string _revisionFile;
        private int _clientRevision = 0;
        private int _serverRevision = 0;

        public void BeginProcess(string[] args) {
            if (args.Length > 0) {
                if (args[0] == "NewLauncher") {
                    Console.WriteLine("NEW LAUNCHER");
                    this._gotNewLauncher = true;

                    if (File.Exists(_startupPath + "NewLauncherHandler.exe")) {
                        DeleteFileWait(_startupPath + "NewLauncherHandler.exe");
                    }
                }
            }
            this.Prep();
            this.Start();
            this.End();
        }

        private void Prep() {
            // Establish necessary paths.
            this._startupPath = AppDomain.CurrentDomain.BaseDirectory;
            this._revisionFile = _startupPath + "Revision.txt";

            // Load the revision if we can find it.
            if (File.Exists(_revisionFile)) {
                _clientRevision = Int32.Parse(File.ReadAllText(_revisionFile));
            }

            // Download and convert the folder list into a byte stream, and create
            // the necessary folders.
            var folderList = new DataBuffer(DownloadFile(_baseURL + "FolderList.dat"));
            int folderCount = folderList.ReadInt();
            for (int i = 0; i < folderCount; i++) {
                Directory.CreateDirectory(_startupPath + folderList.ReadString());
                Application.DoEvents();
            }

            // Download and convert the file list into a byte stream.
            var fileList = new DataBuffer(DownloadFile(_baseURL + "FileList.dat"));

            // Get the current product revision as determined by the FileIndexer.
            _serverRevision = fileList.ReadInt();

            // Create the webclient event handlers.
            _client.DownloadFileCompleted += _client_DownloadFileCompleted;
            _client.DownloadProgressChanged += _client_DownloadProgressChanged;

            // Loop through the file list and retrieve information about the required files.
            int fileCount = fileList.ReadInt();
            for (int i = 0; i < fileCount; i++) {
                var file = new GameFile(fileList.ReadString(), fileList.ReadString(), fileList.ReadInt(), fileList.ReadLong());

                if (file.ClientPath.ToLower().Contains("newlauncher.exe")) {
                    if (!this._gotNewLauncher) {
                        if (file.Revision > _clientRevision) {
                            this._needsNewLauncher = true;

                            var processer = new GameFile("\\NewLauncherHandler.exe", "\\NewLauncherHandler.exe", int.MaxValue, -1);

                            this.Files.Clear();
                            this.Files.Add(processer);
                            this.Files.Add(file);
                            break;
                        }
                    }
                    continue;
                }

                // If the file already exists and the revision it was introduced in
                // is older than our current version, then continue to the next
                // file in the file list.
                string path = _startupPath + file.ClientPath;
                if (File.Exists(path)) {
                    var fi = new FileInfo(path);
                    if (fi.Length != file.Size) {

                        if (file.ServerPath.EndsWith(".cfg")) {
                            continue;
                        }
                        DeleteFileWait(path);
                    } else if (file.Revision <= this._clientRevision) {
                        continue;
                    }
                }

                // Remember the size of the zip file.
                if (file.ServerPath.EndsWith(".zip")) {
                    if (file.ServerPath.ToLower().Contains("pdb")) {
                        this._pdbSize = file.Size;
                    } else if (file.ServerPath.ToLower().Contains("assets.zip")) {
                        this._assetsSize = file.Size;
                    }
                    continue;
                }

                // Check to see if it's an assets file. 
                // If it is, we can probably save time by downloading the assets.zip file.
                // Make sure we only download it once.
                if (file.ServerPath.ToLower().EndsWith(".assets") || file.ServerPath.ToLower().EndsWith(".ress") || file.ServerPath.ToLower().EndsWith(".resource")) {
                    if (!_assetsRequired) {
                        _assetsRequired = true;

                        file.ServerPath = file.ServerPath.Remove(file.ServerPath.IndexOf("\\")) + _assetsFile;
                        file.ClientPath = _assetsFile;

                        if (File.Exists(_startupPath + file.ClientPath)) {
                            var fi = new FileInfo(_startupPath + file.ClientPath);
                            if (fi.Length != this._assetsSize) {
                                DeleteFileWait(_startupPath + file.ClientPath);
                            } else {
                                continue;
                            }
                        }
                    } else {
                        continue;
                    }
                }

                if (file.ServerPath.ToLower().EndsWith(".pdb")) {
                    if (!_pdbRequired) {
                        _pdbRequired = true;

                        file.ServerPath = file.ServerPath.Remove(file.ServerPath.IndexOf("\\")) + _pdbFile;
                        file.ClientPath = _pdbFile;

                        if (File.Exists(_startupPath + file.ClientPath)) {
                            var fi = new FileInfo(_startupPath + file.ClientPath);
                            if (fi.Length != this._pdbSize) {
                                DeleteFileWait(_startupPath + file.ClientPath);
                            } else {
                                continue;
                            }
                        }
                    } else {
                        continue;
                    }
                }


                this.Files.Add(file);
            }
        }

        private void Start() {
            for (int i = 0; i < Files.Count; i++) {
                var file = Files[i];
                decimal value = Decimal.Divide(i + 1, Files.Count);
                int prcn = (int)(value * 100);

                Program.Menu.Invoke((MethodInvoker)delegate {
                    Program.Menu.lblTotal.Text = "Files Remaining: " + (Files.Count - i).ToString();
                    Program.Menu.prgTotal.Value = prcn;
                });

                try {
                    _continue = false;
                    Uri url = new Uri(_baseURL + file.ServerPath);
                    _currentFile = _baseURL + file.ServerPath;
                    _rawFile = _currentFile;
                    while (_rawFile.Contains("\\")) {
                        if (_rawFile.StartsWith("\\")) {
                            _rawFile = _rawFile.Remove(0, 1);
                        } else {
                            _rawFile = _rawFile.Remove(0, _rawFile.IndexOf("\\"));
                        }
                    }
                    _currentFilePath = _startupPath + file.ClientPath;
                    _start = Environment.TickCount;
                    _client.DownloadFileAsync(url, _startupPath + file.ClientPath);
                } catch {
                    Program.Menu.Invoke((MethodInvoker)delegate {
                        Program.Menu.lblCurrent.Text = "Unable to download " + _currentFile + " attempt " + _dlErrorRetryCount + "/5";
                    });
                }

                while (!_continue) {
                    Application.DoEvents();
                }
            }
        }

        private void End() {

            // Do we need a new launcher?
            if (this._needsNewLauncher) {
                string myPath = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
                myPath = myPath.Replace(".vshost", "");
                string newPath = this._startupPath + Files[1].ClientPath;

                Process.Start(this._startupPath + "\\NewLauncherHandler.exe", myPath + "¨" + newPath);
                Environment.Exit(7);
                return;
            }

            // Decompress files
            Program.Menu.Invoke((MethodInvoker)delegate {

                Program.Menu.lblCurrent.Text = "Extracting files...";
                Program.Menu.prgCurrent.Visible = false;
                Program.Menu.prgTotal.Visible = false;
                Program.Menu.lblTotal.Visible = false;
            });


            if (_assetsRequired) {
                new Thread(ExtractAssets).Start();

                while (!_extractDone) {
                    Application.DoEvents();
                }
            }

            if (_pdbRequired) {
                _extractDone = false;
                new Thread(ExtractPDB).Start();

                while (!_extractDone) {
                    Application.DoEvents();
                }
            }

            // Store the current revision we installed, and terminate the program
            // with a message of success.
            File.WriteAllText(_revisionFile, _serverRevision.ToString());

            Program.Menu.Invoke((MethodInvoker)delegate {
                Program.Menu.lblCurrent.Visible = false;
                if (_error) {
                    Program.Menu.lblRetry.Visible = true;
                } else {
                    Program.Menu.lblPlay.Visible = true;
                }
            });
        }

        private void _client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e) {
            int percentage = e.ProgressPercentage;

            decimal Received = Decimal.Parse((e.BytesReceived / 1024d / 1024d).ToString("0.00"));
            decimal Total = Decimal.Parse((e.TotalBytesToReceive / 1024d / 1024d).ToString("0.00"));
            int secondselated = (_start - Environment.TickCount) + 1;
            string time = (e.BytesReceived / -1024d / secondselated).ToString("0.00");


            Program.Menu.Invoke((MethodInvoker)delegate {
                Program.Menu.lblCurrent.Text = _rawFile + ": (" + time + "mb/s) " + (Total - Received) + " MB remaining";
                Program.Menu.prgCurrent.Value = percentage;
            });
        }

        private void _client_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e) {
            if (e.Error != null) {
                _dlErrorRetryCount++;
                if (_dlErrorRetryCount >= 5) {
                    if (File.Exists(_currentFilePath)) {
                        File.Delete(_currentFilePath);
                    }
                    _continue = true;
                    _dlErrorRetryCount = 0;
                    _error = true;
                    return;
                } else {
                    Program.Menu.Invoke((MethodInvoker)delegate {
                        Program.Menu.lblCurrent.Text = "Unable to download " + _currentFile + " attempt " + _dlErrorRetryCount + "/5";
                    });
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(1000);
                    _client.DownloadFileAsync(new Uri(_currentFile), _currentFilePath);
                    return;
                }
            }

            _continue = true;
            _dlErrorRetryCount = 0;
        }

        private void ExtractAssets() {
            IO.Compression.DecompressDirectory(AppDomain.CurrentDomain.BaseDirectory + _assetsFile, AppDomain.CurrentDomain.BaseDirectory + _assetsPath);
            _extractDone = true;
        }

        private void ExtractPDB() {
            IO.Compression.DecompressDirectory(AppDomain.CurrentDomain.BaseDirectory + _pdbFile, AppDomain.CurrentDomain.BaseDirectory + "\\Game\\");
            _extractDone = true;
        }

        private void DeleteFileWait(string path) {
            // Delete the file.
            File.Delete(path);

            // Wait until the file is fully deleted.
            while (File.Exists(path)) {
                Application.DoEvents();
            }
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
                _downloadErrorCount++;
                if (_downloadErrorCount <= 5) {
                    Program.Menu.Invoke((MethodInvoker)delegate {
                        Program.Menu.lblCurrent.Text = "Unable to download the core file " + url + " attempt " + _downloadErrorCount + "/5";
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

    public class GameFile
    {
        public string ClientPath;
        public string ServerPath;
        public int Revision;
        public long Size;

        public GameFile(string clientpath, string serverpath, int revision, long size) {
            this.ClientPath = clientpath;
            this.ServerPath = serverpath;
            this.Revision = revision;
            this.Size = size;
        }
    }
}
