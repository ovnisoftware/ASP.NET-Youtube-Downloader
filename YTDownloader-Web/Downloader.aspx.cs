using System;
using YoutubeDownloader;
using YoutubeExtractor;

namespace YTDownloader_Web
{
    public partial class Downloader : System.Web.UI.Page
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

            //Get video
            videoDownloader.VideoInfo = FileDownloader.GetVideoInfos(videoDownloader);
            Label2.Text = "Right Click and Save As to Download<br/>";
            foreach (VideoInfo vi in videoDownloader.VideoInfo)
            {
                Label2.Text += "<a href=" + vi.DownloadUrl + ">" + vi.Title + " " + vi.Resolution + " " + vi.VideoExtension + "</a><br/>";
            }
        }
    }
}