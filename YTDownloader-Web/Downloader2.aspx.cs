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
    public partial class Downloader2 : System.Web.UI.Page
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

            foreach (VideoInfo vi in videoDownloader.VideoInfo)
            {
                Label2.Text += "<a href=" + vi.DownloadUrl + ">" + vi.Resolution + " " + vi.VideoExtension + "</a><br/>";
            }
        }
    }
}