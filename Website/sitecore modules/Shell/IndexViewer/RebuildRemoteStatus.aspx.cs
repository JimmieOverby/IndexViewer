using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace IndexViewer.sitecore_modules.Shell.IndexViewer
{
    public partial class RebuildRemoteStatus : System.Web.UI.Page
    {
        private string _url;
        protected void Page_Load(object sender, EventArgs e)
        {
            string url = Server.UrlDecode(Request.QueryString["jobUrl"]);
            if (string.IsNullOrEmpty(url))
                throw new InvalidOperationException("Cannot find jobUrl");
            SetRunningLabel(url);
        }

        private void SetRunningLabel(string url)
        {
            WebClient webClient = new WebClient();
            XDocument indexRebuildResult;
            try
            {
                string status = webClient.DownloadString(url);
                indexRebuildResult = XDocument.Parse(status);
            }
            catch (Exception ex)
            {
                Response.Write("Something went wrong. Sorry for this hopeless error");
                return;
            }
            XElement resultElement = indexRebuildResult.Descendants("rebuild").FirstOrDefault();
            string currentStatus = resultElement.Attribute("status").Value;
            if (currentStatus.ToLower() == "running")
            {
                RunningLabel.Text = currentStatus;
                string javascript = GetJavascript(Request.RawUrl);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "UpdateScript", javascript, true);
            }
            else
            {
                LoadingPanel.Visible = false;
                DonePanel.Visible = true;
                DoneLabel.Text = currentStatus;
            }
        }

        private string GetJavascript(string url)
        {
            return @"function update(){setTimeout(function(){location.href = '" + _url + @"';} , 5000);}update()";
        }
    }
}