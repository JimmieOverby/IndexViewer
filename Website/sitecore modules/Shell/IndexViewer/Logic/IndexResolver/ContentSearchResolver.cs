using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IndexViewer.sitecore_modules.Shell.IndexViewer.Logic.Index;
using Sitecore.ContentSearch;
using Sitecore.Diagnostics;

namespace IndexViewer
{
    public class ContentSearchResolver : IIndexResolver
    {
        public List<string> GetIndexNames()
        {
            return Sitecore.ContentSearch.ContentSearchManager.Indexes.Select(i => i.Name).ToList();
        }

        public IIndex GetIndex(string indexName)
        {
            Assert.ArgumentNotNullOrEmpty(indexName, "indexName");

            ISearchIndex searchIndex = Sitecore.ContentSearch.ContentSearchManager.Indexes.FirstOrDefault(i => i.Name == indexName);
            if (searchIndex != null)
            {
                return new ContentSearchIndex(searchIndex);
            }

            return null;

        }
    }
}