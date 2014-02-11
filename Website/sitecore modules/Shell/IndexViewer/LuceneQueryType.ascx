<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LuceneQueryType.ascx.cs" Inherits="IndexViewer.LuceneQueryType" %>
<asp:DropDownList ID="ddlQueryType" runat="server">
    <asp:ListItem Selected="True" Text="QueryParser" Value="QueryParser"></asp:ListItem>
    <asp:ListItem Text="TermQuery" Value="TermQuery"></asp:ListItem>
    <asp:ListItem Text="PrefixQuery" Value="PrefixQuery"></asp:ListItem>
    <asp:ListItem Text="WildCardQuery" Value="WildCardQuery"></asp:ListItem>
</asp:DropDownList>
