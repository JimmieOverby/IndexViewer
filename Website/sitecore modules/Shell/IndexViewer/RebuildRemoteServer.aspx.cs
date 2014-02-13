using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sitecore.Data.Items;
using Sitecore.Data;
using System.Net;
using System.Xml.Linq;
using Sitecore.Exceptions;
using Sitecore.Data.Fields;

namespace IndexViewer.sitecore_modules.Shell.IndexViewer
{
    public partial class RebuildRemoteServerCommand : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsRemoteRebuildEnabled())
            {
                NotAllowedArea.Visible = true;
                SelectRemoteIndexArea.Visible = false;
                return;
            }
            if (!Page.IsPostBack)
                FillServerList();
        }

        private bool IsRemoteRebuildEnabled()
        {
            Item settingsItem = Database.GetDatabase("master").GetItem(new ID(Constants.ItemIds.SettingsItemId));
            if (settingsItem == null)
                throw new InvalidOperationException("IndexViewer not configured correcty. Cannot find settings items");
            CheckboxField enableRemoteRebuild = settingsItem.Fields[Constants.FieldNames.EnableRemoteRebuild];
            if (enableRemoteRebuild == null)
                return false;
            if (!enableRemoteRebuild.Checked)
                return false;

            return true;
        }

        protected void FillServerList()
        {
            ServerDropDown.Items.Add("Choose server...");
            Item serverRoot = Database.GetDatabase("master").GetItem(new ID(Constants.ItemIds.RemoteServerRoot));
            if (serverRoot == null)
            {
                throw new ConfigurationException("No server root found on your system");
            }
            foreach (Item item in serverRoot.Children)
            {
                ServerDropDown.Items.Add(new ListItem(item.Name, item.ID.ToString()));
            }
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Write("<script language='javascript'>window.top.dialogClose();</script>");
            }
            catch (Exception ex)
            {
                ErrorResolver.ResolveError(ex, this);
            }
        }

        protected void ServerDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(ServerDropDown.SelectedValue))
            {
                Item serverItem = Database.GetDatabase("master").GetItem(new ID(ServerDropDown.SelectedValue));
                string address = serverItem[Constants.FieldNames.ServerAddress];
                if (String.IsNullOrEmpty(address))
                {
                    Response.Write("<script language='javascript'>alert('No address entered for server: " + ServerDropDown.SelectedItem.Text + "');</script>");
                    IndexDropDown.Enabled = false;
                    return;
                }
                FillIndexList();
            }
        }

        private string GetAddress()
        {
            Item serverItem = Database.GetDatabase("master").GetItem(new ID(ServerDropDown.SelectedValue));
            string address = serverItem[Constants.FieldNames.ServerAddress];
            if (!address.StartsWith("http"))
                address = "http://" + address;
            return address;
        }

        private void FillIndexList()
        {
            string address = GetAddress();
            string pathToService = "/sitecore/system/Modules/IndexViewer/Service/AvailableIndexes";
            Item settingsItem = Database.GetDatabase("master").GetItem(new ID(Constants.ItemIds.SettingsItemId));
            if (settingsItem == null)
                throw new InvalidOperationException("Could not find settings item");
            string securityToken = settingsItem[Constants.FieldNames.SecurityToken];
            string fullAddress = address + pathToService + "?securityToken=" + securityToken;
            WebClient webClient = new WebClient();
            XDocument indexList;
            try
            {
                string xmlResponse = webClient.DownloadString(fullAddress);
                indexList = XDocument.Parse(xmlResponse);
            }
            catch (Exception ex)
            {
                //Could handle this nicer
                ErrorResolver.ResolveError(ex, this);
                return;

            }

            IndexDropDown.Items.Clear();
            IndexDropDown.Enabled = true;

            IEnumerable<XElement> indexElements = indexList.Descendants("index");
            if (indexElements == null || indexElements.Count() == 0)
                IndexDropDown.Items.Add("No index...");
            else
                IndexDropDown.Items.Add("Choose index...");

            foreach (XElement indexElement in indexElements)
            {
                string indexName = indexElement.Attribute("name").Value;
                IndexDropDown.Items.Add(new ListItem(indexName, indexName));
            }
        }

        protected void IndexDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            RebuildButton.Enabled = !String.IsNullOrEmpty(IndexDropDown.SelectedValue);
        }

        protected void RebuildButton_Click(object sender, EventArgs e)
        {
            string address = GetAddress();
            string pathToService = "/sitecore/system/Modules/IndexViewer/Service/rebuildindex";
            Item settingsItem = Database.GetDatabase("master").GetItem(new ID(Constants.ItemIds.SettingsItemId));
            if (settingsItem == null)
                throw new InvalidOperationException("Settings item could not be found");
            string securityToken = settingsItem[Constants.FieldNames.SecurityToken];
            string indexName = IndexDropDown.SelectedValue;
            string fullAddress = address + pathToService + "?indexName=" + indexName + @"&method=rebuildindex&SecurityToken=" + securityToken;
            WebClient webClient = new WebClient();
            XDocument indexRebuildResult;
            try
            {
                string xmlResponse = webClient.DownloadString(fullAddress);
                indexRebuildResult = XDocument.Parse(xmlResponse);
            }
            catch (Exception ex)
            {
                ErrorResolver.ResolveError(ex, this);
                return;
            }
            XElement resultElement = indexRebuildResult.Descendants("rebuild").FirstOrDefault();
            if (resultElement == null)
            {
                //TODO: Do something nicer
                Response.Write("Something went wrong. I am truly sorry. However the truth is probably, that you didn't configure the module correctly");
                return;
            }
            string jobId = resultElement.Attribute("jobId").Value;
            string url = address + pathToService + @"?method=status&jobName=" + jobId + @"&SecurityToken=" + securityToken;
            SessionManager.Instance.CurrentRebuildingJobUrl = Server.UrlEncode(url);
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "Close", "window.top.dialogClose();", true);

        }
    }
}