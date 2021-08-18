
namespace LogViewer
{
    partial class FormLogViewer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
      /*  protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            
            base.Dispose(disposing);
        }*/

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLogViewer));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.autoscroll_CheckBox = new System.Windows.Forms.CheckBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.message = new System.Windows.Forms.CheckBox();
            this.info = new System.Windows.Forms.CheckBox();
            this.error = new System.Windows.Forms.CheckBox();
            this.debug = new System.Windows.Forms.CheckBox();
            this.Others = new System.Windows.Forms.CheckBox();
            this.textBox_Console = new System.Windows.Forms.RichTextBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.textBox_Console, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.progressBar1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(800, 450);
            this.tableLayoutPanel1.TabIndex = 10;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.autoscroll_CheckBox);
            this.flowLayoutPanel1.Controls.Add(this.button3);
            this.flowLayoutPanel1.Controls.Add(this.button2);
            this.flowLayoutPanel1.Controls.Add(this.message);
            this.flowLayoutPanel1.Controls.Add(this.info);
            this.flowLayoutPanel1.Controls.Add(this.error);
            this.flowLayoutPanel1.Controls.Add(this.debug);
            this.flowLayoutPanel1.Controls.Add(this.Others);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 404);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(794, 43);
            this.flowLayoutPanel1.TabIndex = 15;
            // 
            // autoscroll_CheckBox
            // 
            this.autoscroll_CheckBox.AutoSize = true;
            this.autoscroll_CheckBox.Checked = true;
            this.autoscroll_CheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.autoscroll_CheckBox.Location = new System.Drawing.Point(3, 3);
            this.autoscroll_CheckBox.Name = "autoscroll_CheckBox";
            this.autoscroll_CheckBox.Size = new System.Drawing.Size(74, 17);
            this.autoscroll_CheckBox.TabIndex = 21;
            this.autoscroll_CheckBox.Text = "AutoScroll";
            this.autoscroll_CheckBox.UseVisualStyleBackColor = true;
            this.autoscroll_CheckBox.CheckedChanged += new System.EventHandler(this.autoscroll_CheckedChanged);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Location = new System.Drawing.Point(83, 3);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(116, 33);
            this.button3.TabIndex = 14;
            this.button3.Text = "Save Logs";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.SaveLog_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Location = new System.Drawing.Point(205, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(116, 33);
            this.button2.TabIndex = 16;
            this.button2.Text = "Clear Screen";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // message
            // 
            this.message.AutoSize = true;
            this.message.Checked = true;
            this.message.CheckState = System.Windows.Forms.CheckState.Checked;
            this.message.Location = new System.Drawing.Point(327, 3);
            this.message.Name = "message";
            this.message.Size = new System.Drawing.Size(69, 17);
            this.message.TabIndex = 19;
            this.message.Text = "Message";
            this.message.UseVisualStyleBackColor = true;
            this.message.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // info
            // 
            this.info.AutoSize = true;
            this.info.Checked = true;
            this.info.CheckState = System.Windows.Forms.CheckState.Checked;
            this.info.Location = new System.Drawing.Point(402, 3);
            this.info.Name = "info";
            this.info.Size = new System.Drawing.Size(44, 17);
            this.info.TabIndex = 18;
            this.info.Text = "Info";
            this.info.UseVisualStyleBackColor = true;
            this.info.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // error
            // 
            this.error.AutoSize = true;
            this.error.Checked = true;
            this.error.CheckState = System.Windows.Forms.CheckState.Checked;
            this.error.Location = new System.Drawing.Point(452, 3);
            this.error.Name = "error";
            this.error.Size = new System.Drawing.Size(48, 17);
            this.error.TabIndex = 20;
            this.error.Text = "Error";
            this.error.UseVisualStyleBackColor = true;
            this.error.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // debug
            // 
            this.debug.AutoSize = true;
            this.debug.Checked = true;
            this.debug.CheckState = System.Windows.Forms.CheckState.Checked;
            this.debug.Location = new System.Drawing.Point(506, 3);
            this.debug.Name = "debug";
            this.debug.Size = new System.Drawing.Size(58, 17);
            this.debug.TabIndex = 17;
            this.debug.Text = "Debug";
            this.debug.UseVisualStyleBackColor = true;
            this.debug.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // Others
            // 
            this.Others.AutoSize = true;
            this.Others.Checked = true;
            this.Others.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Others.Location = new System.Drawing.Point(570, 3);
            this.Others.Name = "Others";
            this.Others.Size = new System.Drawing.Size(57, 17);
            this.Others.TabIndex = 21;
            this.Others.Text = "Others";
            this.Others.UseVisualStyleBackColor = true;
            this.Others.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // textBox_Console
            // 
            this.textBox_Console.BackColor = System.Drawing.SystemColors.InfoText;
            this.textBox_Console.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Console.Location = new System.Drawing.Point(3, 23);
            this.textBox_Console.Name = "textBox_Console";
            this.textBox_Console.Size = new System.Drawing.Size(794, 375);
            this.textBox_Console.TabIndex = 11;
            this.textBox_Console.TabStop = false;
            this.textBox_Console.Text = "";
            // 
            // progressBar1
            // 
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressBar1.Location = new System.Drawing.Point(3, 3);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(794, 14);
            this.progressBar1.TabIndex = 12;
            // 
            // FormLogViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormLogViewer";
            this.Text = "Form1";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.RichTextBox textBox_Console;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.CheckBox debug;
        private System.Windows.Forms.CheckBox info;
        private System.Windows.Forms.CheckBox error;
        private System.Windows.Forms.CheckBox message;
        private System.Windows.Forms.CheckBox Others;
        private System.Windows.Forms.CheckBox autoscroll_CheckBox;
    }
}

