<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DocumentsOverview.ascx.cs" Inherits="IndexViewer.sitecore_modules.Shell.IndexViewer.DocumentsOverview" %>


<div style="margin:10px;">

    <div style="margin-top:10px; margin-bottom:10px;"><h3>Browse by document number:</h3></div>
    
    <div>
      Doc. #. 0 
      <asp:Button CssClass="IterateButton" ID="SelectPreviousDocNumberButton" runat="server" 
        onclick="SelectPreviousDocNumberButton_Click" Text="&lt;" />
      <asp:TextBox ID="SelectedNumberTextBox" runat="server" >0</asp:TextBox>
      <asp:Button CssClass="IterateButton" ID="SelectNextDocNumberButton" runat="server" 
        CausesValidation="False" onclick="SelectNextDocNumberButton_Click" 
        Text="&gt;" />
      <asp:Label ID="DocSelectorLastIndexLabel" runat="server" />
      <asp:Button ID="GoButton" CssClass="GoButton" runat="server" onclick="GoButton_Click" Text="Go" />
    </div>    
    
    <br/>
    <hr style="width:100%;"/>
    
    <div style="margin-top:10px; margin-bottom:10px;">
        Doc #: <b><asp:Label ID="SelectedDocumentNumber" runat="server" /></b>
    </div>
    
    <table cellpadding="2px" cellspacing="0" class="mGrid" style="width:100%; white-space:normal;">
      <asp:Repeater ID="FieldRepeater" runat="server">
        <HeaderTemplate>
            <tr>
              <th>No</th>
              <th>Field Name</th>
              <th>Field Value</th>
            </tr>
          </thead>
        </HeaderTemplate>
        <ItemTemplate>
          <tr>
            <td style="text-align:center; width:30px;"><%# Container.ItemIndex %></td>
            <td><%# (Container.DataItem as Lucene.Net.Documents.Field).Name()%></td>
            <td style="white-space:normal;"><%# (Container.DataItem as Lucene.Net.Documents.Field).StringValue()%></td>
          </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
          <tr class="alt">
            <td style="text-align:center; width:30px;"><%# Container.ItemIndex %></td>
            <td><%# (Container.DataItem as Lucene.Net.Documents.Field).Name()%></td>
            <td style="white-space:normal;"><%# (Container.DataItem as Lucene.Net.Documents.Field).StringValue()%></td>
          </tr>
        </AlternatingItemTemplate>
      </asp:Repeater>
    </table>
    
    <br />

</div>


