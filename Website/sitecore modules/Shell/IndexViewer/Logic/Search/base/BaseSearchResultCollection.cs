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
    public abstract class BaseSearchResultCollection
    {
        //----------------------------------------------------------------------------------
        /// <summary>
        /// Gets the time elapsed.
        /// </summary>
        /// <value>The time elapsed.</value>
        /// <remark>Created 10/02/2009 14:33 by jm</remark>
        //----------------------------------------------------------------------------------
        public double TimeElapsed { get; protected set; }


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


        protected void AddColumns2Table(DataTable table, ICollection<String> fields)
        {
            table.Columns.Add("No", typeof(string));
            table.Columns.Add("HitScore", typeof(string));

            foreach (var fieldTitle in fields)
            {
                table.Columns.Add(fieldTitle as string, typeof(string));
            }
        }

        protected abstract void AddRows2Table(DataTable table, ICollection<String> fields);

        protected void DeleteEmptyColumns(DataTable table)
        {
            if (table == null)
            {
                return;
            }

            for (int i = table.Columns.Count - 1; i >= 0; i--)
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
    }
}
