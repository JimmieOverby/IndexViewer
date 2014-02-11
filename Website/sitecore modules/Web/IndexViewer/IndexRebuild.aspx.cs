using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Jobs;

namespace IndexViewer.sitecore_modules.Web.IndexViewer
{
    public partial class IndexRebuild : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!RequestIsValid())
                throw new InvalidOperationException("No good securityToken");

            //Semi-routing
            string methodName = Request.QueryString["method"];
            if (String.IsNullOrEmpty(methodName))
                throw new InvalidOperationException("No method name");

            if (methodName.ToLower() == "rebuildindex")
            {
                string indexName = Request.QueryString["indexName"];
                if (String.IsNullOrEmpty(indexName))
                    //TODO: Return error in XML
                    throw new NotImplementedException("no index name specified");
                string jobName = RebuildIndex(indexName);
                IndexRebuildPlaceholder.Controls.Add(new LiteralControl("<rebuild status='started' jobId='" + jobName + "'/>"));
            }
            else if (methodName.ToLower() == "status")
            {
                string jobName = Request.QueryString["jobName"];
                jobName = Server.UrlDecode(jobName);
                string jobStatus = GetJobStatus(jobName);
                IndexRebuildPlaceholder.Controls.Add(new LiteralControl("<rebuild status='" + jobStatus + "' jobId='" + jobName + "'/>"));
            }
        }

        private bool RequestIsValid()
        {
            Item settingsItem = Sitecore.Context.Database.GetItem(new ID(Constants.ItemIds.SettingsItemId));
            if(settingsItem == null)
                throw new InvalidOperationException("Cannot find settings item");
            string enteredToken = settingsItem[Constants.FieldNames.SecurityToken];
            string tokenSent = Request.QueryString["SecurityToken"];
            return (enteredToken == tokenSent);
        }

        private string GetJobStatus(string jobName)
        {
            Job job = JobManager.GetJob(jobName);
            if (job == null)
                return "NotRunning";
            if (job.Status.Failed)
                return "Failed";
            if (job.IsDone)
                return "Done";
            if (job.Status.State == JobState.Queued)
                return "Queued";
            if (job.Status.State == JobState.Running)
                return "Running";
            return "Unknown";
        }

        private string RebuildIndex(string indexName)
        {
            SearchIndexResolver resolver = new SearchIndexResolver();
            IIndex index = resolver.GetIndex(indexName);
            if (index == null)
            {
                ContentSearchResolver contentSearchResolver = new ContentSearchResolver();
                index = contentSearchResolver.GetIndex(indexName);
                if(index == null)
                    throw new InvalidOperationException("unknown index");
            }

            string jobId = Guid.NewGuid().ToString();

            RebuildIndexJob indexJob = new RebuildIndexJob(index);
            JobOptions option = new JobOptions(jobId, "Index rebuild", Sitecore.Context.Site.Name, indexJob, "Start") { AfterLife = TimeSpan.FromHours(1) };
            JobManager.Start(option);
            return jobId;
        }

        public class RebuildIndexJob
        {
            private IIndex _index;
            public RebuildIndexJob(IIndex index)
            {
                Assert.ArgumentNotNull(index, "indexName");
                _index = index;
            }

            public void Start()
            {
                _index.Rebuild();
            }
        }
    }
}