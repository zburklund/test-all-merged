<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="CASP.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="ErrorLbl" runat ="server"></asp:Label>
        <br />
        <asp:Button id="SignoutBtn" runat="server" Text="Sign out of SSO" OnClick="SignoutBtn_Click"/>
    </div>
    </form>
</body>
</html>
