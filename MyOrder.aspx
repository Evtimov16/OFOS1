<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MyOrder.aspx.cs" Inherits="OFOS.MyOrder" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>My Order</title>
      <link href="~/StyleSheet1.css" rel="stylesheet" type="text/css" runat="server"  />
</head>
<body>
     <center>
    <form id="form1" runat="server">
           <asp:Button CssClass="b1" Text="Изход" ID="b" runat="server" OnClick="LogOut_click" />
           <asp:Button CssClass="b1" Text="Благодаря" ID="b1" runat="server" OnClick="LogOut_click" />
    
      
        <h2><asp:Label Style="float:right; color:white; margin-right:30px;" ID="l" runat="server" />
           <asp:Label ID="l2" Style="float:right; color:white;" Text="Здравей&nbsp" runat="server" />
       </h2>
      <br />
     
<br />


        <font size="7" color="#47a9c2">&emsp;&emsp;<b><u>Детайли за поръчката</u></b></font>
        <br />
        <br />
       <h2> <asp:label ID="lbl" runat="server" ForeColor="#47a9c2" /></h2>
        <br /><br />

        <asp:GridView ID="gridorder" DataSourceID="sql1" DataKeyNames="Item_no,Order_Id" 
            CssClass="gridview1"  HeaderStyle-CssClass="header" RowStyle-CssClass="row" runat="server" 
            AutoGenerateColumns="False" OnRowUpdated="gridorder_RowUpdated" OnRowUpdating="gridorder_RowUpdating">
            <Columns>
                <asp:BoundField DataField="Item_name" HeaderText="Име" ReadOnly="true" ItemStyle-HorizontalAlign="Center"  ItemStyle-Font-Bold="true" />
                <asp:BoundField DataField="Price" HeaderText="Цена" ReadOnly="true"  ItemStyle-Font-Bold="true" ItemStyle-HorizontalAlign="Center"/>
                <asp:BoundField DataField="Quantity" HeaderText="Количество"  ItemStyle-Font-Bold="true" ItemStyle-HorizontalAlign="Center"/>
                <asp:BoundField DataField="Amount" HeaderText="Сума" ReadOnly="true"  ItemStyle-Font-Bold="true" ItemStyle-HorizontalAlign="Center"/>
                <asp:BoundField DataField="Order_Id" HeaderText="Номер на поръчка" ReadOnly="true" Visible="false"/>
                <asp:BoundField DataField="Item_no" HeaderText="Item_no" ReadOnly="true" Visible="false" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button ID="edit" runat="server" Text="Промени" CommandName="Edit" Style="cursor:pointer"/>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:Button ID="update" runat="server" Text="Ъпдейт" CommandName="Update" Style="cursor:pointer"/>
                        <asp:Button ID="cancel" runat="server" Text="Отказ" CommandName="Cancel" Style="cursor:pointer"/>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button ID="delete" runat="server" Text="Изтриване" CommandName="Delete" Style="cursor:pointer" />
                    </ItemTemplate>
                </asp:TemplateField> 
              

            </Columns>
        </asp:GridView>

        <asp:SqlDataSource ID="sql1" runat="server" 
            ConnectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\ofos.mdf;Integrated Security=True"
            SelectCommand="SELECT im.Item_name,od.Price,od.Quantity,od.Amount,od.Order_Id,od.Item_no
                            FROM [dbo].[Order_Details] od INNER JOIN [dbo].[Item_Master] im 
                             ON od.Item_no=im.Item_no WHERE od.Order_Id=@Order_Id"
            UpdateCommand="UPDATE [dbo].[Order_Details] SET Quantity=@Quantity,
                            Amount = @Quantity * Price WHERE Item_no=@Item_no "
            DeleteCommand="DELETE from [dbo].[Order_Details] 
                        WHERE Item_no=@Item_no AND Order_Id=@Order_Id">
            <UpdateParameters>
                <asp:Parameter Name="Quantity" Type="Int32" />
                <asp:Parameter Name="Item_no" Type="Int32" />
            </UpdateParameters>
            
            <SelectParameters>
                <asp:SessionParameter Name="Order_Id" SessionField="order_id"  />                
            </SelectParameters>
 
        </asp:SqlDataSource>
         
        <br />
        <br />
       <h2> <b><asp:Label ID="Label1" runat="server" Text="" OnPreRender="Page_Load" ForeColor="White"></asp:Label>
        </b></h2>
        <br />
        <br />
        <br />
        <br />
        <asp:Button ID="Button1" runat="server" Text="Добави още неща" OnClick="Button1_Click" CssClass="button button2"/>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="Button2" runat="server" Text="Към плащане" OnClick="Button2_Click" CssClass="button button2" />
        &nbsp;<div style="margin-left: 200px">
            <br />
        </div>
        <br />



    </form>
        </center>
</body>
</html>
