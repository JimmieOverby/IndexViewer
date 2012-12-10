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
using System.Collections;

namespace IndexViewer
{
    using Lucene.Net.Index;
    using Lucene.Net.Search;
    using Lucene.Net.Store;
    
    public interface IIndex
    {
        #region properties

        IndexSearcher Searcher { get; }
        
        string Name { get; }
        
        ICollection Fields { get; }

        #endregion properties

        #region methods

        IndexReader CreateReader();
        
        IndexSearcher CreateSearcher();
        
        Directory CreateDirectory();
        
        int GetDocumentCount();
        
        string GetDirectoryPath();
        
        DateTime GetLastUpdated();

        void Rebuild();

        void Optimize();

        #endregion methods
    }
}
