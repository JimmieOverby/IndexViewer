using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq.Dynamic;
using Sitecore.Search;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;
using System.CodeDom.Compiler;
using System.IO;
using Microsoft.CSharp;
using System.Reflection;
using System.Text;
using IndexViewer.sitecore_modules.Shell.IndexViewer.Logic.CodeCompletion;
using IndexViewer.Logic.DynamicLinq;
using CodeGenerator = IndexViewer.Logic.DynamicLinq.CodeGenerator;

namespace IndexViewer.sitecore_modules.Shell.IndexViewer
{
    public partial class DynamicLinq : System.Web.UI.UserControl
    {
        private string _searchItemJSON;
        

        protected void Page_Load(object sender, EventArgs e)
        {
            DataBind();    
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (SessionManager.Instance.LinqSearchResults == null)
            {
                SearchResultGrid.DataSource = null;
                SearchResultGrid.DataBind();
            }
            else
            {
                SearchResultGrid.DataSource = SessionManager.Instance.LinqSearchResults.AsDataTable();
                SearchResultGrid.DataBind();
            }
            SearchResultTypeLabel.Text = GetObjectType();
        }

        protected void SearchResultGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                SearchResultGrid.PageIndex = e.NewPageIndex;
            }
            catch (Exception ex)
            {
                OnError(new ExceptionEventArgs(ex, this));
            }
        }

        protected void RunButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (SessionManager.Instance.CurrentIndex == null)
                    return;

                CodeGenerator generator = new CodeGenerator();
                string code = generator.GetLinqCode(GetObjectType(), GetWhereClause(), GetTakeClause());
                var result = Compiler.CompileAndRun(code, GetSearchResultItemAssembly());
                int timeTaken;
                if (!Int32.TryParse(result.Item1, out timeTaken))
                {
                    ErrorLabel.Text = result.Item1;
                }
                else
                {
                    ErrorLabel.Text = String.Empty;
                    //Unfortunately we can't really work on the entered searchResultClass type as it can't be passed to T
                    IEnumerable<SearchResultItem> results = result.Item2.Select(i => i as SearchResultItem);
                    LinqSearchResult<SearchResultItem> resultList = new LinqSearchResult<SearchResultItem>(results);
                    SessionManager.Instance.LinqSearchResults = resultList;
                    int numberOfHits = results.Count();
                    TotalHitsLabel.Text = numberOfHits.ToString();
                    TimeElapsedLabel.Text = result.Item1 + "ms";
                }
            }
            catch (Exception ex)
            {
                ErrorResolver.ResolveError(ex, this);
            }
        }

        protected void SearchTypeButton_Click(object sender, EventArgs e)
        {

        }

        protected string SearchItemJSON
        {
            get
            {
                if (_searchItemJSON == null)
                    _searchItemJSON = GetSearchItemJSON();
                return _searchItemJSON;
            }
        }

        protected string IndexName
        {
            get
            {
                if (SessionManager.Instance.CurrentIndex == null)
                    return String.Empty;

                return SessionManager.Instance.CurrentIndex.Name;
            }
        }

        private string GetSearchItemJSON()
        {
            MemberResolver memberResolver = new MemberResolver();
            IEnumerable<Member> members = memberResolver.GetMembersForCodeCompletion(GetFullyQualifiedObjectType());
            StringBuilder builder = new StringBuilder();
            builder.Append("[");
            var lastMember = members.Last();
            foreach (var member in members)
            {
                builder.Append("{ name : \"" + member.DisplayMemberName + "\", value: \"" + member.DisplayMemberName + "\", type: \"" + member.MemberType + "\"}");
                if (members != lastMember)
                {
                    builder.Append(",");
                }
            }
            builder.Append("]");
            return builder.ToString();
        }
        
        private string GetWhereClause()
        {
            return WhereStatementBox.Text;
        }

        private string GetTakeClause()
        {
            return TakeStatementBox.Text;
        }

        private string GetObjectType()
        {
            string fullType = GetFullyQualifiedObjectType();
            string[] pieces = fullType.Split(',');
            if (pieces.Length != 2)
                throw new InvalidOperationException("Fully qualified type is not valid");
            return pieces[0];

        }

        private string GetSearchResultItemAssembly()
        {
            string fullType = GetFullyQualifiedObjectType();
            string[] pieces = fullType.Split(',');
            if (pieces.Length != 2)
                throw new InvalidOperationException("Fully qualified type is not valid");
            return pieces[1].Trim();
        }

        private Type GetTypeOfSeachObject()
        {
            string typeAsString = GetFullyQualifiedObjectType();
            return Type.GetType(typeAsString);
        }

        private string GetFullyQualifiedObjectType()
        {
            string fullQualifiedType = ReturnTypeTextBox.Text;
            return fullQualifiedType;
        }

      
        
    }
}