using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogViewer
{
    public partial class FormLogViewer : Form
    {

        private string logFile;

        // Join the dark theme 
        readonly Color TEXTBACKCOLOR = System.Drawing.ColorTranslator.FromHtml("#252526");
        readonly Color BACKCOLOR = System.Drawing.ColorTranslator.FromHtml("#2D2D30");
        readonly Color INFOCOLOR = System.Drawing.ColorTranslator.FromHtml("#1E7AD4");
        readonly Color WARNININGCOLOR = System.Drawing.ColorTranslator.FromHtml("#FFF380");
        readonly Color MESSAGECOLOR = System.Drawing.ColorTranslator.FromHtml("#86A95A");
        readonly Color DEBUGCOLOR = System.Drawing.ColorTranslator.FromHtml("#DCDCAA");
        readonly Color ERRORCOLOR = System.Drawing.ColorTranslator.FromHtml("#B0572C");

        public FormLogViewer(string logFile)
        {
            Text = logFile;
            this.logFile = logFile;
            InitializeComponent();
            BackColor = BACKCOLOR;
            debugList = new List<string>(debug_arr);
            autoscroll = autoscroll_CheckBox.Checked;

        }
        bool autoscroll;
        CancellationTokenSource ts;
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            ts = new CancellationTokenSource();
            CancellationToken ct = ts.Token;
            textBox_Console.LoadFile(logFile, RichTextBoxStreamType.PlainText);
            // Task bgTask = Task.Run(() => BackgroundWorker1_DoWork(ct));
        }

        private void BackgroundWorker1_DoWork(CancellationToken ct)
        {
            var wh = new AutoResetEvent(false);
            var fsw = new FileSystemWatcher(".");
            fsw.Filter = logFile;
            fsw.EnableRaisingEvents = true;
            fsw.Changed += (s, e) => wh.Set();

            var fs = new FileStream(logFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            List<string> entries = new List<string>();

            using (var sr = new StreamReader(fs))
            {
                var s = "";
                while (true) /// Infinite loop (exit??)
                {
                    s = sr.ReadLine();
                    if (s != null)
                    {
                        try
                        {
                            WriteInConsole(s);
                            //entries.Add(s);

                            progressBar1.Invoke((MethodInvoker)delegate
                            {
                                progressBar1.Value = (Convert.ToInt32(Convert.ToDouble(fs.Position) / Convert.ToDouble(fs.Length) * 100));
                            });
                        }
                        catch (Exception e1)
                        {
                            Console.WriteLine(e1);
                        }
                    }
                    else
                    {

                        if (reachedEndFirstTime)
                            autoscroll_CheckBox.Invoke((MethodInvoker)delegate
                            {
//                                WriteAllTogether(entries);
                                autoscroll_CheckBox.Checked = true;
                                GoToEnd();
                                reachedEndFirstTime = false;
                            });
                        wh.WaitOne(1000);
                        if (ct.IsCancellationRequested)
                        {
                            // another thread decided to cancel
                            Console.WriteLine("task canceled");
                            break;
                        }
                    }
                }
                wh.Close();
            }
        }

        private void WriteAllTogether(List<string> entries)
        {
            foreach (var item in entries)
            {
                WriteInConsole(item);
            }
        }

        bool reachedEndFirstTime = true;


        string[] errors_arr = {
            "ClientCommManager.log",
            "ClientLog.txt"

        };
        string[] info_arr = {
            "Information",
            "INFO"

        };

        string[] debug_arr = {
            "VERBOSE",
            "DEBUG",
            "TRACE"
        };

        List<string> debugList;



        private LogType lastLogType = LogType.info;
        private void WriteInConsole(string text)
        {
            // debug / info / warning => message / error
            string header = "";

            int ini = text.IndexOf('\t');
            if (ini > 0)
            {

                int end = text.IndexOf('\t', text.IndexOf('\t') + 1);
                header = text.Substring(ini + 1, end - ini - 1);
            }
            else if (text.Length > 50)
            {
                header = text.Substring(0, 50);
            }


            header = header.ToUpper();

            LogType type = lastLogType;
            if (header.Contains("INFO")) type = LogType.info;
            else if (header.Contains("WARNING")) type = LogType.warning;
            else if (header.Contains("MESSAGE")) type = LogType.message;
            else if (header.Contains("ERROR")) type = LogType.error;
            else if (debugList.Contains(header)) type = LogType.debug;
            lastLogType = type;

            //  if (type != LogType.info)
            {

                textBox_Console.Invoke((ThreadStart)delegate
                {
                    Color _color;
                    switch (type)
                    {
                        case LogType.debug:
                            _color = DEBUGCOLOR;
                            break;
                        case LogType.warning:
                            _color = WARNININGCOLOR;
                            break;
                        case LogType.message:
                            _color = MESSAGECOLOR;
                            break;
                        case LogType.info:
                            _color = INFOCOLOR;
                            break;
                        case LogType.error:
                            _color = ERRORCOLOR;
                            break;
                        default:
                            _color = Color.White;
                            break;
                    }

                    textBox_Console.SelectionStart = textBox_Console.TextLength;
                    textBox_Console.SelectionLength = 0;

                    textBox_Console.SelectionColor = _color;
                    textBox_Console.AppendText(text + Environment.NewLine);
                    textBox_Console.SelectionColor = textBox_Console.ForeColor;


                    if (autoscroll)
                    {
                        textBox_Console.SelectionStart = textBox_Console.TextLength;
                        textBox_Console.ScrollToCaret();
                    }
                });
            }
        }


        private void GoToEnd()
        {
            textBox_Console.Invoke((MethodInvoker)delegate
            {
                textBox_Console.SelectionStart = textBox_Console.TextLength;
                textBox_Console.ScrollToCaret();
            });
        }
        enum LogType
        {
            debug,
            warning,
            message,
            info,
            error,
        }

        private void ClearLogs_Click(object sender, EventArgs e)
        {
            try
            {
                System.IO.File.WriteAllText(logFile, string.Empty);
                textBox_Console.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SaveLog_Click(object sender, EventArgs e)
        {
            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            saveFileDialog1.FileName = Path.GetFileName(logFile);

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    StreamWriter sw = new StreamWriter(myStream);
                    sw.Write(textBox_Console.Text);
                    sw.Close();
                    myStream.Close();
                }
            }
        }

        //Dictionary<>
        private void CheckedChanged(object sender, EventArgs e)
        {
            //  TODO
            //    ((CheckBox)e).Name
        }


        private void button2_Click(object sender, EventArgs e)
        {
            textBox_Console.Clear();
        }


        private void autoscroll_CheckedChanged(object sender, EventArgs e)
        {
            autoscroll = autoscroll_CheckBox.Checked;
        }




        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                ts.Cancel();
                // Dispose stuff here
            }


            base.Dispose(disposing);
        }
    }
}
