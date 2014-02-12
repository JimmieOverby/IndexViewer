using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Lucene.Net.Documents;
using Sitecore.Search;

namespace IndexViewer
{
    /// <summary>
    /// Collection of results. 
    /// </summary>
    public class SitecoreSearchResultCollection : BaseSearchResultCollection
    {
        public SitecoreSearchResultCollection(SearchResultCollection results, double timeElapsed)
        {
            SearchResultCollection = results;
            TimeElapsed = timeElapsed;
        }

        public SearchResultCollection SearchResultCollection { get; private set; }

        protected override void AddRows2Table(DataTable table, ICollection<string> fields)
        {
            int i = 0;
            foreach (var result in SearchResultCollection)
            {
                var values = new List<string> { i.ToString(), result.HitScore().ToString() };

                foreach (var fieldTitle in fields)
                {
                    Field field = result.Document.GetField(fieldTitle as string);

                    values.Add(field != null
                                    ? field.StringValue
                                    : String.Empty);
                }

                table.Rows.Add(values.ToArray());

                i++;
            }
        }

    }
}
