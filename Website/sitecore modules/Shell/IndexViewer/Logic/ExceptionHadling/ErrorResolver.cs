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

namespace IndexViewer
{
    public static class ErrorResolver
    {
        public static string CheckPageError()
        {
            if (SessionManager.Instance.LastError != null)
            {
                string message = "ERROR (please see log): " + SessionManager.Instance.LastError.Message + "<br>";

                if (SessionManager.Instance.LastError.ToString().Contains("Lucene") ||
                    SessionManager.Instance.LastError.StackTrace.Contains("Lucene"))
                {
                    message += "<br> Try rebuild indexes <br>";
                }

                message += "<br>";
            }

            SessionManager.Instance.LastError = null;

            return string.Empty;
        }

        public static void ResolveError(Exception exception, object sender)
        {
            exception = exception ?? new Exception("Unknown error.");

            Sitecore.Diagnostics.Log.Error(exception.Message, exception, sender);

            SessionManager.Instance.LastError = exception;
        }

        public static void ResolveError(Exception exception)
        {
            ResolveError(exception, null);
        }

        public static void ResolveError()
        {
            ResolveError(null, null);
        }
    }
}
