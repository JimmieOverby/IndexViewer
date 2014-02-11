<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SitecoreQueryType.ascx.cs" Inherits="IndexViewer.SitecoreQueryType" %>
<asp:DropDownList ID="ddlQueryType" runat="server">
    <asp:ListItem Selected="True" Text="Full Text Query" Value="FullTextQuery"></asp:ListItem>
    <asp:ListItem Text="Field Query" Value="FieldQuery"></asp:ListItem>
</asp:DropDownList>
