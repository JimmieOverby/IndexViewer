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
    public class ExceptionEventArgs : EventArgs
    {
        public ExceptionEventArgs(Exception exception) : this(exception, null) { }

        public ExceptionEventArgs(Exception exception, object sender)
        {
            _exception = exception;
            _sender = sender;
        }

        public Exception Exception 
        { 
            get { return _exception; } 
        }
        private Exception _exception;


        public object Sender 
        { 
            get { return _sender; } 
        }
        private object _sender;
    }
}
