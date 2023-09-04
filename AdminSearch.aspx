<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminSearch.aspx.cs" Inherits="OFOS.AdminSearch" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <meta charset="utf-8"/>
    <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
    <meta name="viewport" content="width=device-width, shrink-to-fit=yes, initial-scale=1"/>
    <meta name="description" content=""/>
    <meta name="author" content=""/>
    
    <title>Admin Search</title>

    <!-- Bootstrap Core CSS -->
    <link href="sidebar/css/bootstrap.min.css" rel="stylesheet"/>

    <!-- Custom CSS -->
    <link href="sidebar/css/simple-sidebar.css" rel="stylesheet"/>

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
    <center>
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
            <asp:Button ID="btnSearch" runat="server" Text="Търсене" OnClick="btns_Click" />
        </p>
             
            <div id="calendar" runat="server">
                <center><h1>SEARCH FOR PLACED ORDERS</h1></center>
                <br /><br />
                <center><h2>SEARCH BY:</h2></center>
                <br />
                <asp:Label ID="lbl_city" runat="server" Text="Град:" Font-Bold="true" Font-Size="Medium" Font-Names="Georgia"/>
                <asp:DropDownList ID="dropdown_city" runat="server" >
                <asp:ListItem>Burgas</asp:ListItem>
                <asp:ListItem>Несебър</asp:ListItem>
                <asp:ListItem>Варна</asp:ListItem>
                </asp:DropDownList>
                &emsp;&emsp;&emsp;&emsp;&emsp;
               <asp:Label ID="lbl_user" runat="server" Text="Потребител:" Font-Bold="true" Font-Size="Medium" Font-Names="Georgia"/>
                    <asp:DropDownList ID="ddlUserType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlUserType_SelectedIndexChanged">
                    <asp:ListItem Text="Всички" Value="All" />
                    <asp:ListItem Text="Регистриран потребител" Value="RegisteredUser" />
                    <asp:ListItem Text="Гост" Value="Guest" />
                    </asp:DropDownList>


                <asp:Label ID="lbl_dt" runat="server" Text="Дата:" Font-Bold="true" Font-Size="Medium" Font-Names="Georgia"/>
                <br />
                <asp:Label ID="label" runat="server" Text="Избери дата за която искаш да провериш направените поръчки:"></asp:Label>
                <br /><br />
                <asp:Calendar ID="clndr" runat="server" BackColor="White" BorderColor="Black" DayNameFormat="Shortest" Font-Names="Times New Roman" Font-Size="10pt" ForeColor="Black" Height="220px" NextPrevFormat="FullMonth" TitleFormat="Month" Width="400px" AutoPostBack="True" >
                   
                    <DayHeaderStyle BackColor="#CCCCCC" Font-Bold="True" Font-Size="7pt" ForeColor="#333333" Height="10pt" />
                    <DayStyle Width="14%" />
                    <NextPrevStyle Font-Size="8pt" ForeColor="White" />
                    <OtherMonthDayStyle ForeColor="#999999" />
                    <SelectedDayStyle BackColor="#CC3333" ForeColor="White" />
                    <SelectorStyle BackColor="#CCCCCC" Font-Bold="True" Font-Names="Verdana" Font-Size="8pt" ForeColor="#333333" Width="1%" />
                    <TitleStyle BackColor="Black" Font-Bold="True" Font-Size="13pt" ForeColor="White" Height="14pt" />
                    <TodayDayStyle BackColor="#CCCC99" />
                    

                </asp:Calendar>
                <asp:LinkButton ID="lnkSelectToday" runat="server" Text="Избери текуща дата" OnClick="lnkSelectToday_Click"></asp:LinkButton>


               
                <asp:Label ID="status" runat="server" Text="" Visible="false"></asp:Label>
                <br />
                
            </div>
            <div id="details" runat="server" visible="false">
                <br /><br />
		        <asp:GridView id="gridview_order_details" Runat="server" AutoGenerateColumns="true"
                    HeaderStyle-ForeColor="#3DFF33" CellSpacing="10" CellPadding="12" 
             PagerStyle-CssClass="gridViewPager" PagerStyle-HorizontalAlign="Center" Width="270px" EnableViewState="true" Visible="true" />
                <br /><br />

                <b>Order Details</b>
                <br />
                
		        <asp:GridView ID="gridview_orders" runat="server" AutoGenerateColumns="true" OnRowDataBound="gridview1_RowDataBound"
            HeaderStyle-ForeColor="#3DFF33" CellSpacing="10" CellPadding="12" 
             PagerStyle-CssClass="gridViewPager" PagerStyle-HorizontalAlign="Center" Width="270px" EnableViewState="true" Visible="true" >
                    
                    <HeaderStyle ForeColor="#3DFF33" />
                    <PagerStyle CssClass="gridViewPager" HorizontalAlign="Center" />
                </asp:GridView>
                    
		        <br />
                

                <asp:Label ID="Sts" runat="server" Text="" Visible="false"></asp:Label>

	        </div>

                   </form> 

                    </div>
                </div>
            </div>
        </div>
        <!-- /#page-content-wrapper -->

    
    <!-- /#wrapper -->

    <!-- jQuery -->
    <script src="sidebar/js/jquery.js"></script>

    <!-- Bootstrap Core JavaScript -->
    <script src="sidebar/js/bootstrap.min.js"></script>

    
    </center>
</body>
</html>

