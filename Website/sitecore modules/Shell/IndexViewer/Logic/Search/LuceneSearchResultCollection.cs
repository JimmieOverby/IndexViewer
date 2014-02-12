using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Lucene.Net.Documents;
using Lucene.Net.Search;

namespace IndexViewer
{
    /// <summary>
    /// Collection of results. 
    /// </summary>
    public class LuceneSearchResultCollection : BaseSearchResultCollection
    {
        public LuceneSearchResultCollection(IList<Document> hits, double timeElapsed)
        {
            SearchResultHits = hits;
            TimeElapsed = timeElapsed;
        }

        public IList<Document> SearchResultHits { get; private set; }


        protected override void AddRows2Table(DataTable table, ICollection<String> fields)
        {
            for (int i = 0; i < SearchResultHits.Count; i++)
            {
                List<string> values = new List<string> { i.ToString() };
                foreach (var fieldTitle in fields)
                {
                    Field field = SearchResultHits[i].GetField(fieldTitle);
                    values.Add(field != null
                                    ? field.StringValue
                                    : String.Empty);
                }

                table.Rows.Add(values.ToArray());
            }
        }

    }
}
