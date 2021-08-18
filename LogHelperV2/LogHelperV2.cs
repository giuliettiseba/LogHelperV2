using LogViewer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogHelperV2
{
    public partial class LogHelperV2Form : Form
    {

        // Join the dark theme 
        readonly Color TEXTBACKCOLOR = System.Drawing.ColorTranslator.FromHtml("#252526");
        readonly Color BACKCOLOR = System.Drawing.ColorTranslator.FromHtml("#2D2D30");
        readonly Color INFOCOLOR = System.Drawing.ColorTranslator.FromHtml("#1E7AD4");
        readonly Color MESSAGECOLOR = System.Drawing.ColorTranslator.FromHtml("#86A95A");
        readonly Color DEBUGCOLOR = System.Drawing.ColorTranslator.FromHtml("#DCDCAA");
        readonly Color ERRORCOLOR = System.Drawing.ColorTranslator.FromHtml("#B0572C");
        readonly Color SNOW = System.Drawing.ColorTranslator.FromHtml("#FFFAFA");


        static ImageList _imageList;

        public LogHelperV2Form()
        {
            InitializeComponent();
            BackColor = BACKCOLOR;
            treeView1.BackColor = BACKCOLOR;
            treeView1.ForeColor = SNOW;
            treeView1.ImageList = LogHelperV2Form.ImageList;
            treeView1.ImageList.ImageSize = new Size(32, 32);
        }
        string INITIALPATH_MILESTONE;
        string INITIALPATH_VIDEOOS;
        protected override void OnHandleCreated(EventArgs e)
        {
            INITIALPATH_MILESTONE = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\Milestone";
            INITIALPATH_VIDEOOS = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\VideoOS";
            try
            {
                treeView1.Nodes.Add(TraverseDirectory(INITIALPATH_MILESTONE));
                treeView1.Nodes.Add(TraverseDirectory(INITIALPATH_VIDEOOS));
                treeView1.Nodes[0].Expand();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }





            backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += new DoWorkEventHandler(backgroundWorker_DoWork);
            backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker_ProgressChanged);
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.WorkerSupportsCancellation = true;



        }


        private TreeNode TraverseDirectory(string directoryName)
        {
            string[] favs_arr = {
            "ClientCommManager",
            "ClientLog",
            "DeviceHandling",
            DateTime.Now.ToString("yyyyMMdd"),
            DateTime.Now.ToString("yyyy_MM_dd"),
            DateTime.Now.ToString("yyyy-MM-dd")
        };

            List<string> favs = new List<string>(favs_arr);

            DirectoryInfo directory = new DirectoryInfo(directoryName);
            TreeNode directoryNode = new TreeNode(directory.Name);
            directoryNode.ImageKey = "folder";
            directoryNode.Tag = directory;

            foreach (var subdirectory in Directory.GetDirectories(directory.FullName))
            {
                directoryNode.Nodes.Add(TraverseDirectory(subdirectory));
            }

            DirectoryInfo info = new DirectoryInfo(directory.FullName);
            FileInfo[] files = info.GetFiles().OrderByDescending(p => p.LastWriteTime).ToArray();

            foreach (FileInfo file in files)
            {
                //FileInfo file = new FileInfo(fileName);
                if (file.Extension == ".txt" || file.Extension == ".log")
                {


                    TreeNode fileNode = new TreeNode($"[{ file.LastWriteTime }] - {file.Name} ({ ConvertSizeToString(file.Length) }) ");
                    fileNode.Tag = file;
                    directoryNode.Nodes.Add(fileNode);

                    if (FilterFavs(file.Name, favs))
                    {
                        fileNode.ImageKey = "fav_file";
                        fileNode.SelectedImageKey = "fav_file";
                    }
                    else
                    {
                        fileNode.ImageKey = "file";
                        fileNode.SelectedImageKey = "file";
                    }
                }
            }
            return directoryNode;
        }

        private static string ConvertSizeToString(long size)
        {
            string _size;
            if (size < 1024)
                _size = size + "B";
            else if (size / 1024 < 1024) _size = (size / 1024) + "KB";
            else _size = size / 1024 / 1024 + "MB";
            return _size;
        }

        private bool FilterFavs(string name, List<string> favs)
        {
            foreach (string fav in favs)
                if (name.Contains(fav)) return true;
            return false;
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {

            if (treeView1.SelectedNode != null && treeView1.SelectedNode.Tag is FileInfo)
            {
                OpenLogFile(((FileInfo)treeView1.SelectedNode.Tag).FullName);
            }
        }

        private void OpenLogFile(string fullName)
        {
            FormLogViewer flv = new FormLogViewer(fullName);
            flv.Text = fullName;
            flv.Show();
        }


        public static ImageList ImageList
        {
            get
            {
                if (_imageList == null)
                {
                    _imageList = new ImageList();
                    _imageList.ColorDepth = ColorDepth.Depth32Bit;
                    _imageList.Images.Add("folder", Properties.Resources.folder);
                    _imageList.Images.Add("file", Properties.Resources.file);
                    _imageList.Images.Add("fav_file", Properties.Resources.fav_file);
                }
                return _imageList;
            }
        }

        private void OpenOther_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    OpenLogFile(openFileDialog.FileName);
                }
            }
        }











        //---/// UPDATE IN BACKGROND IS NOT WORKING 





       private BackgroundWorker backgroundWorker;

        private void button1_Click(object sender, EventArgs e)
        {

            if (backgroundWorker.IsBusy)
            {
                backgroundWorker.CancelAsync();
            }
            else
            {
                backgroundWorker.RunWorkerAsync();
            }
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Console.WriteLine(e.ProgressPercentage);
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!backgroundWorker.CancellationPending)
            {
                nodeNumer = 0;

                 foreach (TreeNode node in treeView1.Nodes)
                {
                    RefreshRecursive(node);
                };
               Thread.Sleep(5000);
            }

         
        }

        int nodeNumer = 0;
        private void RefreshRecursive(TreeNode node)
        {
            backgroundWorker.ReportProgress(nodeNumer++, treeView1.Nodes.Count);
            if (node.Tag is FileInfo)
            {
                FileInfo file = new FileInfo((node.Tag as FileInfo).FullName);

                treeView1.Invoke((MethodInvoker)delegate
                {
                    //treeView1.BeginUpdate();
                    //treeView1.SuspendLayout();
                    node.Text = ($"[{ file.LastWriteTime }] - {file.Name} ({ ConvertSizeToString(file.Length) }) ");
                    //treeView1.ResumeLayout(false);
                    //treeView1.PerformLayout();
                    //treeView1.EndUpdate();

                });

            }
            else
            {
                foreach (TreeNode tn in node.Nodes)
                {
                    RefreshRecursive(tn);
                }
            }

        }
    }
}
