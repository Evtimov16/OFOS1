﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Payment.aspx.cs" Inherits="OFOS.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Payment</title>
     <link href="~/StyleSheet1.css" rel="stylesheet" type="text/css" runat="server"  />

</head>
<body>
    <form id="form1" runat="server" >
        <asp:Button CssClass="b1" Text="Изход" ID="b" runat="server" OnClick="LogOut_click" />
        <asp:Button CssClass="b1" Text="Благодаря" ID="b1" runat="server" OnClick="LogOut_click" />
        <center>
         <h2><asp:Label Style="float:right; color:white; margin-right:30px;" ID="l" runat="server" />
           <asp:Label ID="l2" Style="float:right; color:white;" Text="Здравей&nbsp" runat="server" />
       </h2>
        </center>
        <br />
     <br />
      
       
       
        

        <center>&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&nbsp;<font size="7" color="#47a9c2"><b><u>PAYMENT&nbsp; DETAILS</u></b></font> 
        <br />
        <br />
        <br />
        <br />
        <div style="margin-left: 40px">
            <h3><asp:Label ID="Label1" runat="server" Text="" ForeColor="#47a9c2"></asp:Label></h3>
        </div>
        <h1><font color="#47a9c2">Final Order List</font></h1>
        <asp:GridView ID="gridorder" DataSourceID="sql1" DataKeyNames="Item_no,Order_Id" 
            CssClass="gridview1" HeaderStyle-CssClass="header" RowStyle-CssClass="row" runat="server" 
            AutoGenerateColumns="False" HeaderStyle-Font-Bold="true" >
            <Columns>
                <asp:BoundField DataField="Item_name" HeaderText="ITEM NAME" ReadOnly="true" ItemStyle-Font-Bold="true" />
                <asp:BoundField DataField="Price" HeaderText="PRICE" ReadOnly="true" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Bold="true" />
                <asp:BoundField DataField="Quantity" HeaderText="QUANTITY" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Bold="true"/>
                <asp:BoundField DataField="Amount" HeaderText="AMOUNT" ReadOnly="true" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Bold="true"/>
                <asp:BoundField DataField="Order_Id" HeaderText="Order_Id" ReadOnly="true" Visible="false"/>
                <asp:BoundField DataField="Item_no" HeaderText="Item_no" ReadOnly="true" Visible="false" />
      
            </Columns>
        </asp:GridView>

        <asp:SqlDataSource ID="sql1" runat="server" 
            ConnectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\ofos.mdf;Integrated Security=True"
            SelectCommand="SELECT im.Item_name,od.Price,od.Quantity,od.Amount,od.Order_Id,od.Item_no
                            FROM [dbo].[Order_Details] od INNER JOIN [dbo].[Item_Master] im 
                             ON od.Item_no=im.Item_no WHERE od.Order_Id=@Order_Id">
           <SelectParameters>
                <asp:SessionParameter Name="Order_Id" SessionField="order_id"  />           
        </SelectParameters>
 
        </asp:SqlDataSource>
        </center>


        <p>
            &nbsp;</p>
        <p>
            &nbsp;</p>
        <h3><b>
        <p style="margin-left: 40px" >
           <font color="#47a9c2" size="5">Total Amount:</font>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Label ID="Label2" runat="server" Text="Label" ForeColor="White"></asp:Label>
        </p>
        <p style="margin-left: 40px">
         <font color="#47a9c2" size="4">PAY  BY:</font></p>
</b></h3>

        <p style="margin-left: 120px">
            <asp:Button ID="Button1" runat="server" Text="Онлайн транзакция" OnClick="Button1_Click" 
                 CssClass="button button2" Height="52px" Width="248px" Visible="False" />
        </p>
        <p style="margin-left: 120px">
            <asp:Button ID="Button2" runat="server" Text="Плащане в брой"  Width="248px" 
                OnClick="Button2_Click" Height="52px" CssClass="button button2" />
        </p>
    </form>
</body>
</html>
