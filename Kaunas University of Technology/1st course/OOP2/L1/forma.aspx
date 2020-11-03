<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="forma.aspx.cs" Inherits="Lab1.forma" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Paleisti" />
            <br />
        </div>
        <br />
        <asp:Table ID="Table2" runat="server">
        </asp:Table>
        <br />
        <asp:Label ID="Label1" runat="server"></asp:Label>
        <asp:Table ID="Table1" runat="server">
        </asp:Table>
    </form>
</body>
</html>
