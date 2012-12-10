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
using Sitecore.Shell.Applications.Dialogs.ProgressBoxes;

namespace IndexViewer
{
    public class OptimizeCommand : Command, ISupportsContinuation
    {
        public override void Execute(CommandContext context)
        {
            if (SessionManager.Instance.CurrentIndex == null)
            {
                SheerResponse.Alert(Translate.Text("The index is not selected. Please select the index."));
            }
            else
            {
                ProgressBox.Execute("IndexRebuild", "Rebuild",
                                    this.GetIcon(context, string.Empty),
                                    new ProgressBoxMethod(this.Optimize),
                                    "indexviewer:indexoptimized",
                                    new object[] { SessionManager.Instance.CurrentIndex, context });
            }
        }

        public void Optimize(params object[] parameters)
        {
            IIndex index = parameters[0] as IIndex;
            index.Optimize();
        }
    }
}
