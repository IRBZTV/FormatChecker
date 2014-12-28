using MediaInfoNET;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;

using System.Text;

using System.Windows.Forms;

namespace FormatChecker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.AllowDrop = true;
            this.DragEnter += Form1_DragEnter;
            //   this.DragDrop += Form1_DragDrop;
        }
        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }

            textBox1.BackColor = Color.LightSkyBlue;
            textBox1.Text = "DROP FILE HERE";
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] filePaths = (string[])(e.Data.GetData(DataFormats.FileDrop));
                if (Path.GetExtension(filePaths[0].ToString()).ToLower() == ".mp4")
                {
                    textBox1.Text = filePaths[0].ToString();
                    textBox1.BackColor = Color.LightGreen;
                    Checker();
                }
                else
                {
                    textBox1.Text = "FILE IS NOT.MP4";
                    textBox1.BackColor = Color.Red;
                }
            }
        }

        protected void Checker()
        {
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();


            MediaFile aviFile = new MediaFile(textBox1.Text);


            //Video
            label1.Text = aviFile.Video[0].DurationStringAccurate;

            dataGridView1.Rows.Add("Format",
                System.Configuration.ConfigurationSettings.AppSettings["VideoFormat"].Trim(),
                "---",
                aviFile.Video[0].Format);

            //Check Format Row:
            if (aviFile.Video[0].Format.ToLower() != System.Configuration.ConfigurationSettings.AppSettings["VideoFormat"].Trim().ToLower())
            {
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0].Style.BackColor = Color.LightPink;
            }
            else
            {
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0].Style.BackColor = Color.White;
            }



            string VBitrateMode = "Constant";

            aviFile.Video[0].Properties.TryGetValue("Frame rate mode", out VBitrateMode);

            if (VBitrateMode == "Variable")
            {
                string VBitrateVbrMax = "";
                aviFile.Video[0].Properties.TryGetValue("Maximum bit rate", out VBitrateVbrMax);
                dataGridView1.Rows.Add("Bitrate Max", "", "", VBitrateVbrMax);

                string VBitrateVbrMin = "";
                aviFile.Video[0].Properties.TryGetValue("Bit rate", out VBitrateVbrMin);
                dataGridView1.Rows.Add("Bitrate Min", "", "", VBitrateVbrMin);
            }
            else
            {
                dataGridView1.Rows.Add("Bitrate",
                    System.Configuration.ConfigurationSettings.AppSettings["VideoBitrateMin"].Trim(),
                    System.Configuration.ConfigurationSettings.AppSettings["VideoBitrateMax"].Trim(),
                    aviFile.Video[0].Bitrate);

                //Check Bitrate Row:
                if (!(aviFile.Video[0].Bitrate <= int.Parse(System.Configuration.ConfigurationSettings.AppSettings["VideoBitrateMax"].Trim())
                    &&
                    aviFile.Video[0].Bitrate >= int.Parse(System.Configuration.ConfigurationSettings.AppSettings["VideoBitrateMin"].Trim())))
                {
                    dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0].Style.BackColor = Color.LightPink;
                }
                else
                {
                    dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0].Style.BackColor = Color.White;
                }


                VBitrateMode = "Constant";
            }

            dataGridView1.Rows.Add("Frame Mode",
                System.Configuration.ConfigurationSettings.AppSettings["VideoFrameMode"].Trim(),
                "---",
                VBitrateMode);

            if (VBitrateMode.ToLower() != System.Configuration.ConfigurationSettings.AppSettings["VideoFrameMode"].Trim().ToLower())
            {
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0].Style.BackColor = Color.LightPink;
            }
            else
            {
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0].Style.BackColor = Color.White;
            }


            dataGridView1.Rows.Add("FormatID",
                System.Configuration.ConfigurationSettings.AppSettings["VideoFormatID"].Trim(),
                "---",
                aviFile.Video[0].FormatID);

            //Check Video Format Id Row:
            if (aviFile.Video[0].FormatID.ToLower() != System.Configuration.ConfigurationSettings.AppSettings["VideoFormatID"].Trim().ToLower())
            {
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0].Style.BackColor = Color.LightPink;
            }
            else
            {
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0].Style.BackColor = Color.White;
            }


            dataGridView1.Rows.Add("FrameRate",
                System.Configuration.ConfigurationSettings.AppSettings["VideoFrameRateMin"].Trim(),
                System.Configuration.ConfigurationSettings.AppSettings["VideoFrameRateMax"].Trim(),
                aviFile.Video[0].FrameRate);
            //Check VideoBitrate
            if (!(aviFile.Video[0].FrameRate <= double.Parse(System.Configuration.ConfigurationSettings.AppSettings["VideoFrameRateMax"].Trim())
                   &&
                   aviFile.Video[0].FrameRate >= double.Parse(System.Configuration.ConfigurationSettings.AppSettings["VideoFrameRateMin"].Trim())))
            {
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0].Style.BackColor = Color.LightPink;
            }
            else
            {
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0].Style.BackColor = Color.White;
            }

            dataGridView1.Rows.Add("Height",
                 System.Configuration.ConfigurationSettings.AppSettings["VideoHeightMin"].Trim(),
                 System.Configuration.ConfigurationSettings.AppSettings["VideoHeightMax"].Trim(),
                aviFile.Video[0].Height);
            //Check Height Row:
            if (!(aviFile.Video[0].Height <= int.Parse(System.Configuration.ConfigurationSettings.AppSettings["VideoHeightMax"].Trim())
                &&
                aviFile.Video[0].Height >= int.Parse(System.Configuration.ConfigurationSettings.AppSettings["VideoHeightMin"].Trim())))
            {
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0].Style.BackColor = Color.LightPink;
            }
            else
            {
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0].Style.BackColor = Color.White;
            }

            dataGridView1.Rows.Add("Width",
                 System.Configuration.ConfigurationSettings.AppSettings["VideoWidthMin"].Trim(),
                 System.Configuration.ConfigurationSettings.AppSettings["VideoWidthMax"].Trim(),
                aviFile.Video[0].Width);

            //Check Video Width Row:
            if (!(aviFile.Video[0].Width <= int.Parse(System.Configuration.ConfigurationSettings.AppSettings["VideoWidthMax"].Trim())
              &&
              aviFile.Video[0].Width >= int.Parse(System.Configuration.ConfigurationSettings.AppSettings["VideoWidthMin"].Trim())))
            {
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0].Style.BackColor = Color.LightPink;
            }
            else
            {
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0].Style.BackColor = Color.White;
            }

            dataGridView1.Rows.Add("IsInterlaced",
                System.Configuration.ConfigurationSettings.AppSettings["VideoIsInterlaced"].Trim(),
                "---",
                aviFile.Video[0].IsInterlaced);
            //Check Video IsInterlaced Row:
            if (aviFile.Video[0].IsInterlaced.ToString().ToLower() != System.Configuration.ConfigurationSettings.AppSettings["VideoIsInterlaced"].Trim().ToLower())
            {
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0].Style.BackColor = Color.LightPink;
            }
            else
            {
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0].Style.BackColor = Color.White;
            }


            dataGridView1.ClearSelection();



            //Audio
            if (aviFile.Audio.Count > 0)
            {
                label2.Text = aviFile.Audio[0].DurationStringAccurate;
                dataGridView2.Rows.Add("Format", "", "", aviFile.Audio[0].Format);
                dataGridView2.Rows.Add("Bitrate", "", "", aviFile.Audio[0].Bitrate);
                dataGridView2.Rows.Add("SamplingRate", "", "", aviFile.Audio[0].SamplingRate);
                dataGridView2.Rows.Add("Channel(s)", "", "", aviFile.Audio[0].Properties["Channel(s)"].ToString());
            }

            dataGridView2.ClearSelection();
        }


    }
}
