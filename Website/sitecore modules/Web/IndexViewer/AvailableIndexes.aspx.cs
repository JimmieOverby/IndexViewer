using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace IndexViewer.sitecore_modules.Web.IndexViewer
{
    public partial class AvailableIndexes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!RequestIsValid())
                throw new InvalidOperationException("No good securityToken");
            FillIndexes();
        }

        private bool RequestIsValid()
        {
            Item settingsItem = Sitecore.Context.Database.GetItem(new ID(Constants.ItemIds.SettingsItemId));
            if (settingsItem == null)
                throw new InvalidOperationException("IndexViewer not configured correcty. Cannot find settings items");
            string enteredToken = settingsItem[Constants.FieldNames.SecurityToken];
            string tokenSent = Request.QueryString["SecurityToken"];
            return (enteredToken == tokenSent);
        }

        private void FillIndexes()
        {
            IEnumerable<string> indexNames = GetIndexNames();
            foreach (string indexName in indexNames)
            {
                IndexPlaceholder.Controls.Add(new LiteralControl(@"<index name='" + indexName + "'/>"));
            }
        }


        private IEnumerable<string> GetIndexNames()
        {
            IIndexResolver resolver = ResolverFactory.GetIndexResolver(IndexType.SearchIndex);
            List<String> normalIndexes = resolver.GetIndexNames();
            IIndexResolver contentSearchResolver = ResolverFactory.GetIndexResolver(IndexType.ContentSearch);
            List<String> contentSearchIndexes = contentSearchResolver.GetIndexNames();
            normalIndexes.AddRange(contentSearchIndexes);
            return normalIndexes;
        }
    }
}