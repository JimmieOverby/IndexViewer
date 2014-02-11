using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using Sitecore.Search;

namespace IndexViewer
{
    public static class ExtensionMethods
    {
        private static FieldInfo _searchResultHitField = null;
        private static FieldInfo SearchResultHitField
        {
            get
            {
                if (_searchResultHitField == null)
                    _searchResultHitField = typeof(SearchResult).GetField("_hit", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                return _searchResultHitField;
            }
        }

        public static double HitScore(this SearchResult result)
        {
            var searchHit = SearchResultHitField.GetValue(result) as SearchHit;
            if (searchHit != null)
                return searchHit.Score;
            return -1;
        }
    }
}