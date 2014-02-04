<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QueryOccurance.ascx.cs" Inherits="IndexViewer.QueryOccurance" %>
<asp:DropDownList ID="ddlQueryOccurance" runat="server">
    <asp:ListItem Selected="True" Text="Must" Value="Must"></asp:ListItem>
    <asp:ListItem Text="Must Not" Value="MustNot"></asp:ListItem>
    <asp:ListItem Text="Should" Value="Should"></asp:ListItem>
</asp:DropDownList>
