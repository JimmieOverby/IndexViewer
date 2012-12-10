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

namespace IndexViewer
{
    using Sitecore.Diagnostics;

    public static class ResolverFactory
    {
        public static IIndexResolver GetIndexResolver(IndexType indexType)
        {
            return GetIndexResolver(indexType, String.Empty);
        }
        
        public static IIndexResolver GetIndexResolver(IndexType indexType, string databaseName)
        {
            if (indexType == IndexType.DataIndex && 
                String.IsNullOrEmpty(databaseName))
            {
                Log.Error("Parameter 'Database name' must not be empty", typeof(ResolverFactory));
                throw new ArgumentNullException("databaseName", "Parameter 'Database name' must not be empty");
            }

            switch (indexType)
            {
                case IndexType.SearchIndex :
                    return new SearchIndexResolver();
                case IndexType.DataIndex:
                    return new DataIndexResolver(databaseName);
            }
            
            return null;
        }
    }
}
