<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SitecoreSearch.ascx.cs" Inherits="IndexViewer.SitecoreSearch" %>
<%@ Register TagPrefix="uc" TagName="SitecoreQueryType" Src="~/sitecore modules/Shell/IndexViewer/SitecoreQueryType.ascx" %>
<%@ Register TagPrefix="uc" TagName="QueryOccurance" Src="~/sitecore modules/Shell/IndexViewer/QueryOccurance.ascx" %>
<div style="margin: 10px;">
    <b>Enter search expression here:</b>
    <table cellpadding="2px" cellspacing="2px">
        <tr>
            <td>
                Fieldname:
            </td>
            <td>
                Search word:
            </td>
            <td>
                Query to use:
            </td>
            <td>
                Occurrence:
            </td>
        </tr>
        <tr>
            <td>
                <asp:TextBox ID="txtFieldName1" runat="server"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtSearchWord1" runat="server"></asp:TextBox>
            </td>
            <td>
                <uc:SitecoreQueryType ID="SitecoreQueryType1" runat="server" />
            </td>
            <td>
                <uc:QueryOccurance ID="QueryOccurance1" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:TextBox ID="txtFieldName2" runat="server"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtSearchWord2" runat="server"></asp:TextBox>
            </td>
            <td>
                <uc:SitecoreQueryType ID="SitecoreQueryType2" runat="server" />
            </td>
            <td>
                <uc:QueryOccurance ID="QueryOccurance2" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:TextBox ID="txtFieldName3" runat="server"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtSearchWord3" runat="server"></asp:TextBox>
            </td>
            <td>
                <uc:SitecoreQueryType ID="SitecoreQueryType3" runat="server" />
            </td>
            <td>
                <uc:QueryOccurance ID="QueryOccurance3" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                Exclude Empty Columns: <asp:CheckBox ID="ExcludeEmptyField" runat="server" Checked="true" />
            </td>
            <td colspan="3" align="right">
                <asp:Button ID="SearchFieldButton" CssClass="SearchButton" runat="server" Text="Search" OnClick="SearchFieldButton_Click" />
            </td>
        </tr>
    </table>
    <hr style="width: 100%;" />
    <div style="margin-top: 2px; margin-bottom: 2px; width: 100%;">
        <h3>
            Search results:</h3>
    </div>
    <div>
        Time elapsed for search: <b>
            <asp:Label ID="TimeElapsedLabel" runat="server"></asp:Label></b>
    </div>
    <div>
        Total hits: <b>
            <asp:Label ID="TotalHitsLabel" runat="server"></asp:Label></b>
    </div>
    <asp:GridView ID="SearchResultGrid" runat="server" AllowPaging="True" OnPageIndexChanging="SearchResultGrid_PageIndexChanging" PageSize="15" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" EnableModelValidation="True" onselectedindexchanged="SearchResultGrid_SelectedIndexChanged">
<AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>

        <Columns>
            <asp:ButtonField CommandName="Select" HeaderText="Explain" ShowHeader="True" Text="show" />
        </Columns>
        <PagerStyle CssClass="PagerLinks" />
    </asp:GridView>
    <br />
    <div style="margin-top: 2px; margin-bottom: 2px; width: 100%;">
        <h3>
            Explanation:</h3>
            <asp:TextBox ID="txtExplanation" Rows="10" Columns="100" runat="server" TextMode="MultiLine"></asp:TextBox>
    </div>

</div>
