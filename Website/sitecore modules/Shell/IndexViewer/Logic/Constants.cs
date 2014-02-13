using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IndexViewer
{
    public class Constants
    {
        public struct ItemIds
        {
            public const string SettingsItemId = "{CE44221A-6C25-47DE-AFCB-9F68A0825A91}";
            public const string RemoteServerRoot = "{F756223A-951C-4B98-8939-F9FCBC1F3C38}";
        }

        public struct FieldNames
        {
            public const string SecurityToken = "Settings_SecurityToken";
            public const string EnableRemoteRebuild = "Settings_EnableRemoteRebuild";
            public const string ServerAddress = "ServerInfo_Address";
        }
    }
}   