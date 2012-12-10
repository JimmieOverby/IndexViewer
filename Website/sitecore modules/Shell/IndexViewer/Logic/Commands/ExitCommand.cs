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

namespace IndexViewer
{
    public class ExitCommand : Command, ISupportsContinuation
    {
        public override void Execute(CommandContext context)
        {
            AjaxScriptManager.Current.Dispatch("indexviewer:exitindexviewer");
        }
    }
}
