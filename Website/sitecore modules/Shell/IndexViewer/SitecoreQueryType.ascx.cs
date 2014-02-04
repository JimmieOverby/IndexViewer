using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IndexViewer
{
    public partial class SitecoreQueryType : System.Web.UI.UserControl
    {
        public DropDownList Dropdown
        {
            get
            {
                return this.ddlQueryType;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}