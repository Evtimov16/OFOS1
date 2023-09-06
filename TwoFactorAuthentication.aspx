<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TwoFactorAuthentication.aspx.cs" Inherits="OFOS.TwoFactorAuthentication" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Two-Factor Authentication</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f0f0f0;
            margin: 0;
            padding: 0;
        }
        .container {
            max-width: 400px;
            margin: 0 auto;
            padding: 20px;
            background-color: #fff;
            border-radius: 5px;
            box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
            text-align: center;
        }
        h1 {
            color: #333;
            margin-bottom: 20px;
        }
        .input-group {
            margin-bottom: 15px;
        }
        .input-group label {
            display: block;
            margin-bottom: 5px;
            color: #666;
            font-size: 14px;
        }
        .input-group input[type="text"] {
            width: 100%;
            padding: 10px;
            border: 1px solid #ccc;
            border-radius: 4px;
            font-size: 16px;
        }
        .btn-verify {
            display: inline-block;
            background-color: #00ccff;
            color: #fff;
            border: none;
            border-radius: 4px;
            padding: 10px 20px;
            cursor: pointer;
            font-size: 16px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h1>Two-Factor Authentication</h1>
            <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
            <div class="input-group">
                <label for="tbTwoFactorCode">Enter Code:</label>
                <asp:TextBox ID="tbTwoFactorCode" runat="server"></asp:TextBox>
            </div>
            <asp:Button ID="btnVerify" runat="server" Text="Verify" OnClick="btnVerify_Click" CssClass="btn-verify" />
            <div id="divQRCode" runat="server" style="text-align: center;">
                <img id="qrCodeImage" runat="server" style="max-width: 100%; height: auto;" />
                </div>

    </form>
</body>
</html>
