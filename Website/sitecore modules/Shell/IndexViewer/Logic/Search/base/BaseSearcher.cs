using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lucene.Net.Search;

namespace IndexViewer
{
    public abstract class BaseSearcher
    {
        public BaseSearcher()
        {
            Explain = -1;
        }

        protected IIndex Index { get; set; }

        /// <summary>
        /// The row index of a result to perform a query explanation for..
        /// </summary>
        public int Explain { get; set; }

        public Explanation Explanation { get; set; }
    }
}