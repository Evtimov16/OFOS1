<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FoodItems.aspx.cs" Inherits="OFOS.FoodItems" %>

<!DOCTYPE html>

<html lang="en" xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">


    <title>Home Page</title>
 
    <link href="StyleSheet1.css" rel="stylesheet" type="text/css" runat="server"  />
</head>
<body>
   
    <center>
        <h1><asp:Label Style="color:#47a9c2;" ID="status" runat="server" Visible="false" /></h1>
        <h1><asp:Label Style="color:#47a9c2;" ID="status1" runat="server" Visible="false" /></h1>
    </center>           

    <div id="home" runat="server" >
        <form id="form1" runat="server">

        <asp:Button CssClass="b1" Text="Изход" ID="b" runat="server" OnClick="LogOut_click" />
        <asp:Button CssClass="b1" Text="Благодаря" ID="b1" runat="server" OnClick="LogOut_click" />

            <div class="dropdown" runat="server" visible="false" id="dropdown" style="float:right">
                <button class="dropbtn">
                    <asp:Label Style="float:right; color:white" ID="u" runat="server" Font-Bold="true" />
                    <asp:Label ID="h" Style="float:right; color:white" Text="Здравей&nbsp" runat="server" Font-Bold="true" />
                </button>
                <div class="dropdown-content">
                    <a href="MyAccount.aspx">Моят акаунт</a>
                    

                    <a href="Feedback.aspx">Обратна връзка</a>
                </div>
            </div>

       <asp:Button CssClass="b1"  id="my_order" Text="Моята поръчка" OnClick="MyOrder_click" runat="server" Visible="false" />
       <asp:Button CssClass="b1"  id="hl" Text="Вход" OnClick="signin_click" runat="server" />
       <asp:Button ID="Register" runat="server" Text="Регистрация" Visible="false"  CssClass="b1" OnClick="Register_Click" />
         <h2><asp:Label Style="float:right; color:white; margin-right:30px;" ID="Label1" runat="server" Font-Bold="true" Visible="false" />
             <asp:Label ID="Label2" Style="float:right; color:white" Text="Здравей&nbsp" runat="server" Font-Bold="true" Visible="false" />
             
         </h2>      
            <div style="display: flex; align-items: center;">
    <asp:Image ID="i" runat="server" ImageUrl="PIC\logo.png" Width="250px"  Height="150px"/>
    <div class="centered-link">
    <a href="tel:+359883641526">
        <img src="https://w7.pngwing.com/pngs/826/886/png-transparent-iphone-computer-icons-telephone-call-phone-call-icon-blue-call-icon-miscellaneous-text-logo.png" alt="+359883641526" class="phone-image" />
    </a>
</div>


</div>
       <center>&emsp;&emsp;
        <h1><asp:Label Style="color:white; margin-left:100px; font-variant-caps:small-caps;"  ID="sizlr" runat="server" Visible="false" /></h1>
           
       </center>
            
       <br /><br />

       <p class="text-slide"><asp:Button CssClass="b" ID="Button_soup" runat="server" Text="Супи" OnClick="Button_soup_onclick"/></p>
       <p class="text-slide"><asp:Button CssClass="b" ID="Button_starter" runat="server" Text="Предястия" OnClick="Button_starter_onclick"/></p>
      <p class="text-slide"><asp:Button CssClass="b" ID="Button_salads" runat="server" Text="Салати" OnClick="Button_salads_onclick"/></p>
       <p class="text-slide"><asp:Button CssClass="b" ID="Button_sea" runat="server" Text="Морска храна" OnClick="Button_sea_onclick"/></p>
       <p class="text-slide"><asp:Button CssClass="b" ID="Button_MainCourse" runat="server" Text="Основни ястия" OnClick="Button_maincourse_onclick"/></p>
      <p class="text-slide"><asp:Button CssClass="b" ID="Button_dessert" runat="server" Text="Десерти" OnClick="Button_dessert_onclick"/></p>
            
       <br /><br /><br />
     <center>
        <div class="w3-content w3-section" style="max-width:550px" id="pic" runat="server">
            <br />
            <center>
        <img class="mySlides w3-animate-fading" src="PIC/c_v.jpg"  height="450px" width="550px" />
        <img class="mySlides w3-animate-fading" src="PIC/man.jpg" height="450px" width="550px" />
        <img class="mySlides w3-animate-fading" src="PIC/szh_f.JPG" height="450px" width="550px" />
        <img class="mySlides w3-animate-fading" src="PIC/rice.PNG" height="450px" width="550px" />
        <img class="mySlides w3-animate-fading" src="PIC/chop.PNG" height="450px" width="550px" />
        <img class="mySlides w3-animate-fading" src="PIC/schewan-vegetables.jpg" height="450px" width="550px" />
        <img class="mySlides w3-animate-fading" src="PIC/choco.jpg" height="450px" width="550px" />
        <img class="mySlides w3-animate-fading" src="PIC/manchurian.jpg" height="450px" width="550px" />
         <img class="mySlides w3-animate-fading" src="PIC/noodles.jpg" height="450px" width="550px" />
                </center>
        </div>
         
        <script>
            var myIndex = 0;
            carousel();

            function carousel() {
                var i;
                var x = document.getElementsByClassName("mySlides");
                for (i = 0; i < x.length; i++) {
                    x[i].style.display = "none";
                }
                myIndex++;
                if (myIndex >= x.length) { myIndex = 0; }
                x[myIndex].style.display = "block";
                setTimeout(carousel, 3000);
            }
        </script>


        </center>
      <asp:GridView ID="griditem" CssClass="gridview" HeaderStyle-CssClass="header" 
    RowStyle-CssClass="row" runat="server" AutoGenerateColumns="false"
    CellSpacing="10" CellPadding="12" HeaderStyle-Font-Bold="true">
    <Columns>
        <asp:BoundField DataField="Item_no" HeaderText="Номер" ReadOnly="true" SortExpression="Item_no" ItemStyle-CssClass="item_no" HeaderStyle-CssClass="header-hide-mobile" />
        <asp:BoundField DataField="Item_name" HeaderText="Име" ReadOnly="true" SortExpression="Name"  ItemStyle-Font-Bold="true" />
        <asp:BoundField DataField="Description" HeaderText="Описание"  ReadOnly="true" SortExpression="Description" ItemStyle-CssClass="item_no description" HeaderStyle-CssClass="header-hide-mobile" />
        <asp:ImageField DataImageUrlField="Image_url" ControlStyle-Width="150" 
            ControlStyle-Height="150" HeaderText="Снимка"  ItemStyle-HorizontalAlign="Center"/>
        <asp:BoundField DataField="Price" HeaderText="Цена" ItemStyle-HorizontalAlign="Center" 
            ReadOnly="true" SortExpression="Price" ItemStyle-Font-Bold="true" ItemStyle-Font-Size="Large"/>
        <asp:TemplateField HeaderText="Грамаж" ItemStyle-HorizontalAlign="Center">
            <ItemTemplate>
                <asp:Label ID="lblWeight" runat="server" Text='<%# Eval("Weight") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Количество" ItemStyle-HorizontalAlign="Center"  ItemStyle-Font-Bold="true">
            <ItemTemplate>
               <asp:TextBox ID="tb_quantity" runat="server" TextMode="number" Width="70" />
        <asp:RangeValidator ID="RangeValidator1" runat="server" 
            ControlToValidate="tb_quantity" Type="Integer" MinimumValue="0" MaximumValue="10"
            ErrorMessage="Грешка: Въведете число между 0 и 10." />                                         
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <asp:Label ID="l1" Text="Добавено към поръчката" runat="server"  Visible="false"/>
                <asp:Button ID="button_cart" runat="server" Text="Добави към поръчката" 
                    OnClick="griditem_Click" CssClass="button button3" />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>

   
    </form>
        </div>
</body>
</html>

