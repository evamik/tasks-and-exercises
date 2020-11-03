<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Forma.aspx.cs" Inherits="L2.Forma" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title></title>
        <link href="Stilius.css" rel="stylesheet" />
    </head>
    <body>
        <form id="form1" runat="server">
            <div>
                <table class="auto-style1">
                    <tr>
                        <td>
                            <asp:Label ID="LabelFailas1" runat="server" Text="Maršrutų duomenys:"></asp:Label>
&nbsp;<asp:FileUpload ID="Failas1" runat="server" />
&nbsp;<asp:CustomValidator ID="CustomValidator_Failas1" runat="server" ControlToValidate="Failas1" ErrorMessage="Pasirinkite failą" ForeColor="Red" OnServerValidate="CustomValidator_Failas1_ServerValidate" ValidateEmptyText="True" ValidationGroup="Nuskaitymas"></asp:CustomValidator>
                            <br />
                            <asp:Label ID="LabelFailas2" runat="server" Text="Miestų duomenys:"></asp:Label>
&nbsp;<asp:FileUpload ID="Failas2" runat="server" />
&nbsp;<asp:CustomValidator ID="CustomValidator_Failas2" runat="server" ControlToValidate="Failas2" ErrorMessage="Pasirinkite failą" ForeColor="Red" OnServerValidate="CustomValidator_Failas2_ServerValidate" ValidateEmptyText="True" ValidationGroup="Nuskaitymas"></asp:CustomValidator>
                            <br />
            <asp:Button ID="ButtonNuskaityti" runat="server" OnClick="Button1_Click" Text="Nuskaityti duomenis" Width="133px" ValidationGroup="Nuskaitymas" />
                        </td>
                        <td>
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/Paveiksliukai/Paveiksliukas.jpg" />
                        </td>
                    </tr>
                    <tr runat="server" id="DuomenuEilute">
                        <td colspan="2">
                <asp:Label ID="LabelPradDuom" runat="server" Font-Size="Large" Text="Pradiniai duomenys:"></asp:Label>
                            <br />
                <asp:Label ID="LabelDuom1" runat="server" Text="U8a.txt"></asp:Label>
                            <br />
                <asp:Label ID="LabelDuom1_1" runat="server"></asp:Label>
                            <br />
                <asp:Label ID="LabelDuom1_2" runat="server"></asp:Label>
                <asp:Table ID="LenteleDuom1" runat="server" CssClass="table">
                </asp:Table>
                            <br />
                <asp:Label ID="LabelDuom2" runat="server" Text="U8b.txt"></asp:Label>
                <asp:Table ID="LenteleDuom2" CssClass="table" runat="server">
                </asp:Table>
                            <br />
                <asp:Label ID="LabelMaziausias" runat="server" Text="Mažiausias atstumas:"></asp:Label>
                <asp:TextBox ID="TextBoxMinimum" runat="server" placeholder="Mažiausias atstumas"></asp:TextBox>
                            <asp:CustomValidator ID="CustomValidatorAtstumas" runat="server" ControlToValidate="TextBoxMinimum" ErrorMessage="Neteisingai įvestas atstumas" ForeColor="Red" OnServerValidate="CustomValidatorAtstumas_ServerValidate" ValidateEmptyText="True" ValidationGroup="Ieskoti"></asp:CustomValidator>
                            <br />
                <asp:Label ID="LabelDaugiausiai" runat="server" Text="Daugiausiai gyventojų:"></asp:Label>
                            <asp:TextBox ID="TextBoxGyvSk" runat="server" placeholder="Daugiausiai gyventojų"></asp:TextBox>
                            <asp:CustomValidator ID="CustomValidatorGyventojai" runat="server" ControlToValidate="TextBoxGyvSk" ErrorMessage="Neteisingai įvestas gyventojų skaičius" ForeColor="Red" OnServerValidate="CustomValidatorGyventojai_ServerValidate" ValidateEmptyText="True" ValidationGroup="Ieskoti"></asp:CustomValidator>
                            <br />
                <asp:Button ID="ButtonIeskoti" runat="server" Text="Ieškoti" OnClick="ButtonIeskoti_Click" ValidationGroup="Ieskoti"/>
                            <br />
                            <br />
                <asp:Label ID="LabelNeraRastu" runat="server" ForeColor="Red" Text="Nėra rastų maršrutų" Visible="False"></asp:Label>
                        </td>
                    </tr>
                    <tr runat="server" id="RezultatuEilute">
                        <td colspan="2">
                <asp:Label ID="LabelRasti" runat="server" Text="Rasti maršrutai:"></asp:Label>
                            <br />
                <asp:Table ID="LenteleRez" runat="server" CssClass="table">
                </asp:Table>
                <asp:TextBox ID="TextBoxIsbraukti" runat="server" placeholder="Išbraukiamas miestas"></asp:TextBox>
                <asp:Button ID="ButtonIsbraukti" runat="server" Text="Išbraukti" OnClick="ButtonIsbraukti_Click" ValidationGroup="Isbraukti" />
                        </td>
                    </tr>
                </table>
            </div>
            <br />
            <br />
                <br />
                <br />
        </form>
    </body>
</html>
