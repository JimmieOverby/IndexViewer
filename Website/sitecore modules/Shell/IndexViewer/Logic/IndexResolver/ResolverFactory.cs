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
            
            switch (indexType)
            {
                case IndexType.SearchIndex :
                    return new SearchIndexResolver();
                case IndexType.ContentSearch:
                    return new ContentSearchResolver();
            }
            
            return null;
        }
    }
}
