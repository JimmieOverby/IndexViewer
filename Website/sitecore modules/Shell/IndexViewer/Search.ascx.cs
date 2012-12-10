using System;
using System.Web.UI.WebControls;
using Sitecore.Data;
using Sitecore.Data.Indexing;


namespace IndexViewer
{
    public partial class Search : System.Web.UI.UserControl
    {
        #region protected methods

        //----------------------------------------------------------------------------------
        /// <summary>
        /// Handles the PreRender event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remark>Created 10/02/2009 14:36 by jm</remark>
        //----------------------------------------------------------------------------------
        protected void Page_PreRender(object sender, EventArgs e)
        {
            try
            {
                if (SessionManager.Instance.CurrentIndex == null ||
                    SessionManager.Instance.SearchResult == null)
                {
                    SearchResultGrid.DataSource = null;
                    SearchResultGrid.DataBind();
                    SetSearchInfoValues(0, 0);
                }
                else
                {
                    SetSearchInfoValues(SessionManager.Instance.SearchResult.TimeElapsed,
                                        SessionManager.Instance.SearchResult.SearchResultHits.Length());

                    SearchResultGrid.DataSource = SessionManager.Instance.SearchResult.AsDataTable(SessionManager.Instance.CurrentIndex.Fields, ExcludeEmptyField.Checked);
                    SearchResultGrid.DataBind();
                }
                
            }
            catch(Exception ex)
            {
                OnError(new ExceptionEventArgs(ex, this));
            }
        }


        //----------------------------------------------------------------------------------
        /// <summary>
        /// Handles the Click event of the SearchFieldButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remark>Created 07/01/2009 13:45 by jm</remark>
        //----------------------------------------------------------------------------------
        protected void SearchFieldButton_Click(object sender, EventArgs e)
        {
            try
            {
                IndexSearch search = new IndexSearch(SessionManager.Instance.CurrentIndex);
                SearchResultCollection results = search.FieldSearch(FieldNameTextBox.Text, SearchWordTextBox.Text, QuerySelector.SelectedValue);

                SessionManager.Instance.SearchResult = results;
            }
            catch (Exception ex)
            {
                OnError(new ExceptionEventArgs(ex, this));
            }
        }

        //----------------------------------------------------------------------------------
        /// <summary>
        /// Handles the PageIndexChanging event of the SearchResultGrid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewPageEventArgs"/> instance containing the event data.</param>
        /// <remark>Created 10/02/2009 16:24 by jm</remark>
        //----------------------------------------------------------------------------------
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

        #endregion

        #region private methods

        //----------------------------------------------------------------------------------
        /// <summary>
        /// Sets the search info values.
        /// </summary>
        /// <param name="elapsed">The elapsed.</param>
        /// <param name="totalhits">The totalhits.</param>
        /// <remark>Created 07/01/2009 13:49 by jm</remark>
        //----------------------------------------------------------------------------------
        private void SetSearchInfoValues(double elapsed, int totalhits)
        {
            TimeElapsedLabel.Text = elapsed.ToString();
            TotalHitsLabel.Text = totalhits.ToString();
        }

        #endregion

    }
}