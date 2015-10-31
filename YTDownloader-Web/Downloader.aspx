<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Downloader.aspx.cs" Inherits="YTDownloader_Web.Downloader" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="Label1" runat="server" Text="Enter Youtube Link"></asp:Label><br />
        <asp:TextBox ID="txtLink" runat="server" Width="323px"></asp:TextBox><br />
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Get Direct Download Links" /><br />
        <asp:Label ID="Label2" runat="server"></asp:Label>
    </div>
    </form>
</body>
</html>
