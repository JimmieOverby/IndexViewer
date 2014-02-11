using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace IndexViewer.Logic.DynamicLinq
{
    public class CodeGenerator
    {
        public string GetLinqCode(string objectType, string whereStatement, string takestatement)
        {
            return FirstLevelCode + GetLinqStatement(objectType, whereStatement, takestatement) + LastLevelCode;
        }

        private string GetLinqStatement(string objectType, string whereStatement, string takeStatement)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"return context.GetQueryable<");
            sb.Append(objectType);
            sb.Append(@">()");
            string whereClause = whereStatement;
            if (!String.IsNullOrEmpty(whereClause))
            {
                sb.Append(".Where(searchItem => ");
                sb.Append(whereClause);
                sb.Append(")");
            }
            string takeClause = takeStatement;
            if (!String.IsNullOrEmpty(takeClause))
            {
                sb.Append(".Take(");
                sb.Append(takeClause);
                sb.Append(").ToList();");
            }
            else
            {
                sb.Append(".ToList();");
            }
            return sb.ToString();
        }

        #region code strings

        private string FirstLevelCode
        {
            get
            {


                return @"
                        using System;
                        using System.Collections.Generic;
                        using System.Diagnostics;
                        using System.Linq;
                        using System.Web;
                        using Sitecore.ContentSearch.SearchTypes;
                        using Sitecore.Buckets.Extensions;
                        using Sitecore.Buckets.Interfaces;
                        using Sitecore.Buckets.Search;
                        using Sitecore.Buckets.Search.Tags;
                        using Sitecore.Configuration;
                        using Sitecore.ContentSearch;
                        using Sitecore.ContentSearch.Utilities;
                        using Sitecore.Data;
                        using Sitecore.Data.Fields;
                        using Sitecore.Data.Items;
                        using Sitecore.Globalization;
                        using Sitecore.SecurityModel;
                        using Sitecore.Sites;
                        using Sitecore.Web;
                        using Sitecore;
                        using System.ComponentModel;
                        using System.Diagnostics.CodeAnalysis;

                        using Sitecore.ContentSearch.Linq;
                        using Sitecore.ContentSearch.Linq.Common;

                        using Constants = Sitecore.Buckets.Util.Constants;
                        using ContentSearchManager = Sitecore.ContentSearch.ContentSearchManager;
                     
                        namespace Test {
                    
                           class Program {
                                           private static Stopwatch stopWatch = new Stopwatch();
                                            public static IEnumerable<SearchResultItem> Main(string str)
                                            {
                                                using (var context = ContentSearchManager.GetIndex(" + "\"" + SessionManager.Instance.CurrentIndex.Name + "\"" + @").CreateSearchContext())
                                                {";

            }
        }

        private string LastLevelCode
        {
            get
            {
                return @"}
                        }
                    
                        public static string RunTimer(string str) {
                        return stopWatch.ElapsedMilliseconds.ToString();
                        }
                    }
                  }
                    ";
            }
        }

        #endregion

    }


}