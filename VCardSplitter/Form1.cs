using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VCardSplitter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void ofdSource_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void btnSelectSource_Click(object sender, EventArgs e)
        {
            if (this.ofdSource.ShowDialog() == DialogResult.OK)
            {
                this.tbSource.Text = this.ofdSource.FileName;
            }
        }

        private void btnSelectTarget_Click(object sender, EventArgs e)
        {
            if (this.fbdTarget.ShowDialog() == DialogResult.OK)
            {
                this.tbTarget.Text = this.fbdTarget.SelectedPath;
            }
        }

        private void btnSplit_Click(object sender, EventArgs e)
        {
            String source = this.tbSource.Text;
            String target = this.tbTarget.Text;

            if (!File.Exists(source)) return;
            if (!Directory.Exists(target)) return;

            String[] lines = File.ReadAllLines(source);
            String contents = String.Empty;
            String name = String.Empty;
            Boolean isBegin = false;
            Boolean isEnd = false;
            Int32 iFiles = 0;
            this.progress.Maximum = lines.Length;
            foreach (String line in lines)
            {
                if (line == "BEGIN:VCARD") isBegin = true;
                if (line == "END:VCARD") isEnd = true;
                if (line.StartsWith("FN:")) name = line.Substring(3) + ".vcf";
                if (isBegin) contents += System.Environment.NewLine + line;
                if (isEnd)
                {
                    File.WriteAllText(Path.Combine(target, name), contents);
                    isBegin = false;
                    isEnd = false;
                    contents = String.Empty;
                    iFiles++;
                }
                this.progress.PerformStep();
            }

            MessageBox.Show("Your VCard was split into " + iFiles.ToString() + " files.", "Success", MessageBoxButtons.OK);
        }
    }
}
