using LogHelperV2;
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
        private bool autoscroll;
        private CancellationTokenSource ts;

        // Join the dark theme 
        static readonly Color TEXTBACKCOLOR = System.Drawing.ColorTranslator.FromHtml("#252526");
        static readonly Color BACKCOLOR = System.Drawing.ColorTranslator.FromHtml("#2D2D30");
        static readonly Color INFOCOLOR = System.Drawing.ColorTranslator.FromHtml("#1E7AD4");
        static readonly Color WARNININGCOLOR = System.Drawing.ColorTranslator.FromHtml("#FFF380");
        static readonly Color MESSAGECOLOR = System.Drawing.ColorTranslator.FromHtml("#86A95A");
        static readonly Color DEBUGCOLOR = System.Drawing.ColorTranslator.FromHtml("#DCDCAA");
        static readonly Color ERRORCOLOR = System.Drawing.ColorTranslator.FromHtml("#B0572C");

        public FormLogViewer(string logFile)
        {
            Text = logFile;
            this.logFile = logFile;
            InitializeComponent();
            BackColor = BACKCOLOR;
            autoscroll = autoscroll_CheckBox.Checked;
        }


        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            ExecuteBackgroundWorker();
        }


        bool firstIteration = true;
        private void BackgroundWorker1_DoWork(CancellationToken ct)
        {
            var wh = new AutoResetEvent(false);
            var fsw = new FileSystemWatcher(".");
            fsw.Filter = logFile;
            fsw.EnableRaisingEvents = true;
            fsw.Changed += (s, e) => wh.Set();

            var fs = new FileStream(logFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            RTFGenerator rtf_generator = new RTFGenerator();
            Cursor.Current = Cursors.WaitCursor;

            using (var sr = new StreamReader(fs))
            {
                bool[] show = { info.Checked, warning.Checked, message.Checked, error.Checked, debug.Checked };
                var text = "";
                while (!ct.IsCancellationRequested) /// LOOP UNTIL THE FORM IS DISPOSED 
                {
                    text = sr.ReadLine();
                    if (text != null)
                    {
                        try
                        {
                            if (firstIteration)
                                rtf_generator.add(text, show);   // Load the file, convert the text to rtf in RTFGenerator
                            else
                                WriteInConsole(text);      // Add the new line dictly to the box

                            progressBar1.Invoke((MethodInvoker)delegate
                            {
                                progressBar1.Value = (Convert.ToInt32(Convert.ToDouble(fs.Position) / Convert.ToDouble(fs.Length) * 100));  // dont panic 
                            });
                        }
                        catch (Exception e1)
                        {
                            WriteInConsole(">>>>>>>>>>>" + e1.Message + "<<<<<<<<<<<");  // WRITE ERRORS ON THE SAME CONSOLE, ADDED A BIG DECORATOR 
                        }
                    }
                    else // NO MORE LINES 
                    {
                        if (firstIteration)
                        {
                            textBox_Console.Invoke((MethodInvoker)delegate
                            {
                                textBox_Console.LoadFile(GenerateStreamFromString(rtf_generator.get()), RichTextBoxStreamType.RichText); // STREAM RICH TEXT TO THE BOX
                            });


                            autoscroll_CheckBox.Invoke((MethodInvoker)delegate // GO TO THE END OF THE FILE 
                                {
                                    GoToEnd();
                                    firstIteration = false;
                                    Cursor.Current = Cursors.Default;

                                });
                        }
                        wh.WaitOne(1000);
                    }
                }
                wh.Close();
            }
        }


        public static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        Color _color = INFOCOLOR;

        private void WriteInConsole(string text)
        {
            // debug / info / warning / message / error

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
            if (header.Contains("INFO")) { if (info.Checked) _color = INFOCOLOR; else return; }
            else if (header.Contains("WARNING")) { if (warning.Checked) _color = WARNININGCOLOR; else return; }
            else if (header.Contains("MESSAGE")) { if (message.Checked) _color = MESSAGECOLOR; else return; }
            else if (header.Contains("ERROR")) { if (error.Checked) _color = ERRORCOLOR; else return; }
            else if (header.Contains("DEBUG")) { if (debug.Checked) _color = DEBUGCOLOR; else return; }

            textBox_Console.Invoke((ThreadStart)delegate
                        {
                            textBox_Console.SelectionStart = textBox_Console.TextLength;
                            textBox_Console.SelectionLength = 0;
                            textBox_Console.SelectionColor = _color;
                            textBox_Console.AppendText(text + Environment.NewLine);
                            if (autoscroll)
                            {
                                textBox_Console.SelectionStart = textBox_Console.TextLength;
                                textBox_Console.ScrollToCaret();
                            }
                        });
        }


        private void GoToEnd()
        {
            textBox_Console.Invoke((MethodInvoker)delegate
            {
                textBox_Console.SelectionStart = textBox_Console.TextLength;
                textBox_Console.ScrollToCaret();
            });
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

            textBox_Console.Clear();

            ts.Cancel();
            //            bgTask.Wait();
            //bgTask.Dispose();
            firstIteration = true;
            ExecuteBackgroundWorker();
        }

        Task bgTask;
        private void ExecuteBackgroundWorker()
        {
            ts = new CancellationTokenSource();


            bgTask = Task.Run(() => BackgroundWorker1_DoWork(ts.Token), ts.Token);

            //bgTask = Task.Run(() => BackgroundWorker1_DoWork(ct));
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
