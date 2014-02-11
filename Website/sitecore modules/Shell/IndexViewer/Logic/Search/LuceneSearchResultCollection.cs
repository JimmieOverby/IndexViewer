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
        public LuceneSearchResultCollection(Hits hits, double timeElapsed)
        {
            SearchResultHits = hits;
            TimeElapsed = timeElapsed;
        }

        public Hits SearchResultHits { get; private set; }


        protected override void AddRows2Table(DataTable table, ICollection fields)
        {
            for (int i = 0; i < SearchResultHits.Length(); i++)
            {
                List<string> values = new List<string> { i.ToString(), SearchResultHits.Score(i).ToString() };

                foreach (var fieldTitle in fields)
                {
                    Field field = SearchResultHits.Doc(i).GetField(fieldTitle as string);

                    values.Add(field != null
                                    ? field.StringValue()
                                    : String.Empty);
                }

                table.Rows.Add(values.ToArray());
            }
        }

    }
}
