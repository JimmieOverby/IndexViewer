using System;
using System.Collections.Generic;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Sitecore.Diagnostics;

namespace IndexViewer
{
    /// <summary>
    /// The class allows searching of different kinds in the specified index and database
    /// </summary>
    public class LuceneSearcher : BaseSearcher
    {
        public LuceneSearcher(IIndex index)
            : base()
        {
            Index = index;
        }

        #region public methods
        public LuceneSearchResultCollection FieldSearch(QueryInfo[] qis)
        {
            HighResTimer timer = new HighResTimer(true);
            var booleanQuery = new BooleanQuery();
            foreach (var qi in qis)
            {
                var q = GetQuery(qi);
                if (q != null)
                    booleanQuery.Add(q, qi.LuceneBooleanClauseOccur);
            }

            IList<Document> hits = null;

            if (booleanQuery.Clauses.Count > 0)
                hits = GetSearchHits(booleanQuery);

            return hits != null
                    ? new LuceneSearchResultCollection(hits, timer.Elapsed())
                    : null;
        }

        private Query GetQuery(QueryInfo qi)
        {
            if (String.IsNullOrEmpty(qi.SearchString) ||
                String.IsNullOrEmpty(qi.FieldName) ||
                Index == null)
            {
                return null;
            }

            Query query = null;
            try
            {

                switch (qi.QueryType)
                {
                    case "QueryParser":
                        var parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30,qi.FieldName, new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30));
                        query = parser.Parse(QueryParser.Escape(qi.SearchString));
                        break;

                    default:
                        var term = new Term(qi.FieldName, qi.SearchString);

                        switch (qi.QueryType)
                        {
                            case "TermQuery":
                                query = new TermQuery(term);
                                break;

                            case "PrefixQuery":
                                query = new PrefixQuery(term);
                                break;

                            case "WildCardQuery":
                                query = new WildcardQuery(term);
                                break;
                        }

                        break;
                }
            }
            //The query has some invalid search term, but this is ok, it just doesnt give a result
            catch { }

            return query;
        }

        private IList<Document> GetSearchHits(Query q)
        {
            if (q == null)
                return null;

            Lucene.Net.Search.IndexSearcher searcher = Index.CreateSearcher();

            var hits = searcher.Search(q, Int32.MaxValue);

            if (this.Explain > -1)
                this.Explanation = searcher.Explain(q, this.Explain);

            IList<Document> hitsToReturn = new List<Document>();
            foreach (var searchDoc in hits.ScoreDocs)
            {
                var doc = searcher.Doc(searchDoc.Doc);
                hitsToReturn.Add(doc);
            }

            return hitsToReturn;
        }

        #endregion

    }
}
