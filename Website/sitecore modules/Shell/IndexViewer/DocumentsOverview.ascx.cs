using System;
using Lucene.Net.Index;

namespace IndexViewer.sitecore_modules.Shell.IndexViewer
{
    public partial class DocumentsOverview : System.Web.UI.UserControl
    {

        #region protected methods

        //----------------------------------------------------------------------------------
        /// <summary>
        /// Handles the PreRender event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remark>Created 05/01/2009 15:51 by jm</remark>
        //----------------------------------------------------------------------------------
        protected void Page_PreRender(object sender, EventArgs e)
        {
            try
            {
                if (SessionManager.Instance.CurrentIndex != null)
                {
                    BindDocumentToGrid(SessionManager.Instance.CurrentDocumentNumber);

                    SelectedNumberTextBox.Text = SessionManager.Instance.CurrentDocumentNumber.ToString();
                    SelectedDocumentNumber.Text = SessionManager.Instance.CurrentDocumentNumber.ToString();
                    DocSelectorLastIndexLabel.Text = (SessionManager.Instance.CurrentIndex.GetDocumentCount() - 1).ToString();
                }
            }
            catch (Exception ex)
            {
                OnError(new ExceptionEventArgs(ex, this));
            }
        }


        //----------------------------------------------------------------------------------
        /// <summary>
        /// Handles the Click event of the SelectPreviousDocNumberButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.ImageClickEventArgs"/> instance containing the event data.</param>
        /// <remark>Created 02/01/2009 15:34 by jm</remark>
        //----------------------------------------------------------------------------------
        protected void SelectPreviousDocNumberButton_Click(object sender, EventArgs e)
        {
            try
            {
                SessionManager.Instance.CurrentDocumentNumber--;
            }
            catch (Exception ex)
            {
                OnError(new ExceptionEventArgs(ex, this));
            }
        }

        //----------------------------------------------------------------------------------
        /// <summary>
        /// Handles the Click event of the SelectNextDocNumberButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remark>Created 05/01/2009 15:27 by jm</remark>
        //----------------------------------------------------------------------------------
        protected void SelectNextDocNumberButton_Click(object sender, EventArgs e)
        {
            try
            {
                SessionManager.Instance.CurrentDocumentNumber++;
            }
            catch (Exception ex)
            {
                OnError(new ExceptionEventArgs(ex, this));
            }
        }

        //----------------------------------------------------------------------------------
        /// <summary>
        /// Handles the Click event of the GoButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remark>Created 05/01/2009 14:37 by jm</remark>
        //----------------------------------------------------------------------------------
        protected void GoButton_Click(object sender, EventArgs e)
        {
            try
            {
                int selectedNo = 0;

                Int32.TryParse(SelectedNumberTextBox.Text, out selectedNo);

                if (selectedNo >= 0 &&
                    selectedNo < SessionManager.Instance.CurrentIndex.GetDocumentCount())
                {
                    SessionManager.Instance.CurrentDocumentNumber = selectedNo;
                }
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
        /// Binds the document to grid.
        /// </summary>
        /// <param name="doc">The doc.</param>
        /// <remark>Created 02/01/2009 15:42 by jm</remark>
        //----------------------------------------------------------------------------------
        private void BindDocumentToGrid(int documentNumber)
        {
            IndexReader reader = SessionManager.Instance.CurrentIndex.Reader;
            reader.Reopen();

            if (reader != null)
            {
                try
                {
                    FieldRepeater.DataSource = reader.Document(documentNumber).GetFields();
                    FieldRepeater.DataBind();
                }
                finally
                {
                    reader.Dispose();
                }
            }
        }

        #endregion
    }
}