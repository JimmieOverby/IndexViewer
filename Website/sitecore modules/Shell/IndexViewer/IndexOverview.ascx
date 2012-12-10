<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IndexOverview.ascx.cs" Inherits="IndexViewer.IndexOverview" %>

<div style="margin:10px;">

    <table cellpadding="1" cellspacing="1">
        <tr>
            <td>Index name:</td>
            <td class="OverviewValue"><asp:Label ID="IndexNameValue" runat="server" /></td>
        </tr>
        <tr>
            <td>Index directory:</td>
            <td class="OverviewValue"><asp:Label ID="IndexDirectoryValue" runat="server" /></td>
        </tr>
        <tr>
            <td>Number of documents:</td>
            <td class="OverviewValue"><asp:Label ID="NumberOfDocumentsValue" runat="server" /></td>
        </tr>
        <tr>
            <td>Last modified:</td>
            <td class="OverviewValue"><asp:Label ID="LastModifiedValue" runat="server" /></td>
        </tr>
    </table>
    
    <br/>
    <hr style="width:100%;"/>    

    <div style="margin-top:10px; margin-bottom:10px;"><h3>Available Fields:</h3></div>

    <table class="mGrid" style="width:100%;">
    <asp:Repeater ID="FieldsRepeater" runat="server">
    <HeaderTemplate>
      <thead>
        <tr>
          <th>No</th>
          <th>Field Name</th>
        </tr>
      </thead>
    </HeaderTemplate>
    
    <ItemTemplate>
      <tr>
        <td style="text-align:center; width:30px;"><%# Container.ItemIndex %></td>
        <td><%# Container.DataItem.ToString() %></td>
      </tr>
    </ItemTemplate>
    <AlternatingItemTemplate>
      <tr class="alt">
        <td style="text-align:center; width:30px;"><%# Container.ItemIndex %></td>
        <td><%# Container.DataItem.ToString() %></td>
      </tr>
    </AlternatingItemTemplate>
    </asp:Repeater>
    </table>
    
    <br />

</div>