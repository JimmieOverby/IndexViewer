using System;
using Sitecore.Data.Indexing;
using Sitecore.Data;


namespace IndexViewer
{
    public partial class IndexOverview : System.Web.UI.UserControl
    {

        #region page lifecycle methods

        //----------------------------------------------------------------------------------
        /// <summary>
        /// Handles the PreRender event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remark>Created 07/01/2009 10:18 by jm</remark>
        //----------------------------------------------------------------------------------
        protected void Page_PreRender(object sender, EventArgs e)
        {
            try
            {
                RefreshOverviewInformation();
                RefreshFieldInformation();
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
        /// Refreshes the field information.
        /// </summary>
        /// <remark>Created 06/02/2009 09:51 by jm</remark>
        //----------------------------------------------------------------------------------
        private void RefreshFieldInformation()
        {
            if (SessionManager.Instance.CurrentIndex != null)
            {
                FieldsRepeater.DataSource = SessionManager.Instance.CurrentIndex.Fields;
                FieldsRepeater.DataBind();
            }
        }

        //----------------------------------------------------------------------------------
        /// <summary>
        /// Refreshes the overview information.
        /// </summary>
        /// <remark>Created 02/01/2009 12:21 by jm</remark>
        //----------------------------------------------------------------------------------
        private void RefreshOverviewInformation()
        {
            if (SessionManager.Instance.CurrentIndex != null)
            {
                IndexNameValue.Text         = SessionManager.Instance.CurrentIndex.Name;
                IndexDirectoryValue.Text    = SessionManager.Instance.CurrentIndex.GetDirectoryPath();
                NumberOfDocumentsValue.Text = SessionManager.Instance.CurrentIndex.GetDocumentCount().ToString();
                DateTime lastUpdated        = SessionManager.Instance.CurrentIndex.GetLastUpdated();
                LastModifiedValue.Text      = lastUpdated.ToShortDateString()
                                                + " "
                                                + lastUpdated.ToShortTimeString();
            }
        }

        #endregion
    }
}