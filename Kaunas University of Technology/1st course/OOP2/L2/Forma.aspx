<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Forma.aspx.cs" Inherits="L2.Forma" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title></title>
        <style>
            .table {
                border-color: black;
                padding: 3px;
                border-style: solid;
            }
            .table th, .table td { padding: 5px; }
        </style>
    </head>
    <body>
        <form id="form1" runat="server">
            <div>
            </div>
            <asp:Button ID="ButtonNuskaityti" runat="server" OnClick="Button1_Click" Text="Nuskaityti duomenis" Width="133px" CausesValidation="False" />
            <br />
            <br />
            <asp:Panel id="PanelIeskoti" runat="server" Visible ="False">
                <asp:Label ID="LabelPradDuom" runat="server" Font-Size="Large" Text="Pradiniai duomenys:"></asp:Label>
                <br />
                <asp:Label ID="LabelDuom1" runat="server" Text="U8a.txt"></asp:Label>
                <br />
                <asp:Label ID="LabelDuom1_1" runat="server"></asp:Label>
                <br />
                <asp:Label ID="LabelDuom1_2" runat="server"></asp:Label>
                <asp:Table ID="LenteleDuom1" runat="server" CssClass="table" GridLines="Both">
                </asp:Table>
                <br />
                <asp:Label ID="LabelDuom2" runat="server" Text="U8b.txt"></asp:Label>
                <asp:Table ID="LenteleDuom2" CssClass="table" runat="server" GridLines="Both">
                </asp:Table>
                <br />
                <asp:Label ID="LabelMaziausias" runat="server" Text="Mažiausias atstumas:"></asp:Label>
                &nbsp;&nbsp;
                <asp:TextBox ID="TextBoxMinimum" runat="server" placeholder="Mažiausias atstumas"></asp:TextBox>
                &nbsp;<asp:CustomValidator ID="CustomValidatorAtstumas" runat="server" ControlToValidate="TextBoxMinimum" ErrorMessage="Neteisingai įvestas atstumas" ForeColor="Red" OnServerValidate="CustomValidatorAtstumas_ServerValidate" ValidateEmptyText="True" ValidationGroup="Ieskoti"></asp:CustomValidator>
                <br />
                <asp:Label ID="LabelDaugiausiai" runat="server" Text="Daugiausiai gyventojų:"></asp:Label>
                &nbsp;<asp:TextBox ID="TextBoxGyvSk" runat="server" placeholder="Daugiausiai gyventojų"></asp:TextBox>
                &nbsp;<asp:CustomValidator ID="CustomValidatorGyventojai" runat="server" ControlToValidate="TextBoxGyvSk" ErrorMessage="Neteisingai įvestas gyventojų skaičius" ForeColor="Red" OnServerValidate="CustomValidatorGyventojai_ServerValidate" ValidateEmptyText="True" ValidationGroup="Ieskoti"></asp:CustomValidator>
                <br />
                <asp:Button ID="ButtonIeskoti" runat="server" Text="Ieškoti" OnClick="ButtonIeskoti_Click" ValidationGroup="Ieskoti"/>
            </asp:Panel>
                <asp:PlaceHolder ID="PlaceIeskoti" runat="server"></asp:PlaceHolder>
                <br />
                <asp:Label ID="LabelNeraRastu" runat="server" ForeColor="Red" Text="Nėra rastų maršrutų" Visible="False"></asp:Label>
                <br />
            <asp:Panel id="PanelRasti" runat="server" Visible ="False">
                <asp:Label ID="LabelRasti" runat="server" Text="Rasti maršrutai:"></asp:Label>
                <asp:Table ID="LenteleRez" runat="server" GridLines="Both" CssClass="table">
                </asp:Table>
                <asp:TextBox ID="TextBoxIsbraukti" runat="server" placeholder="Išbraukiamas miestas"></asp:TextBox>
                &nbsp;<asp:CustomValidator ID="CustomValidatorIsbraukti" runat="server" ControlToValidate="TextBoxIsbraukti" ErrorMessage="Tokio miesto saraše nėra" ForeColor="#FF6600" OnServerValidate="CustomValidatorIsbraukti_ServerValidate" ValidateEmptyText="True" ValidationGroup="Isbraukti"></asp:CustomValidator>
                <br />
                <asp:Button ID="ButtonIsbraukti" runat="server" Text="Išbraukti" OnClick="ButtonIsbraukti_Click" ValidationGroup="Isbraukti" />
            </asp:Panel>
        </form>
    </body>
</html>
