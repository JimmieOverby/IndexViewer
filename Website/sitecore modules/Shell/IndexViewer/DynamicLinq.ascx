<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DynamicLinq.ascx.cs" Inherits="IndexViewer.sitecore_modules.Shell.IndexViewer.DynamicLinq" %>
<script src="/sitecore modules/shell/IndexViewer/scripts/jquery-1.10.2.min.js"></script>
<script src="/sitecore modules/shell/IndexViewer/scripts/jquery-ui-1.10.4.custom.min.js"></script>
<script src="/sitecore modules/shell/IndexViewer/scripts/jquery.caret.js"></script>
<script type="text/javascript">
    $(document).ready(function(){
        $.indexViewer = $.indexViewer || {};
        $.indexViewer.autoComplete = $.indexViewer.autoComplete || {};
        $.indexViewer.autoComplete.searchItemMembers = <%# SearchItemJSON %>;
        });
</script>
<script src="/sitecore modules/shell/IndexViewer/scripts/autoCompletion.js"></script>
<div class="linqArea">
    <div class="typeNameArea">
        Enter fully qualified type for Search return type: <asp:TextBox runat="server" ID="ReturnTypeTextBox" Text="Sitecore.ContentSearch.SearchTypes.SearchResultItem, Sitecore.ContentSearch" CssClass="searchTypeTextBox"/><asp:Button runat="server" ID="SearchTypeButton" Text="Use type" OnClick="SearchTypeButton_Click" /><br />
    </div>
    <div class="codeArea">
        <span class="darkblue">using</span> (<span class="darkblue">var</span> context = <span class="lightblue">ContentSearchManager</span>.GetIndex(<span class="darkred">"<%# IndexName %>"</span>)).CreateSearchContext())<br />
        {<br/>
            &nbsp;&nbsp;&nbsp;&nbsp;context.GetQueryable&lt;<span class="lightblue"><span id="type"><asp:Label runat="server" ID="SearchResultTypeLabel" /></span></span>&gt()<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;.Where(searchItem => <asp:TextBox runat="server" ID="WhereStatementBox" Text="" ClientIDMode="Static"/>)<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;.Take(<asp:TextBox runat="server" ID="TakeStatementBox" Text="" />)<br />
        }
    </div>
    <div class="errorArea">
        <asp:Label runat="server" ID="ErrorLabel"/>
    </div>
    <asp:Button ID="RunButton" runat="server" OnClick="RunButton_Click" Text="Run!" />
    <hr style="width:100%" />
    <div style="margin-top:2px; margin-bottom:2px; width:100%;"><h3>Search results:</h3></div>
    <div>
        Time elapsed for search: <b><asp:Label ID="TimeElapsedLabel" runat="server"></asp:Label></b>
    </div>
    <div>
        Total hits: <b><asp:Label ID="TotalHitsLabel" runat="server"></asp:Label></b>
    </div>
    
    <div class="searchResultArea">
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
    </div>
</div>