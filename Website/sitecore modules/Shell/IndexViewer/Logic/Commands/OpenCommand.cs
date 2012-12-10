using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using Sitecore.Shell.Framework.Commands;
using Sitecore.Web.UI.XamlSharp.Continuations;
using Sitecore.Web.UI.Sheer;
using Sitecore.Web.UI.WebControls;
using Sitecore.Globalization;

namespace IndexViewer
{
    public class OpenCommand : Command, ISupportsContinuation
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
                if (SessionManager.Instance.CurrentIndex != null)
                {
                    AjaxScriptManager.Current.Dispatch("indexviewer:indexselected");
                }
            }
            else
            {
                Sitecore.Web.UI.WebControls.AjaxScriptManager.Current.ShowModalDialog("/sitecore modules/Shell/IndexViewer/SelectIndex.aspx", "330px", "230px", String.Empty, true);
                args.WaitForPostBack();
            }
        }

    }
}
