<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MainForm.aspx.cs" Inherits="IndexViewer.MainForm" %>
<%@ Register src="IndexOverview.ascx" tagname="IndexOverview" tagprefix="IndexViewer" %>
<%@ Register src="DocumentsOverview.ascx" tagname="DocumentsOverview" tagprefix="IndexViewer" %>
<%@ Register src="LuceneSearch.ascx" tagname="LuceneSearch" tagprefix="IndexViewer" %>
<%@ Register src="SitecoreSearch.ascx" tagname="SitecoreSearch" tagprefix="IndexViewer" %>
<%@ Register TagPrefix="sc" Namespace="Sitecore.Web.UI.HtmlControls" Assembly="Sitecore.Kernel" %>
<%@ Register TagPrefix="sc" Assembly="Sitecore.Kernel" Namespace="Sitecore.Web.UI.WebControls" %>
<%@ Register TagPrefix="sc" Assembly="Sitecore.Kernel" Namespace="Sitecore.Web.UI.WebControls.Ribbons" %>
<%@ Register Src="~/sitecore modules/Shell/IndexViewer/DynamicLinq.ascx" TagPrefix="IndexViewer" TagName="DynamicLinq" %>



<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    
    <sc:Stylesheet Src="Content Manager.css" DeviceDependant="true" runat="server" />
    
    <link rel="stylesheet" type="text/css" href="/sitecore modules/Shell/IndexViewer/css/jquery-ui.css"/>
    <link rel="Stylesheet" type="text/css" href="/sitecore modules/Shell/IndexViewer/css/Ribbon.css" /> 
    <link rel="Stylesheet" type="text/css" href="/sitecore modules/Shell/IndexViewer/css/IndexViewerDefault.css" /> 

    
    <script type="text/javascript" language="javascript" src="/sitecore/shell/Controls/Lib/Prototype/prototype.js"></script>
    <script type="text/javascript" language="javascript" src="/sitecore/shell/Controls/InternetExplorer.js"></script>
    <script type="text/javascript" language="javascript" src="/sitecore/shell/Controls/Sitecore.js"></script>
    <script type="text/javascript" language="javascript" src="/sitecore/shell/Controls/SitecoreObjects.js"></script>
    <script type="text/javascript" language="javascript" src="/sitecore/shell/Applications/Content Manager/Content Editor.js"></script>
    
    <script type="text/javascript" language="javascript">
        
        function disableBtn(btnID, isDisable) {
            var btn = document.getElementById(btnID);
                btn.disabled = isDisable;
        }
        
        function clickBtn(btnID) {
            var btn = document.getElementById(btnID);
                btn.click();
        }
        
    </script>    
    

</head>
<body style="background:transparent; height:1%;">

    <input type="hidden" id="scActiveRibbonStrip" name="scActiveRibbonStrip" />
    
    <form id="form1" runat="server" style="position:relative;">
    
    <sc:AjaxScriptManager ID="AjaxScriptManager1" runat="server"/>
    <sc:ContinuationManager ID="ContinuationManager1" runat="server" />
    
    <sc:Ribbon ID="Ribbon" runat="server" />
    
    <div class="ErrorText" style="color:Red " ID="ErrorInfo" runat="server">&nbsp;</div>
        
    <table cellpadding="0" cellspacing="0" style="margin:5px; width:99%; height:100%;">
        <tr>
            <td>
                <asp:Button id="OverviewButton" OnClick="OverviewButton_Click" CssClass="ButtonSelector" runat="server" 
                    Text="Overview">
                </asp:Button>
                <asp:Button id="DocumentsButton" OnClick="DocumentsButton_Click" CssClass="ButtonSelector" runat="server" 
                    Text="Documents">
                </asp:Button>
                <asp:Button id="LuceneSearchButton" OnClick="LuceneSearchButton_Click" CssClass="ButtonSelector" runat="server" 
                    Text="Lucene.NET API">
                </asp:Button>
                <asp:Button id="SitecoreSearchButton" OnClick="SitecoreSearchButton_Click" CssClass="ButtonSelector" runat="server" 
                    Text="Sitecore.Search API">
                </asp:Button>
				<asp:Button id="LinqButton" CssClass="ButtonSelector" runat="server" Text="LINQ" OnClick="LinqButton_Click">
                </asp:Button>   
            </td>
        </tr>
        <tr style="width:100%; height:100%; background-color:#EEE;">
            <td style="width:100%; height:100%;">
                <asp:MultiView ID="IndexTabs" runat="server" ActiveViewIndex="0">
                    <asp:View ID="WelcomeTab" runat="server">
                        <div class="WelcomeMessage">
                            <div class="Information">
                              <p>
                                Welcome to the IndexViewer
                              </p>
                              <p>
                              Please select a database and an Index
                              </p>
                            </div>
                            <div class="ContactInformation">
                              If you have any problems or feature request contact me at:<br /><br />
                              Jens Mikkelsen<br />
                              Inmento Solutions<br />
                              <a href="mailto:jm@inmento.dk">jm@inmento.dk</a><br /><br />                                    
                            </div>                    
                        </div>
                    </asp:View>
                    
                    <asp:View ID="IndexTab" runat="server">
                        <table cellspacing="0" style="width:100%; height:100%; background-color:#EEE;">
                            <tr>
                                <td><IndexViewer:IndexOverview ID="IndexOverview" runat="server"/> </td>
                            </tr>
                        </table>
                    </asp:View>
                    
                    <asp:View ID="DocumentsTab" runat="server">
                        <table cellspacing="0" style="width:100%; height:100%; background-color:#EEE;">
                            <tr>
                                <td>
                                    <IndexViewer:DocumentsOverview ID="DocumentsOverviewControl" runat="server"></IndexViewer:DocumentsOverview>
                                </td>
                            </tr>
                        </table>
                    </asp:View>
                    
                    <asp:View ID="LuceneSearchTab" runat="server">
                        <table cellspacing="0" style="width:100%; height:100%; background-color:#EEE;">
                            <tr>
                                <td>
                                    <IndexViewer:LuceneSearch ID="LuceneSearchControl" runat="server"/>        
                                </td>
                            </tr>
                        </table>
                    </asp:View>

                    <asp:View ID="SitecoreSearchTab" runat="server">
                        <table cellspacing="0" style="width:100%; height:100%; background-color:#EEE;">
                            <tr>
                                <td>
                                    <IndexViewer:SitecoreSearch ID="SitecoreSearchControl" runat="server"/>        
                                </td>
                            </tr>
                        </table>
                    </asp:View>
                    <asp:View ID="LinqTab" runat="server">
                        <table cellspacing="0" style="width:100%; height:100%; background-color:#EEE;">
                            <tr>
                                <td>
                                    <IndexViewer:DynamicLinq runat="server" ID="DynamicLinq" />
                                </td>
                            </tr>
                        </table>
                    </asp:View>
                </asp:MultiView>
            </td>
        </tr>
    </table>

    </form>
</body>
</html>
