using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace IndexViewer
{
    using Sitecore.Search;
    using Sitecore.Diagnostics;
    
    public class SearchIndexResolver : IIndexResolver
    {
        #region public methods

        public virtual List<string> GetIndexNames()
        {
            if (SearchManager.IndexCount > 0)
            {
                Type searchManagerType = typeof(SearchManager);

                SearchConfiguration configuration = searchManagerType.InvokeMember("_configuration",
                                    BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField,
                                    null,
                                    null,
                                    null) as SearchConfiguration;

                if (configuration != null &&
                    configuration.Indexes.Count > 0)
                {
                    return configuration.Indexes.Keys.ToList<string>();
                }
            }

            return null;
        }

        public virtual IIndex GetIndex(string indexName)
        {
            Assert.ArgumentNotNullOrEmpty(indexName, "indexName");

            Index index = SearchManager.GetIndex(indexName);

            if (index != null)
            {
                return new SearchIndex(index);
            }
            
            return null;
        }

        #endregion public methods
    }
}
