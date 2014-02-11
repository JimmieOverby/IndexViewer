using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;

namespace IndexViewer.Logic.DynamicLinq
{
    public class LinqSearchResult<T>
    {
        private IEnumerable<T> _results;

        public LinqSearchResult(IEnumerable<T> results)
        {
            _results = results;
        }

        public DataTable AsDataTable()
        {
            DataTable table = new DataTable();

            AddColumnsToTable(table);
            AddRowsToTable(table);

            return table;
        }

        private void AddRowsToTable(DataTable table)
        {
            for (int i = 0; i < _results.Count(); i++)
            {
                List<string> values = new List<string> { i.ToString() };
                foreach (PropertyInfo info in Properties)
                {
                    T resultItem = _results.ElementAt(i);
                    object resultValue = typeof(T).GetProperty(info.Name).GetValue(resultItem);
                    if (resultValue == null)
                        values.Add("NULL");
                    else if (resultValue is string)
                        values.Add(resultValue as string);
                    else
                        values.Add(resultValue.ToString());
                }
                table.Rows.Add(values.ToArray());
            }
        }

        private void AddColumnsToTable(DataTable table)
        {
            table.Columns.Add("No", typeof(string));
            foreach (PropertyInfo info in Properties)
            {
                table.Columns.Add(info.Name, typeof(string));
            }
            
        }

        private IEnumerable<PropertyInfo> Properties
        {
            get
            {
                Type t = typeof(T);
                return GetPropertiesOnType(t);
            }
        }

        private IEnumerable<PropertyInfo> GetPropertiesOnType(Type t)
        {
            return t.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.GetIndexParameters().Length == 0);
        }
    }
}