using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace IndexViewer.sitecore_modules.Web.IndexViewer
{
    public static class RequestValidator
    {
        public static bool IsRequestValid(string tokenFromQueryString)
        {
            Item settingsItem = Sitecore.Context.Database.GetItem(new ID(Constants.ItemIds.SettingsItemId));
            if (settingsItem == null)
                throw new InvalidOperationException("IndexViewer not configured correcty. Cannot find settings items");
            CheckboxField enableRemoteRebuild = settingsItem.Fields[Constants.FieldNames.EnableRemoteRebuild];
            if (enableRemoteRebuild == null)
                return false;
            if (!enableRemoteRebuild.Checked)
                return false;

            string enteredToken = settingsItem[Constants.FieldNames.SecurityToken];
            string tokenSent = tokenFromQueryString;
            return (enteredToken == tokenSent);
        }
    }
}