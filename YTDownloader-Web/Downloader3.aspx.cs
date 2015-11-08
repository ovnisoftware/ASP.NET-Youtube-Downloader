using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YoutubeDownloader;
using YoutubeExtractor;

namespace YTDownloader_Web
{
    public partial class Downloader3 : System.Web.UI.Page
    {
        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                Tuple<bool, string> isGoodLink = ValidateLink();
                if (isGoodLink.Item1 == true)
                    DisplayLinks(isGoodLink.Item2);
            }
            catch (Exception ex)
            {
                Label2.Text = ex.Message;
            }
        }
        //Tuple returns true if the link is good, the string returned is the valid link to the video
        private Tuple<bool, string> ValidateLink()
        {
            //Contains the normalized URL
            string normalUrl;

            //Checks that URL entered corresponds to a valid Youtube video
            if (!(DownloadUrlResolver.TryNormalizeYoutubeUrl(txtLink.Text, out normalUrl)))
            {
                Label2.Text = "Please enter a valid Youtube link";
                return Tuple.Create(false, "");
            }
            else
                return Tuple.Create(true, normalUrl);
        }

        private void DisplayLinks(string validatedLink)
        {
            //Create new videoDownloader object
            YoutubeVideoModel videoDownloader = new YoutubeVideoModel();

            //Set videoDownloader properties
            videoDownloader.Link = validatedLink;

            //Get list of Video objects
            videoDownloader.VideoInfo = FileDownloader.GetVideoInfos(videoDownloader);

            //Get Youtube ID
            int length = videoDownloader.Link.Length;
            string ID = videoDownloader.Link.Substring(length - 11, 11);

            //Get Youtube Thumbnail
            //http://stackoverflow.com/questions/2068344/how-do-i-get-a-youtube-video-thumbnail-from-the-youtube-api
            string thumbnailURL = "http://img.youtube.com/vi/" + ID + "/1.jpg";

            Label2.Text = "Right Click and Save As to Download<br/>";
            Label2.Text += "<img src='" + thumbnailURL + "'>";
            Label2.Text += videoDownloader.VideoInfo.First().Title + "<br/>";

            ProcessLinks(videoDownloader.VideoInfo);
        }

        private void ProcessLinks(IEnumerable<VideoInfo> vids)
        {
            //Sort videos by extension and resolution
            var sortedList = vids.OrderBy(vid => vid.VideoExtension)
                .ThenByDescending(vid => vid.Resolution).ToList();

            //Add sorted links to label2
            foreach (VideoInfo vi in sortedList)
            {
                if (vi.VideoExtension == ".mp4")
                {
                    if (vi.AudioExtension == null)
                        Label2.Text += "<a href=" + vi.DownloadUrl + ">>> Download MP4 <<<a> - " + vi.Resolution + "p No audio<br/>";
                    else if (vi.Resolution > 0)
                        Label2.Text += "<div id='reco'><a href=" + vi.DownloadUrl + ">>> Download MP4 <<<a> - " + vi.Resolution + "p " + vi.AudioExtension + " Recommended</div>";
                    else
                        Label2.Text += "<a href=" + vi.DownloadUrl + ">>> Download MP4 <<<a> - " + vi.Resolution + "p " + vi.AudioExtension + "<br/>";
                }
                else if (vi.VideoExtension == ".webm")
                {
                    if (vi.AudioExtension == null)
                        Label2.Text += "<a href=" + vi.DownloadUrl + ">>> Download WEBM <<<a> - " + vi.Resolution + "p No Audio<br/>";
                    else
                        Label2.Text += "<a href=" + vi.DownloadUrl + ">>> Download WEBM <<<a> - " + vi.Resolution + "p " + vi.AudioExtension + "<br/>";
                }
                else if (vi.VideoExtension == ".flv")
                {
                    if (vi.AudioExtension == null)
                        Label2.Text += "<a href=" + vi.DownloadUrl + ">>> Download FLV <<<a> - " + vi.Resolution + "p No Audio<br/>";
                    else
                        Label2.Text += "<a href=" + vi.DownloadUrl + ">>> Download FLV <<<a> - " + vi.Resolution + "p " + vi.AudioExtension + "<br/>";
                }
                else if (vi.VideoExtension == ".3gp")
                {
                    if (vi.AudioExtension == null)
                        Label2.Text += "<a href=" + vi.DownloadUrl + ">>> Download 3GP <<<a> - " + vi.Resolution + "p No Audio<br/>";
                    else
                        Label2.Text += "<a href=" + vi.DownloadUrl + ">>> Download 3GP <<<a> - " + vi.Resolution + "p " + vi.AudioExtension + "<br/>";
                }
            }
        }
    }
}