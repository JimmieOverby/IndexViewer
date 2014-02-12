using System;
using Lucene.Net.Index;
using Sitecore.Diagnostics;
using Sitecore.Search;

namespace IndexViewer
{
    /// <summary>
    /// The class allows searching of different kinds in the specified index and database
    /// </summary>
    public class SitecoreSearcher : BaseSearcher
    {

        public SitecoreSearcher(IIndex index)
            : base()
        {
            Index = index;
        }


        #region public methods

        public SitecoreSearchResultCollection FieldSearch(QueryInfo[] qis, int maxResults)
        {
            HighResTimer timer = new HighResTimer(true);
            var combinedQuery = new CombinedQuery();
            foreach (var qi in qis)
            {
                var q = GetQuery(qi);
                if (q != null)
                    combinedQuery.Add(q, qi.SitecoreSearchQueryOccurance);
            }

            SearchResultCollection results = null;

            if (combinedQuery.Clauses.Count > 0)
                results = GetSearchHits(combinedQuery, maxResults);

            return results != null
                    ? new SitecoreSearchResultCollection(results, timer.Elapsed())
                    : null;
        }

        private QueryBase GetQuery(QueryInfo qi)
        {
            if (String.IsNullOrEmpty(qi.SearchString) ||
                String.IsNullOrEmpty(qi.FieldName) ||
                Index == null)
            {
                return null;
            }

            QueryBase query = null;

            switch (qi.QueryType)
            {
                case "FieldQuery":
                    query = new FieldQuery(qi.FieldName, qi.SearchString);
                    break;

                default:
                    query = new FullTextQuery(qi.SearchString);
                    break;
            }

            return query;
        }

        private SearchResultCollection GetSearchHits(QueryBase q, int maxResults)
        {
            if (q == null)
                return null;

            var index = SearchManager.GetIndex(this.Index.Name);
            using (IndexSearchContext context = index.CreateSearchContext())
            {
                var preparedQuery = context.Prepare(q);
                var hits = context.Search(preparedQuery, maxResults);

                if (this.Explain > -1)
                    this.Explanation = context.Explain(preparedQuery, this.Explain);

                return hits.FetchResults(0, Math.Min(hits.Length, maxResults));
            }

        }

        #endregion

    }
}
