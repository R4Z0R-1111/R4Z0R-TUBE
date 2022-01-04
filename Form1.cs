using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using YoutubeExplode;
using YoutubeExplode.Common;
using YoutubeExplode.Converter;
using FFmpeg;
using FFMpegCore;

namespace R4Z0R_TUBE
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        //------------SET UP THE VARIABLES NEEDED------
        string videoLink = "";
        string videoName = "";
        string playlistLink = "";
        string savePath = "";
        string ending = ".mp3";
        int alreadyloaded = 0;

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

            videoLink = textBox1.Text;

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

            playlistLink = textBox2.Text;

        }
        private void button3_Click(object sender, EventArgs e)
        {

                var folderBrowserDialog1 = new FolderBrowserDialog();

                // Show the FolderBrowserDialog.
                DialogResult result = folderBrowserDialog1.ShowDialog();
                if (result == DialogResult.OK)
                {
                    string folderName = folderBrowserDialog1.SelectedPath;
                    savePath = folderName;
                    textBox3.Text = savePath;
                }
            
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            var youtube = new YoutubeClient();
            var video = await youtube.Videos.GetAsync(videoLink);
            videoName = video.Title;
            var progress = new Progress<double>(p =>
            {
                progressBar1.Value = Convert.ToInt32(p * 100);
                var percentage = (p * 100);
                var percentage2 = Math.Truncate(percentage) / 1;
                label4.Text = Convert.ToString(percentage2) + " / " + "100%";
                label5.Text = "Video: 1 / 1";
            });
            await youtube.Videos.DownloadAsync(videoLink,savePath+"\\"+ Regex.Replace(videoName, @"[^a-zA-Z0-9\-]", "") + ending,progress);
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private async void button2_Click(object sender, EventArgs e)
        {
            var youtube2 = new YoutubeClient();
            var videos = await youtube2.Playlists.GetVideosAsync(playlistLink);
            foreach (var video in videos) {
                videoName = video.Title;
                alreadyloaded = alreadyloaded+1;
                string linkdw = video.Url;
                var progress = new Progress<double>(p =>
                {
                    progressBar1.Value = Convert.ToInt32(p * 100);
                    var percentage = (p * 100);
                    var percentage2 = Math.Truncate(percentage) / 1;
                    label4.Text = Convert.ToString(percentage2) + " / " + "100%";
                    label5.Text = "Video: " +alreadyloaded.ToString()+ " / " +videos.Count;
                });
                await youtube2.Videos.DownloadAsync(linkdw, savePath + "\\" + Regex.Replace(videoName, @"[^a-zA-Z0-9\-]", "") + ending, progress);
            }
        }
    }
}
