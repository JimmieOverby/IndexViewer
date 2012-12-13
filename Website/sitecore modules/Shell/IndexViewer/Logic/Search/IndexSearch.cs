using System;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Sitecore.Diagnostics;

namespace IndexViewer
{
    /// <summary>
    /// The class allows searching of different kinds in the specified index and database
    /// </summary>
    public class IndexSearch
    {
        #region constructor
        //----------------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the <see cref="IndexSearch"/> class.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="database">The database.</param>
        /// <remark>Created 07/01/2009 14:20 by jm</remark>
        //----------------------------------------------------------------------------------
        public IndexSearch(IIndex index)
        {
            Index = index;
        }

        #endregion

        #region properties
        
        //----------------------------------------------------------------------------------
        /// <summary>
        /// Gets or sets the index.
        /// </summary>
        /// <value>The index.</value>
        /// <remark>Created 11/02/2009 21:37 by jm</remark>
        //----------------------------------------------------------------------------------
        private IIndex Index { get; set; }

        #endregion

        #region public methods
        //----------------------------------------------------------------------------------
        /// <summary>
        /// Search the index for a document where the field has the searchstring
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="searchString">The search string.</param>
        /// <param name="queryType">Type of the query.</param>
        /// <returns></returns>
        /// <remark>Created 07/01/2009 14:19 by jm</remark>
        //----------------------------------------------------------------------------------
        public SearchResultCollection FieldSearch(string fieldName, string searchString, string queryType)
        {
            if (String.IsNullOrEmpty(searchString) ||
                String.IsNullOrEmpty(fieldName) ||
                Index == null)
            {
                return null;
            }

            HighResTimer timer = new HighResTimer(true);
            Hits hits = GetSearchHits(fieldName, searchString, queryType);

            return hits != null
                    ? new SearchResultCollection(hits, timer.Elapsed())
                    : null;
        }

        private Hits GetSearchHits(string fieldName, string searchString, string queryType)
        {
            Hits hits = null;
            Lucene.Net.Search.IndexSearcher searcher = Index.CreateSearcher();

            try
            {
                switch (queryType)
                {
                    case "QueryParser":
                        hits = QueryParserSearch(searcher, fieldName, searchString);
                        break;
                    case "TermQuery":
                        hits = TermSearch(searcher, fieldName, searchString);
                        break;
                    case "PrefixQuery":
                        hits = PrefixSearch(searcher, fieldName, searchString);
                        break;
                    case "WildCardQuery":
                        hits = WildCardSearch(searcher, fieldName, searchString);
                        break;
                }
            }
            //The query has some invalid search term, but this is ok, it just doesnt give a result
            catch { }

            return hits;
        }

        //----------------------------------------------------------------------------------
        /// <summary>
        /// Performs a QueryParser Search
        /// </summary>
        /// <param name="searcher">The searcher.</param>
        /// <param name="field">The field.</param>
        /// <param name="searchTerm">The search term.</param>
        /// <returns></returns>
        /// <remark>Created 10/02/2009 17:29 by jm</remark>
        //----------------------------------------------------------------------------------
        private Hits QueryParserSearch(IndexSearcher searcher, string field, string searchTerm)
        {
            QueryParser parser = new QueryParser(field, new StandardAnalyzer());
            Query query = parser.Parse(QueryParser.Escape(searchTerm));

            return searcher.Search(query);
        }

        //----------------------------------------------------------------------------------
        /// <summary>
        /// Performs a prefixsearch
        /// </summary>
        /// <param name="searcher">The searcher.</param>
        /// <param name="field">The field.</param>
        /// <param name="searchTerm">The search term.</param>
        /// <returns></returns>
        /// <remark>Created 10/02/2009 17:22 by jm</remark>
        //----------------------------------------------------------------------------------
        private Hits PrefixSearch(IndexSearcher searcher, string field, string searchTerm)
        {
            Term term = new Term(field, searchTerm);
            Query query = new PrefixQuery(term);

            return searcher.Search(query);
        }

        //----------------------------------------------------------------------------------
        /// <summary>
        /// Performs a wildcard search
        /// </summary>
        /// <param name="searcher">The searcher.</param>
        /// <param name="field">The field.</param>
        /// <param name="searchTerm">The search term.</param>
        /// <returns></returns>
        /// <remark>Created 10/02/2009 17:24 by jm</remark>
        //----------------------------------------------------------------------------------
        private Hits WildCardSearch(IndexSearcher searcher, string field, string searchTerm)
        {
            Term term = new Term(field, searchTerm);
            Query query = new WildcardQuery(term);

            return searcher.Search(query);
        }

        //----------------------------------------------------------------------------------
        /// <summary>
        /// performs a term search
        /// </summary>
        /// <param name="searcher">The searcher.</param>
        /// <param name="field">The field.</param>
        /// <param name="searchTerm">The search term.</param>
        /// <returns></returns>
        /// <remark>Created 10/02/2009 17:26 by jm</remark>
        //----------------------------------------------------------------------------------
        private Hits TermSearch(IndexSearcher searcher, string field, string searchTerm)
        {
            Term term = new Term(field, searchTerm);
            Query query = new TermQuery(term);

            return searcher.Search(query);
        }

        #endregion

    }
}
