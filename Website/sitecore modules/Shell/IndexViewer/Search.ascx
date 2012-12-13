<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Search.ascx.cs" Inherits="IndexViewer.Search" %>

<div style="margin:10px;">

    <b>Enter search expression here:</b>
    
    <table cellpadding="2px" cellspacing="2px"">
        <tr>
            <td>Fieldname: <asp:TextBox ID="FieldNameTextBox" runat="server"></asp:TextBox></td>
            <td>Search word: <asp:TextBox ID="SearchWordTextBox" runat="server"></asp:TextBox></td>
            <td>
                  Query to use: 
                  <asp:DropDownList ID="QuerySelector" runat="server">
                    <asp:ListItem Selected="True" Text="QueryParser" Value="QueryParser"></asp:ListItem>
                    <asp:ListItem Text="TermQuery" Value="TermQuery"></asp:ListItem>
                    <asp:ListItem Text="PrefixQuery" Value="PrefixQuery"></asp:ListItem>
                    <asp:ListItem Text="WildCardQuery" Value="WildCardQuery"></asp:ListItem>
                  </asp:DropDownList>
            </td>
            <td>Exclude empty: <asp:CheckBox ID="ExcludeEmptyField" runat="server" Checked="true" /></td>
            <td align="center">
                <asp:Button ID="SearchFieldButton" CssClass="SearchButton" runat="server" Text="Search" 
                    onclick="SearchFieldButton_Click" />            
            </td>
        </tr>
    </table>
    

    
    <hr style="width:100%;"/>

    <div style="margin-top:2px; margin-bottom:2px; width:100%;"><h3>Search results:</h3></div>
        <div>
            Time elapsed for search: <b><asp:Label ID="TimeElapsedLabel" runat="server"></asp:Label></b>
        </div>
        <div>
            Total hits: <b><asp:Label ID="TotalHitsLabel" runat="server"></asp:Label></b>
        </div>
    
        <asp:GridView ID="SearchResultGrid" runat="server" 
                AllowPaging="True" 
                onpageindexchanging="SearchResultGrid_PageIndexChanging" 
                PageSize="15"
                CssClass="mGrid"
                PagerStyle-CssClass="pgr"
                AlternatingRowStyle-CssClass="alt"
                >
            <PagerStyle CssClass="PagerLinks" />
        </asp:GridView>


    <br/>
</div>
