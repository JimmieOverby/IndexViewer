<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectIndex.aspx.cs" Inherits="IndexViewer.SelectIndex" %>
<%@ Register TagPrefix="sc" Namespace="Sitecore.Web.UI.HtmlControls" Assembly="Sitecore.Kernel" %>
<%@ Register TagPrefix="sc" Assembly="Sitecore.Kernel" Namespace="Sitecore.Web.UI.WebControls" %>
<%@ Register TagPrefix="sc" Assembly="Sitecore.Kernel" Namespace="Sitecore.Web.UI.WebControls.Ribbons" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Select an index</title>
    
    <BASE target="_self">
    
    <sc:Stylesheet Src="Content Manager.css" DeviceDependant="true" runat="server" />
    <link rel="Stylesheet" type="text/css" href="/sitecore modules/Shell/IndexViewer/css/IndexViewerDefault.css" /> 
        
</head>

<body style="background:transparent; background-color: #EEE; height: 100%; width:100%; overflow:hidden;">
    <form id="form1" runat="server">
        
        <div class="ErrorText" style="color:Red " ID="ErrorInfo" runat="server">&nbsp;</div>
        
        <table style="margin:10px; overflow:hidden; width:100%;" cellpadding="2px">
            <caption><b>Select index</b></caption>
            <tr>
                <td>Index Type</td>
                <td>
                    <asp:DropDownList CssClass="Selector" ID="IndexTypeSelector" runat="server" AutoPostBack="True" 
                        onselectedindexchanged="IndexTypeSelector_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Index</td>
                <td>
                    <asp:dropdownlist CssClass="Selector" ID="IndexSelector" runat="server" AutoPostBack="True" 
                      onselectedindexchanged="IndexSelector_SelectedIndexChanged"></asp:dropdownlist>      
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <br/>
                    <asp:Button ID="OKButton" CssClass="SelectIndexButton" Text="OK" runat="server" OnClick="OKButton_Click" />
                    <asp:Button ID="CancelButton" CssClass="SelectIndexButton" Text="Cancel" OnClick="CancelButton_Click" runat="server" />
                </td>
            </tr>            
        </table>
        
    </form>
</body>
</html>
