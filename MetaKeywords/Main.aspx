<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="MetaKeywords.Main" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1>Słowa kluczowe</h1>
        </div>
        <asp:Panel ID="pnlUrl" runat="server" DefaultButton="">
            <asp:Label ID="lblUrl" runat="server" Text="Wprowadź URL"></asp:Label>
            <asp:TextBox ID="txtUrl" runat="server" autocomplete="off"></asp:TextBox>
            <asp:CheckBox ID="cbShowPage" runat="server" Text="Wyświetl stronę" />
            <asp:Button ID="btnClick" runat="server" Text="Sprawdź słowa kluczowe" OnClick="btnClick_Click" ValidationGroup="form" />
            <br />            
            <asp:RequiredFieldValidator ID="rfvUrl" runat="server" ControlToValidate="txtUrl" Display="Dynamic" ForeColor="Red" ErrorMessage="Wprowadź adres url" ValidationGroup="form"></asp:RequiredFieldValidator>
        </asp:Panel>
        <asp:Panel ID="pnlMessage" runat="server" Visible="false">
            <asp:Literal ID="ltrMessage" runat="server"></asp:Literal>
        </asp:Panel>
        <asp:Panel ID="pnlResult" runat="server" Visible="false" style="float:left; width:250px; margin-top:10px;">
            <asp:Literal ID="ltrInfo" runat="server"></asp:Literal>
        </asp:Panel>
        <asp:Panel ID="pnlResult2" runat="server" Visible="false" style="float:left; width:250px; margin-top:10px;">
            <asp:Literal ID="ltrInfo2" runat="server"></asp:Literal>
        </asp:Panel>
        <asp:Panel ID="pnlPreview" runat="server" Visible="false">
            <iframe id="frame" runat="server" scrolling="auto" style="width:100%; height: 500px;"></iframe>
        </asp:Panel>
    </form>
</body>
</html>
