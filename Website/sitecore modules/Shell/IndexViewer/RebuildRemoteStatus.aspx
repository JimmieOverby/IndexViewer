<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RebuildRemoteStatus.aspx.cs" Inherits="IndexViewer.sitecore_modules.Shell.IndexViewer.RebuildRemoteStatus" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Rebuilding remote index</title>
    <link rel="Stylesheet" type="text/css" href="~/sitecore modules/Shell/IndexViewer/css/IndexViewerDefault.css" /> 
</head>
<body style="background:transparent; background-color: #EEE; height: 100%; overflow:hidden; width:300px">
    <form id="form1" runat="server">
    <div>
        <div id="runningBox">
          <h1>Rebuilding remote index</h1>
          <div class="contentArea">
            <asp:Panel runat="server" ID="LoadingPanel">
              <asp:Label runat="server" ID="RunningLabel" />
                <img alt="spinner for waiting" width="40px" src="/sitecore modules/shell/IndexViewer/img/spinnerLarge.gif" />
             </asp:Panel>
             <asp:Panel runat="server" ID="DonePanel" Visible="false">
              <div>
                <asp:Label runat="server" ID="DoneLabel" /><br />
                <input type="button" class="doneButton" onclick="window.top.dialogClose();" value="Close"/>
              </div>
             </asp:Panel>
           </div>
          </div>
          
        </div>    
    </div>
    </form>
</body>
</html>
