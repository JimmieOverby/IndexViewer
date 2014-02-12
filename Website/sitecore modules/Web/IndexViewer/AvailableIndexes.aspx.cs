using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace IndexViewer.sitecore_modules.Web.IndexViewer
{
    public partial class AvailableIndexes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!RequestIsValid())
                throw new InvalidOperationException("No good securityToken or remote rebuild not allowed");
            FillIndexes();
        }

        private bool RequestIsValid()
        {
            return RequestValidator.IsRequestValid(Request.QueryString["SecurityToken"]);
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