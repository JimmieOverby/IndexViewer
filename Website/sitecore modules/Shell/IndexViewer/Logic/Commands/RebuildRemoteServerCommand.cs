using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Web.UI.XamlSharp.Continuations;
using Sitecore.Web.UI.Sheer;
using Sitecore.Web.UI.WebControls;

namespace IndexViewer
{
    public class RebuildRemoteServerCommand : Command, ISupportsContinuation
    {

        public override void Execute(CommandContext context)
        {
            ClientPipelineArgs args = new ClientPipelineArgs();
            ContinuationManager.Current.Start(this, "Run", args);
        }

        protected void Run(ClientPipelineArgs args)
        {
            if (args.IsPostBack)
            {
                //Little nasty to trigger it here
                if (SessionManager.Instance.CurrentRebuildingJobUrl != null)
                {
                    Sitecore.Web.UI.WebControls.AjaxScriptManager.Current.ShowModalDialog("/sitecore modules/Shell/IndexViewer/RebuildRemoteStatus.aspx?jobUrl=" + SessionManager.Instance.CurrentRebuildingJobUrl, "330px", "230px", "Rebuilding index", true);
                    SessionManager.Instance.CurrentRebuildingJobUrl = null;
                    args.WaitForPostBack();
                }
            }
            else
            {
                AjaxScriptManager.Current.ShowModalDialog("/sitecore modules/Shell/IndexViewer/RebuildRemoteServer.aspx", "330px", "230px", String.Empty, true);
                args.WaitForPostBack();
            }
        }
    }
}