<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Forma.aspx.cs" Inherits="L4.Forma" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <link href="Style.css" rel="stylesheet" />
            <asp:Button ID="Button_Read" runat="server" OnClick="Button_Read_Click" Text="Nuskaityti" />
            <asp:Table ID="Table_Data" runat="server">
            </asp:Table>
            <br />
            <asp:Button ID="Button_Calculate" runat="server" OnClick="Button_Calculate_Click" Text="Paskaičiuoti" Visible="False" />
            <asp:Table ID="Table_Calculated" runat="server">
            </asp:Table>
            <br />
            <asp:Label ID="Label_Input" runat="server" Text="Įveskite datas:" Visible="False"></asp:Label>
            <br />
            <asp:Label ID="Label_From" runat="server" Text="Nuo:" Visible="False"></asp:Label>
            <asp:TextBox ID="TextBox_start" runat="server" Width="73px" Visible="False"></asp:TextBox>
&nbsp;&nbsp;
            <asp:Label ID="Label_To" runat="server" Text="iki:" Visible="False"></asp:Label>
            <asp:TextBox ID="TextBox_end" runat="server" Width="73px" Visible="False"></asp:TextBox>
&nbsp;&nbsp;
            <asp:Label ID="Label_Month" runat="server" Text="Mėnesis:" Visible="False"></asp:Label>
            <asp:TextBox ID="TextBox_month" runat="server" Width="20px" Visible="False"></asp:TextBox>
            <br />
            <asp:Button ID="Button_Search" runat="server" OnClick="Button_Search_Click" Text="Ieškoti" Visible="False" />
            <asp:Table ID="Table_Searched" runat="server">
            </asp:Table>
            <br />
            <asp:Label ID="Label_Error" runat="server" CssClass="errorLabel" Visible="False"></asp:Label>
        </div>
    </form>
</body>
</html>
