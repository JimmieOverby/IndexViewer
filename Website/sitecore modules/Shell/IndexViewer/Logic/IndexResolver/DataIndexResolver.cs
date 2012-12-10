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
    using Sitecore;
    using Sitecore.Diagnostics;
    using Sitecore.Data;
    using Sitecore.Configuration;
    
    public class DataIndexResolver : IIndexResolver
    {
        #region fields

        private readonly string _databaseName;

        #endregion fields

        #region constructors

        public DataIndexResolver(string databaseName)
        {
            Assert.ArgumentNotNullOrEmpty(databaseName, "databaseName");

            _databaseName = databaseName;
        }

        #endregion constructors

        #region public methods

        public virtual List<string> GetIndexNames()
        {
            Database database = Factory.GetDatabase(_databaseName);

            if (database != null)
            {
                List<string> names = new List<string>();

                for (int i = 0; i < database.Indexes.Count; i++)
                {
                    names.Add(database.Indexes[i].Name);
                }

                return names;
            }

            return null;
        }
        
        public virtual IIndex GetIndex(string indexName)
        {
            Assert.ArgumentNotNullOrEmpty(indexName, "indexName");

            Database database = Factory.GetDatabase(_databaseName);
            
            if (database != null &&
                database.Indexes[indexName] != null)
            {
                return new DataIndex(database.Indexes[indexName], _databaseName);
            }
             
            return null;
        }

        #endregion public methods

    }
}
