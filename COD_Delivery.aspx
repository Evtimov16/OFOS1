<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="COD_Delivery.aspx.cs" Inherits="OFOS.COD" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>COD Delivery</title>
    <link href="~/StyleSheet1.css" rel="stylesheet" type="text/css" runat="server" />
    <style>
        input[type=text] {
            padding: 12px 20px;
            margin: 8px 0;
            box-sizing: border-box;
            border-radius: 1px;
            background: #ffffff;
        }
    </style>
   <script type="text/javascript">
       function toggleDeliveryForm() {
           var deliveryMethod = document.getElementById('<%= RadioButtonList1.ClientID %>').value;
           var deliveryForm = document.getElementById('deliveryForm');
           var pickupForm = document.getElementById('pickupForm');

           if (deliveryMethod === 'Delivery') {
               deliveryForm.style.display = 'block';
               pickupForm.style.display = 'none';
           } else if (deliveryMethod === 'Pickup') {
               deliveryForm.style.display = 'none';
               pickupForm.style.display = 'block';
           }
       }
</script>

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>
                <asp:Label Style="float: right; color: white; margin-right: 30px;" ID="l" runat="server" />
                <asp:Label ID="l2" Style="float: right; color: white;" Text="Здравей&nbsp" runat="server" />
            </h2>
            <br />
            <br />
            <center>&emsp;&emsp;&emsp;&emsp;<font size="7" color="#47a9c2"><b><u>CASH ON DELIVERY</u></b></font></center>
            <br />
            <br />
            <center>
                <h3><b><asp:Label ID="Label1" runat="server" Text="" ForeColor="White"></asp:Label></b></h3>
                <br />
            </center>
            <font color="#47a9c2">
                <p>&nbsp;</p>
                <p style="margin-left: 40px">
                     DELIVERY METHOD :&emsp;
                          <asp:RadioButtonList ID="RadioButtonList1" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="RadioButtonList1_SelectedIndexChanged">
    <asp:ListItem Text="Доставка" Value="Delivery" />
    <asp:ListItem Text="Ще взема от място" Value="Pickup" />
</asp:RadioButtonList>
                </p>

                <asp:Panel ID="DeliveryPanel" runat="server" Visible="false">
                    <p style="margin-left: 40px">
                        NAME :&emsp;&emsp;&emsp;&emsp;&nbsp;&nbsp;
                        <asp:TextBox ID="Name" runat="server" Width="240px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Name must be provided"
                            Display="Dynamic" ControlToValidate="Name" ForeColor="Red" Font-Size="Medium"></asp:RequiredFieldValidator>
                    </p>
                    <p style="margin-left: 40px">
                        HOUSE NO :&emsp;&emsp;&nbsp;
                        <asp:TextBox ID="House_no" runat="server" Width="240px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="House no must be provided"
                            Display="Dynamic" ControlToValidate="House_no" ForeColor="Red" Font-Size="Medium"></asp:RequiredFieldValidator>
                    </p>
                    <p style="margin-left: 40px">
                        STREET :&emsp;&emsp;&emsp;&nbsp;&nbsp;
                        <asp:TextBox ID="Street" runat="server" Width="240px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Please provide the street"
                            Display="Dynamic" ControlToValidate="Street" ForeColor="Red" Font-Size="Medium"></asp:RequiredFieldValidator>
                    </p>
                    <p style="margin-left: 40px">
                        CITY :&emsp;&emsp;&emsp;&emsp;&emsp;
                        <asp:DropDownList ID="D_city" runat="server" style="margin-left: 0px" Width="164px" Height="44px" BackColor="#ffffff">
                            <asp:ListItem>Бургас</asp:ListItem>
                            <asp:ListItem>Варна</asp:ListItem>
                            <asp:ListItem>Несебър</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Enter the city"
                            Display="Dynamic" ControlToValidate="D_city" ForeColor="Red" Font-Size="Medium"></asp:RequiredFieldValidator>
                    </p>
                    <p style="margin-left: 40px">
                        CONTACT NO :&nbsp;&nbsp;&nbsp;
                        <asp:TextBox ID="Contact" runat="server" Width="240px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Enter your contact number"
                          Display="Dynamic" ControlToValidate="TextBox2" ForeColor="Red" Font-Size="Medium"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="TextBox2"
                           ValidationExpression="^\d{10}$" ErrorMessage="Моля въведете валиден телефонен номер (поне 10 символа)"
                            ForeColor="Red" Font-Size="Small" />
                    </p>
                    <p style="margin-left: 40px">
    PICKUP TIME :&emsp;&nbsp;
    <asp:DropDownList ID="TimeDropDownList" runat="server" Width="164px" Height="44px" BackColor="#ffffff">
        <asp:ListItem Text="Възможно най-скоро" Value="As soon as possible" />
    </asp:DropDownList>
    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="Select pickup time"
        Display="Dynamic" ControlToValidate="TimeDropDownList" ForeColor="Red" Font-Size="Medium"></asp:RequiredFieldValidator>
</p>


                </asp:Panel>
               <asp:Panel ID="PickupPanel" runat="server" Visible="false">
                <p style="margin-left: 40px">
                        NAME :&emsp;&emsp;&emsp;&emsp;&nbsp;&nbsp;
                        <asp:TextBox ID="TextBox1" runat="server" Width="240px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Name must be provided"
                            Display="Dynamic" ControlToValidate="Name" ForeColor="Red" Font-Size="Medium"></asp:RequiredFieldValidator>
                    </p>
                    <p style="margin-left: 40px">
                        CITY :&emsp;&emsp;&emsp;&emsp;&emsp;
                        <asp:DropDownList ID="DropDownList1" runat="server" style="margin-left: 0px" Width="164px" Height="44px" BackColor="#ffffff">
                            <asp:ListItem>Бургас</asp:ListItem>
                            <asp:ListItem>Варна</asp:ListItem>
                            <asp:ListItem>Несебър</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="Enter the city"
                            Display="Dynamic" ControlToValidate="D_city" ForeColor="Red" Font-Size="Medium"></asp:RequiredFieldValidator>
                    </p>
                    <p style="margin-left: 40px">
                        CONTACT NO :&nbsp;&nbsp;&nbsp;
                        <asp:TextBox ID="TextBox2" runat="server" Width="240px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="Enter your contact number"
                          Display="Dynamic" ControlToValidate="TextBox2" ForeColor="Red" Font-Size="Medium"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="rev_contact2" runat="server" ControlToValidate="TextBox2"
                           ValidationExpression="^\d{10}$" ErrorMessage="Моля въведете валиден телефонен номер (поне 10 символа)"
                            ForeColor="Red" Font-Size="Small" />
                    </p>
                  <p style="margin-left: 40px">
    PICKUP TIME :&emsp;&nbsp;
    <asp:DropDownList ID="PickupTimeDropDownList" runat="server" Width="164px" Height="44px" BackColor="#ffffff">
        <asp:ListItem Text="Възможно най-скоро" Value="As soon as possible" />
    </asp:DropDownList>
    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="Select pickup time"
        Display="Dynamic" ControlToValidate="PickupTimeDropDownList" ForeColor="Red" Font-Size="Medium"></asp:RequiredFieldValidator>
</p>



                </asp:Panel>
                <br />
                <p style="margin-left: 320px">
                    &emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;
                    <asp:Button ID="Button2" runat="server" Text="Достави" OnClick="Button2_Click" CssClass="button button2" />
                </p>
            </font>
        </div>
    </form>
</body>
</html>
