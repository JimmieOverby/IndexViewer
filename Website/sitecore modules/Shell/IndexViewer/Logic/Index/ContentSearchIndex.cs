using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.LuceneProvider;
using Sitecore.Diagnostics;

namespace IndexViewer.sitecore_modules.Shell.IndexViewer.Logic.Index
{
    public class ContentSearchIndex : IIndex
    {
        private ISearchIndex _searchIndex;

        public ContentSearchIndex(ISearchIndex index)
        {
            Assert.ArgumentNotNull(index, "index");
            _searchIndex = index;
        }

        public IndexReader Reader
        {
            get
            {
                return CreateReader();
            }

        }

        public string Name
        {
            get 
            {
                return _searchIndex.Name;
            }
        }

        public ICollection<string> Fields
        {
            get 
            {
                return _searchIndex.Schema.AllFieldNames;
            }
        }

        private Lucene.Net.Index.IndexReader CreateReader()
        {
            return DirectoryReader.Open(((LuceneIndex)_searchIndex).Directory, true);
        }

        public Lucene.Net.Search.IndexSearcher CreateSearcher()
        {
            if (Searcher != null)
                return Searcher;

            IndexReader reader = CreateReader();  

            this.Searcher = new IndexSearcher(reader);
            return this.Searcher;
        }

        public int GetDocumentCount()
        {
            int number;
            if(!int.TryParse(_searchIndex.Summary.NumberOfDocuments.ToString(), out number))
            {
                return -1;
            }
            return number;

        }

        public string GetDirectoryPath()
        {
            Directory dir = Reader.Directory();
            return dir.ToString();

        }

        public DateTime GetLastUpdated()
        {
            return _searchIndex.Summary.LastUpdated;
        }

        public void Rebuild()
        {
            this.CloseSearcher();
            _searchIndex.Rebuild();
        }

        private void CloseSearcher()
        {
            if (this.Searcher != null)
            {
                this.Searcher.Dispose();
                this.Searcher = null;
            }
        }

        public void Optimize()
        {
            throw new InvalidOperationException("Cannot optimize Content Search indexes");
        }

        public IndexSearcher Searcher
        {
            get;
            protected set;
        }
    }
}