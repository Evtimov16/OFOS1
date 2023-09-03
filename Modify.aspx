<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Modify.aspx.cs" Inherits="OFOS.Modify" %>

<!DOCTYPE html>
<html lang="en">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
      <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, shrink-to-fit=yes, initial-scale=1">
    <meta name="description" content="">
    <meta name="author" content="">
    
    <title>Modify</title>

    <!-- Bootstrap Core CSS -->
    <link href="sidebar/css/bootstrap.min.css" rel="stylesheet">

    <!-- Custom CSS -->
    <link href="sidebar/css/simple-sidebar.css" rel="stylesheet">

    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
        <script src="https://oss.maxcdn.com/libs/html5shiv/3.7.0/html5shiv.js"></script>
        <script src="https://oss.maxcdn.com/libs/respond.js/1.4.2/respond.min.js"></script>
    <![endif]-->
    <style type="text/css">
        th
        {
            text-align:center;
            font-size:medium;

        }

        gridViewPager td
        {
            padding:0 10px;
            width:auto;
        }

        
    </style>
</head>
<body>
    <div id="wrapper">

        <!-- Sidebar -->
        <div id="sidebar-wrapper">
            
            <ul class="sidebar-nav">
               
                <li class="sidebar-brand">
                    
                         <font color="#3DFF33"><u><b>ADMIN FEATURES</b></u></font>
                   
                </li>
                <li>
                    <a href="Update_Menu.aspx">Update Menu</a>
                </li>
                <li>
                    <a href="Manage_COD.aspx">Manage COD</a>
                </li>
                <li>
                    <a href="Review_fb.aspx">Review Feedback</a>
                </li>
                <li>
                    <a href="AdminSearch.aspx">Search</a>
                </li>
            </ul>
        </div>
        <!-- /#sidebar-wrapper -->

        <!-- Page Content -->
        <div id="page-content-wrapper">
            <div class="container-fluid">
                <div class="row">
                    <div class="col-lg-12">
                         
         <form id="form1" runat="server">
             
        <p style="margin-left:950px">
        <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click">LogOut</asp:LinkButton>
             
        </p>
        
   
   
        <div>
        <br />
        <center><h1>MODIFY EXISTING ITEM(S)</h1></center>

    
        <p>
            &nbsp;</p>
        <p>
            &nbsp;</p>
                <asp:GridView ID="GridView1" runat="server" CssClass="table"  AutoGenerateColumns="False" 
            DataSourceID="SqlDataSource1" DataKeyNames="Item_no" AllowPaging="true" PageSize="9" 
            HeaderStyle-ForeColor="#3DFF33" CellSpacing="10" CellPadding="12" 
            PagerStyle-CssClass="gridViewPager" PagerStyle-HorizontalAlign="Center" >
            <Columns>
                <asp:BoundField DataField="Item_no" HeaderText="ITEM NO" ReadOnly="True" SortExpression="Item_no" />
                <asp:BoundField DataField="Item_name" HeaderText="ITEM NAME" SortExpression="Item_name" />
                <asp:BoundField DataField="Price" HeaderText="PRICE" SortExpression="Price" />
                <asp:BoundField DataField="Type" HeaderText="TYPE" SortExpression="Type" />
                <asp:BoundField DataField="Description" HeaderText="DESCRIPTION" SortExpression="Description" />
                <asp:BoundField DataField="Image_url" HeaderText="IMAGE URL"  SortExpression="Image_url"/>
                <asp:TemplateField HeaderText="IS ACTIVE">
                    <ItemTemplate>
                        <asp:CheckBox ID="chkIsActive" runat="server" Checked='<%# Eval("IsActive") != DBNull.Value ? Convert.ToBoolean(Eval("IsActive")) : false %>' 
                            AutoPostBack="true" OnCheckedChanged="chkIsActive_CheckedChanged" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:CommandField ShowEditButton="true" ShowDeleteButton="true" />
            </Columns>
        </asp:GridView>  
        
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
            ConnectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\ofos.mdf;Integrated Security=True" 
            SelectCommand="SELECT * FROM [Item_Master]" 
            UpdateCommand="UPDATE [Item_Master] SET Item_name=@Item_name, Price=@Price, 
            Description=@Description, Image_url=@Image_url, Type=@Type, IsActive=@IsActive WHERE Item_no=@Item_no" 
            DeleteCommand="UPDATE [Item_Master] SET IsActive=0 WHERE Item_no=@Item_no">
            <UpdateParameters>
         <asp:Parameter Name="IsActive" Type="Boolean" />
     </UpdateParameters>
 </asp:SqlDataSource>

    </div>
    </form>
                             </div>
                </div>
            </div>
        </div>
        <!-- /#page-content-wrapper -->

    </div>
    <!-- /#wrapper -->

    <!-- jQuery -->
    <script src="sidebar/js/jquery.js"></script>

    <!-- Bootstrap Core JavaScript -->
    <script src="sidebar/js/bootstrap.min.js"></script>

</body>
</html>

