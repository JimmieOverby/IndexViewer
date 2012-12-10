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
using System.IO;
using System.Collections;

namespace IndexViewer
{
    using Lucene.Net.Index;
    using Lucene.Net.Search;
    using Lucene.Net.Store;    
    
    public abstract class BaseIndex : IIndex
    {
        #region abstract methods and properties
        
        public abstract string Name { get; }

        public abstract ICollection Fields { get; }

        public abstract IndexReader CreateReader();

        public abstract IndexSearcher CreateSearcher();

        public abstract Directory CreateDirectory();

        public abstract int GetDocumentCount();

        public abstract void Rebuild();

        public abstract void Optimize();

        #endregion

        #region public methods and properties

        public IndexSearcher Searcher { get; protected set; }

        public virtual string GetDirectoryPath()
        {
            FSDirectory directory = CreateDirectory() as FSDirectory;

            if (directory != null)
            {
                return Sitecore.IO.FileUtil.MapPath(
                                Sitecore.IO.FileUtil.MakePath(directory.GetFile().DirectoryName, directory.GetFile().Name));
            }

            return String.Empty;
        }

        public virtual DateTime GetLastUpdated()
        {
            DateTime latestModified = DateTime.MinValue;

            if (GetDirectoryPath() != null)
            {
                DirectoryInfo dir = new DirectoryInfo(GetDirectoryPath());

                foreach (FileInfo file in dir.GetFiles())
                {
                    if (String.Equals(file.Extension, ".cfs", StringComparison.InvariantCultureIgnoreCase) &&
                        file.LastWriteTime.CompareTo(latestModified) > 0)
                    {
                        latestModified = file.LastWriteTime;
                    }
                }
            }

            return latestModified;
        }

        #endregion

        #region protected methods

        protected void CloseSearcher()
        {
            if (this.Searcher != null)
            {
                this.Searcher.Close();
                this.Searcher = null;
            }
        }

        #endregion protected methods
    }
}
