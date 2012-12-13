using System;
using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Web.UI.WebControls;
using Sitecore.Diagnostics;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Web.UI.XamlSharp.Ajax;
using Sitecore.Web.UI.Sheer;

namespace IndexViewer
{
    /// <summary>
    /// The main form holds all other controls and is responsible for the context and the selection of this
    /// </summary>
    public partial class MainForm : System.Web.UI.Page, Sitecore.Shell.Framework.Commands.IHasCommandContext
    {
        #region IHasCommandContext implementation

        CommandContext IHasCommandContext.GetCommandContext()
        {
            CommandContext context = new CommandContext();
            Item itemNotNull = Client.GetItemNotNull("/sitecore/content/Applications/IndexViewer/Ribbon", Client.CoreDatabase);

            context.RibbonSourceUri = itemNotNull.Uri;

            context.Parameters["param1"] = "test";

            return context;
        }

        #endregion IHasCommandContext implementation



        #region protected methods for events

        protected void Page_Init(object sender, EventArgs e)
        {
            SearchControl.Error                 += new EventHandler(CustomControl_Error);
            DocumentsOverviewControl.Error      += new EventHandler(CustomControl_Error);
            IndexOverview.Error          += new EventHandler(CustomControl_Error);
            Client.AjaxScriptManager.OnExecute  += new AjaxScriptManager.ExecuteDelegate(AjaxScriptManager_OnExecute); 
        }

        
        //----------------------------------------------------------------------------------
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remark>Created 02/01/2009 11:30 by jm</remark>
        //----------------------------------------------------------------------------------
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                CheckUser();

                if (!Page.IsPostBack)
                {
                    SessionManager.Instance.ClearAll();

                    SetViewButtonActive(false);
                    IndexTabs.ActiveViewIndex = 0;
                }
            }
            catch (Exception ex)
            {
                ErrorResolver.ResolveError(ex, this);
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ErrorInfo.InnerHtml = ErrorResolver.CheckPageError();

            if (SessionManager.Instance.CurrentIndex != null)
            {
                SetViewButtonActive(true);
            }
            else
            {
                IndexTabs.ActiveViewIndex = 0;
                SetViewButtonActive(false);
            }
        }

        protected void Page_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            ErrorResolver.ResolveError(ex, this);
        }


        protected void CustomControl_Error(object sender, EventArgs e)
        {
            ExceptionEventArgs args = e as ExceptionEventArgs;

            if (args != null)
            {
                ErrorResolver.ResolveError(args.Exception, args.Sender);
            }
            else
            {
                ErrorResolver.ResolveError();
            }
        }


        //----------------------------------------------------------------------------------
        /// <summary>
        /// Handles the Click event of the OverviewButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remark>Created 02/01/2009 15:18 by jm</remark>
        //----------------------------------------------------------------------------------
        protected void OverviewButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (SessionManager.Instance.CurrentIndex != null)
                {
                    ShowIndexOverview();
                }
                else
                {
                    IndexTabs.ActiveViewIndex = 0;
                    SetViewButtonActive(false);
                }
            }
            catch (Exception ex)
            {
                ErrorResolver.ResolveError(ex, this);
            }
        }

        //----------------------------------------------------------------------------------
        /// <summary>
        /// Handles the Click event of the DocumentsButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remark>Created 02/01/2009 15:19 by jm</remark>
        //----------------------------------------------------------------------------------
        protected void DocumentsButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (SessionManager.Instance.CurrentIndex != null)
                {
                    ShowDocumentsOverview();
                }
                else
                {
                    IndexTabs.ActiveViewIndex = 0;
                    SetViewButtonActive(false);
                }
            }
            catch (Exception ex)
            {
                ErrorResolver.ResolveError(ex, this);
            }
        }

        //----------------------------------------------------------------------------------
        /// <summary>
        /// Handles the Click event of the SearchButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remark>Created 07/01/2009 10:47 by jm</remark>
        //----------------------------------------------------------------------------------
        protected void SearchButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (SessionManager.Instance.CurrentIndex != null)
                {
                    ShowSearch();
                }
                else
                {
                    IndexTabs.ActiveViewIndex = 0;
                    SetViewButtonActive(false);
                }
            }
            catch (Exception ex)
            {
                ErrorResolver.ResolveError(ex, this);
            }
        }

        protected void AjaxScriptManager_OnExecute(object sender, AjaxCommandEventArgs args)
        {
            Assert.ArgumentNotNull(sender, "sender");
            Assert.ArgumentNotNull(args, "args");

            try
            {
                switch (args.Name.ToLowerInvariant())
                {
                    case "indexviewer:indexrebuilded":
                        break;

                    case "indexviewer:indexoptimized":
                        break;

                    case "indexviewer:indexselected":
                        IndexTabs.ActiveViewIndex = 0;
                        SetViewButtonActive(true);

                        SheerResponse.Eval(String.Format("disableBtn('{0}', false); disableBtn('{1}', false); disableBtn('{2}', false);",
                            OverviewButton.UniqueID,
                            DocumentsButton.UniqueID,
                            SearchButton.UniqueID));

                        SheerResponse.Eval(String.Format("clickBtn('{0}');", OverviewButton.UniqueID));

                        break;

                    case "indexviewer:closed":
                        IndexTabs.ActiveViewIndex = 0;

                        SheerResponse.Eval(String.Format("clickBtn('{0}');", OverviewButton.UniqueID));

                        SetViewButtonActive(false);
                        break;


                    case "indexviewer:exitindexviewer":
                        SessionManager.Instance.ClearAll();
                        Sitecore.Shell.Framework.Windows.Close();
                        break;
                }
            }
            catch (Exception ex)
            {
                ErrorResolver.ResolveError(ex, this);
            }
        }

        #endregion


        #region private methods
        
        private void CheckUser()
        {
            if (Sitecore.Context.User == null ||
                Sitecore.Context.User.LocalName == "Anonymous")
            {
                Response.Redirect("/sitecore/login");
            }
        }

        //----------------------------------------------------------------------------------
        /// <summary>
        /// Refreshes the index information.
        /// </summary>
        /// <remark>Created 02/01/2009 10:44 by jm</remark>
        //----------------------------------------------------------------------------------
        private void RefreshIndexInformation()
        {
            IndexTabs.ActiveViewIndex = 0;

            if (SessionManager.Instance.CurrentIndex != null)
            {
                ShowIndexOverview();
                SetViewButtonActive(true);
            }
        }

        #endregion


        #region Visibility
        
        
        //----------------------------------------------------------------------------------
        /// <summary>
        /// Sets the view buttons to be either active or inactive.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <remark>Created 02/01/2009 15:12 by jm</remark>
        //----------------------------------------------------------------------------------
        private void SetViewButtonActive(bool value)
        {
            OverviewButton.Enabled = value;
            DocumentsButton.Enabled = value;
            SearchButton.Enabled = value;
        }


        //----------------------------------------------------------------------------------
        /// <summary>
        /// Shows the index overview.
        /// </summary>
        /// <remark>Created 02/01/2009 15:06 by jm</remark>
        //----------------------------------------------------------------------------------
        private void ShowIndexOverview()
        {
            IndexOverview.Visible = true;
            IndexTabs.ActiveViewIndex = 1;

            OverviewButton.CssClass = "ButtonSelectorSelected";
            DocumentsButton.CssClass = "ButtonSelector";
            SearchButton.CssClass = "ButtonSelector";
        }

        //----------------------------------------------------------------------------------
        /// <summary>
        /// Shows the documents overview.
        /// </summary>
        /// <remark>Created 02/01/2009 15:07 by jm</remark>
        //----------------------------------------------------------------------------------
        private void ShowDocumentsOverview()
        {
            IndexTabs.ActiveViewIndex = 2;

            OverviewButton.CssClass = "ButtonSelector";
            DocumentsButton.CssClass = "ButtonSelectorSelected";
            SearchButton.CssClass = "ButtonSelector";
        }

        //----------------------------------------------------------------------------------
        /// <summary>
        /// Shows the search.
        /// </summary>
        /// <remark>Created 07/01/2009 10:46 by jm</remark>
        //----------------------------------------------------------------------------------
        private void ShowSearch()
        {
            IndexTabs.ActiveViewIndex = 3;
            OverviewButton.CssClass = "ButtonSelector";
            DocumentsButton.CssClass = "ButtonSelector";
            SearchButton.CssClass = "ButtonSelectorSelected";
        }


        #endregion


    }
}
