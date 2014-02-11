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
    public class SearchResultCollection
    {
        #region constructors

        //----------------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchResultCollection"/> class.
        /// </summary>
        /// <param name="hits">The hits.</param>
        /// <param name="timeElapsed">The time elapsed.</param>
        /// <remark>Created 02/02/2009 15:15 by jm</remark>
        //----------------------------------------------------------------------------------
        public SearchResultCollection(IList<Document> hits, double timeElapsed)
        {
            SearchResultHits    = hits;
            TimeElapsed         = timeElapsed;
        }

        #endregion constructors

        #region properties

        //----------------------------------------------------------------------------------
        /// <summary>
        /// Gets or sets the search result hits.
        /// </summary>
        /// <value>The search result hits.</value>
        /// <remark>Created 02/02/2009 15:15 by jm</remark>
        //----------------------------------------------------------------------------------
        public IList<Document> SearchResultHits { get; private set; }

        //----------------------------------------------------------------------------------
        /// <summary>
        /// Gets the time elapsed.
        /// </summary>
        /// <value>The time elapsed.</value>
        /// <remark>Created 10/02/2009 14:33 by jm</remark>
        //----------------------------------------------------------------------------------
        public double TimeElapsed { get; private set; }

        #endregion

        #region public methods

        //----------------------------------------------------------------------------------
        /// <summary>
        /// Converts the SearchResult to a datatable
        /// </summary>
        /// <returns></returns>
        /// <remark>Created 02/02/2009 15:19 by jm</remark>
        //----------------------------------------------------------------------------------
        public DataTable AsDataTable(ICollection<String> fields)
        {
            return AsDataTable(fields, false);
        }
        
        public DataTable AsDataTable(ICollection<String> fields, bool excludeEmpty)
        {
            DataTable table = new DataTable();
            
            if (fields != null)
            {
                AddColumns2Table(table, fields);
                AddRows2Table(table, fields);

                if (excludeEmpty)
                {
                    DeleteEmptyColumns(table);
                }
            }

            return table;
        }


        private void AddColumns2Table(DataTable table, ICollection<String> fields)
        {
            table.Columns.Add("No", typeof(string));

            foreach (var fieldTitle in fields)
            {
                table.Columns.Add(fieldTitle, typeof(string));
            }
        }

        private void AddRows2Table(DataTable table, ICollection<String> fields)
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


        private void DeleteEmptyColumns(DataTable table)
        {
            if (table == null)
            {
                return;
            }

            for (int i = table.Columns.Count-1; i >= 0; i--)
            {
                DataColumn column = table.Columns[i];
                
                bool isExistsValue = false;

                foreach (DataRow row in table.Rows)
                {
                    if (!String.IsNullOrEmpty(row[column] as string))
                    {
                        isExistsValue = true;
                        continue;
                    }
                }

                if (!isExistsValue &&
                    table.Columns.CanRemove(column))
                {
                    table.Columns.Remove(column);
                }
            }

            table.AcceptChanges();
        }


        #endregion public methods
    }
}
