using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IndexViewer
{
    public class QueryInfo
    {
        public string FieldName { get; set; }
        public string SearchString { get; set; }
        public string QueryType { get; set; }
        public string QueryOccurance { get; set; }

        public Sitecore.Search.QueryOccurance SitecoreSearchQueryOccurance
        {
            get
            {
                switch (this.QueryOccurance)
                {
                    case "Must":
                        return Sitecore.Search.QueryOccurance.Must;
                    case "MustNot":
                        return Sitecore.Search.QueryOccurance.MustNot;
                    default:
                        return Sitecore.Search.QueryOccurance.Should;
                }
            }
        }

        public Lucene.Net.Search.BooleanClause.Occur LuceneBooleanClauseOccur
        {
            get
            {
                switch (this.QueryOccurance)
                {
                    case "Must":
                        return Lucene.Net.Search.BooleanClause.Occur.MUST;
                    case "MustNot":
                        return Lucene.Net.Search.BooleanClause.Occur.MUST_NOT;
                    default:
                        return Lucene.Net.Search.BooleanClause.Occur.SHOULD;
                }
            }
        }
    }
}