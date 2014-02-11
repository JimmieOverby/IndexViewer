<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RebuildRemoteServer.aspx.cs" Inherits="IndexViewer.sitecore_modules.Shell.IndexViewer.RebuildRemoteServerCommand" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Select remote index</title>
    <link rel="Stylesheet" type="text/css" href="~/sitecore modules/Shell/IndexViewer/css/IndexViewerDefault.css" /> 
</head>
<body style="background:transparent; background-color: #EEE; height: 100%; overflow:hidden; width:300px">
    <form id="form1" runat="server">
    
    <div class="selectRemoteIndex">
      <h1>Select remote index</h1>
      <div class="selectionArea">
        <div>
          <label>Select server:</label>
          <asp:DropDownList runat="server" ID="ServerDropDown" 
            onselectedindexchanged="ServerDropDown_SelectedIndexChanged" AutoPostBack="true" CssClass="dropDownBox"></asp:DropDownList>
        </div>
        <div>
          <label>Select index:</label>
          <asp:DropDownList runat="server" ID="IndexDropDown" Enabled="false" AutoPostBack="true" 
            onselectedindexchanged="IndexDropDown_SelectedIndexChanged" CssClass="dropDownBox"></asp:DropDownList>
        </div>
      </div>
     <div>
       <div class="buttonArea">
          <asp:Button runat="server" ID="RebuildButton" Text="Rebuild" Enabled="false" 
             onclick="RebuildButton_Click" CssClass="rebuildButton"/>
          <asp:Button runat="server" ID="CancelButton" Text="Cancel" 
            onclick="CancelButton_Click" CssClass="cancelButton"/>
        </div>
      </div>
    </div>
    </form>
</body>
</html>
