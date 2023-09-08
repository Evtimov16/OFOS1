using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace OFOS
{
    public partial class AdminSearch : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            Debug.WriteLine("Page_Load executed.");
            if (Session["admin"] == null)
            {
                Response.Redirect("Admin_Login.aspx?msg=You need to login first");
            }

            if (!IsPostBack)
            {
                if (Cache["OrderData"] != null)
                {
                    DataTable orderData = (DataTable)Cache["OrderData"];
                    gridview_orders.DataSource = orderData;
                    gridview_orders.DataBind();
                }
                dropdown_city.SelectedIndex = 0;
                ddlUserType.SelectedIndex = 0;
            }
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            Session["admin"] = null;
            Response.Redirect("Admin_Login.aspx");
        }

        protected void btns_Click(object sender, EventArgs e)
        {
            details.Visible = true;

            string constr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\ofos.mdf;Integrated Security=True";
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("GetOrderInformation", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@city", dropdown_city.SelectedItem.Text);

                if (ddlUserType.SelectedValue == "All")
                {
                    
                    // За "Всички" използвайте две заявки с различни стойности на @passwordType
                    SqlCommand cmdAll = new SqlCommand("GetOrderInformation", con);
                    cmdAll.CommandType = CommandType.StoredProcedure;
                    cmdAll.Parameters.AddWithValue("@city", dropdown_city.SelectedItem.Text);
                    cmdAll.Parameters.AddWithValue("@passwordType", 1); // Регистрирани
                    cmdAll.Parameters.AddWithValue("@date", (clndr.SelectedDate.Date != DateTime.MinValue.Date) ? clndr.SelectedDate.Date : (object)DBNull.Value);

                    SqlCommand cmdGuests = new SqlCommand("GetOrderInformation", con);
                    cmdGuests.CommandType = CommandType.StoredProcedure;
                    cmdGuests.Parameters.AddWithValue("@city", dropdown_city.SelectedItem.Text);
                    cmdGuests.Parameters.AddWithValue("@passwordType", 0); // Гости
                    cmdGuests.Parameters.AddWithValue("@date", (clndr.SelectedDate.Date != DateTime.MinValue.Date) ? clndr.SelectedDate.Date : (object)DBNull.Value);

                   
                    var a = cmdAll.ExecuteReader();
                    DataTable newOrderData = new DataTable();
                    newOrderData.Load(a);

                    if (ddlUserType.SelectedValue == "All")
                    {
                        a.Close(); // Затваряме първия DataReader
                        var b = cmdGuests.ExecuteReader();
                        newOrderData.Load(b); // Зареждаме данните от втория DataReader
                        b.Close(); // Затваряме втория DataReader

                    }

                    gridview_orders.DataSource = newOrderData;
                    gridview_orders.DataBind();

                    // Кеширане на новите данни за следващите рефрешове
                    Cache["OrderData"] = newOrderData;
                }
                else
                {
                    int userTypeValue;
                    if (ddlUserType.SelectedValue == "All")
                    {
                        userTypeValue = 2;
                    }
                    else if (ddlUserType.SelectedValue == "RegisteredUser")
                    {
                        userTypeValue = 1;
                    }
                    else
                    {
                        userTypeValue = 0;
                    }

                    cmd.Parameters.AddWithValue("@passwordType", userTypeValue);
                    if (clndr.SelectedDate.Date != DateTime.MinValue.Date)
                    {
                        DateTime selectedDate = clndr.SelectedDate;
                        DateTime startDate = selectedDate.Date;
                        DateTime endDate = startDate.AddDays(1).AddSeconds(-1);

                        cmd.Parameters.AddWithValue("@date", startDate);

                        
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@date", DBNull.Value);
                    }

                    var a = cmd.ExecuteReader();

                    Debug.WriteLine("A=" + a.HasRows);
                    Debug.WriteLine("City:" + dropdown_city);
                    Debug.WriteLine("Password:");
                    Debug.WriteLine("Date" + clndr.SelectedDate.ToString("dd.MM.yyyy г. H:mm:ss"));

                    // Зареждане на новите данни и кеширане
                    DataTable newOrderData = new DataTable();
                    newOrderData.Load(a);
                    
                    gridview_orders.DataSource = newOrderData;
                    gridview_orders.DataBind();

                    // Кеширане на новите данни за следващите рефрешове

                    Cache["OrderData"] = newOrderData;
                }
            }
        }
        protected void clndr_SelectionChanged(object sender, EventArgs e)
        {
            DateTime selectedDate = clndr.SelectedDate;
            PopulateOrdersForSelectedDate(selectedDate);
            Debug.WriteLine("SelectionChanged executed." + selectedDate);
        }
        protected void lnkSelectToday_Click(object sender, EventArgs e)
        {
            clndr.SelectedDate = DateTime.Today;
            PopulateOrdersForToday();
            Debug.WriteLine("SelectToday executed.");
        }
        private void PopulateOrdersForToday()
        {
            string constr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\ofos.mdf;Integrated Security=True";

            using (SqlConnection con = new SqlConnection(constr))
            {
                try
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand("GetOrderInformation", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@city", dropdown_city.SelectedItem.Text);
                    cmd.Parameters.AddWithValue("@passwordType", GetPasswordTypeValue());

                    if (clndr.SelectedDate.Date != DateTime.MinValue.Date)
                    {
                        // Use the selected date from the calendar
                        cmd.Parameters.AddWithValue("@selectedDate", clndr.SelectedDate);
                    }
                    else
                    {
                        // Use today's date if no specific date is selected
                        cmd.Parameters.AddWithValue("@selectedDate", DateTime.Today);
                    }

                    gridview_orders.DataSource = cmd.ExecuteReader();
                    gridview_orders.DataBind();
                    Debug.WriteLine("PopulateOrdersForToday executed.");
                }
                catch (Exception err)
                {
                    Sts.Text = err.Message;
                }
            }
        }
        private void PopulateOrdersForSelectedDate(DateTime selectedDate)
        {
            string constr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\ofos.mdf;Integrated Security=True";

            using (SqlConnection con = new SqlConnection(constr))
            {
                try
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand("GetOrderInformation", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@city", dropdown_city.SelectedItem.Text);
                    cmd.Parameters.AddWithValue("@passwordType", GetPasswordTypeValue());
                    if (selectedDate.Date != DateTime.MinValue.Date)
                    {
                        cmd.Parameters.AddWithValue("@selectedDate", selectedDate);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@selectedDate", DateTime.Today);
                    }
                    gridview_orders.DataSource = cmd.ExecuteReader();
                    gridview_orders.DataBind();
                    Debug.WriteLine("PopulateOrdersForSelectedDate executed.");
                }
                catch (Exception err)
                {
                    Sts.Text = err.Message;
                }
            }
        }

        protected void gridview1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int orderID = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "Order_Id"));

                string Type = GetOrderTypeFromDatabase(orderID);

                Label lblOrderType = (Label)e.Row.FindControl("lblOrderType");
                lblOrderType.Text = Type;

                Button btnShowAddress = (Button)e.Row.FindControl("btnShowAddress");
                btnShowAddress.Enabled = (Type == "Доставка");
            }
        }

        protected void gridview1_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("gridview1_Click executed.");
            Button btn = (Button)sender;
            GridViewRow gvr = (GridViewRow)btn.NamingContainer;

            string constr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\ofos.mdf;Integrated Security=True";

            using (SqlConnection con = new SqlConnection(constr))
            {
                try
                {
                    con.Open();

                    string selectQuery = "GetOrderDetails";
                    SqlCommand cmd = new SqlCommand(selectQuery, con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Order_Id", Convert.ToInt32(gvr.Cells[3].Text));
                    gridview_order_details.DataSource = cmd.ExecuteReader();
                    gridview_order_details.DataBind();
                }
                catch (Exception err)
                {
                    Sts.Text = err.Message;
                }
            }
        }
        protected void ddlUserType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DateTime selectedDate = clndr.SelectedDate;
            PopulateOrdersForSelectedDate(selectedDate);
        }

        private int GetPasswordTypeValue()
        {
            if (ddlUserType.SelectedValue == "All")
            {
                return 2; 
            }
            else if (ddlUserType.SelectedValue == "RegisteredUser")
            {
                return 1;
            }
            else 
            {
                return 0; 
            }
        }
        protected void btnViewDetails_Click(object sender, EventArgs e)
        {
            // Получаване на Order_Id от CommandArgument
            Button btn = (Button)sender;
            int order_id = Convert.ToInt32(btn.CommandArgument);

            // Извикване на запазената процедура и зареждане на резултата в DataTable
            DataTable orderDetails = GetOrderDetailsFromDatabase(order_id);

            // Показване на резултата във втория грид
            gridview_order_details.DataSource = orderDetails;
            gridview_order_details.DataBind();
        }

        private DataTable GetOrderDetailsFromDatabase(int order_id)
        {


            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\ofos.mdf;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand("GetOrderDetails", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@order_id", order_id);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable orderDetails = new DataTable();
                adapter.Fill(orderDetails);

                return orderDetails;
            }
        }
        public string GetOrderTypeFromDatabase(int orderID)
        {
            Debug.WriteLine("GetOrderTypeFromDatabase started");
            string constr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\ofos.mdf;Integrated Security=True";
            string Type = "";

            using (SqlConnection con = new SqlConnection(constr))
            {
                try
                {
                    con.Open();

                    string query = "SELECT Type FROM Delivery WHERE Order_ID = @OrderID";
                    Debug.WriteLine("This:" + query);
                    Debug.WriteLine("OrderID:" + orderID);
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@OrderID", orderID);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Type = reader["Type"].ToString();
                            }
                            
                        }

                    }
                }
                catch (Exception ex)
                {
                    
                    Type = "Грешка: " + ex.Message;
                }
            }

            return Type;
        }
        protected string GetOrderType(object orderTypeObj)
        {
            if (orderTypeObj != null && orderTypeObj != DBNull.Value)
            {
                string Type = orderTypeObj.ToString();
                
                return Type;
            }
            else
            {
                return "Неопределен";
            }
        }


        private string GetCustomerAddressByCustID(int cust_id)
        {
            Debug.WriteLine("GetCustomerAddressByCustID started ");
            string customerAddress = string.Empty;
            string constr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\ofos.mdf;Integrated Security=True";

            using (SqlConnection con = new SqlConnection(constr))
            {
                try
                {
                    con.Open();

                    string query = "EXEC GetCustomerInfo @Cust_Id";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Cust_Id", cust_id);
                        Debug.WriteLine("Cust_ID= " + cust_id);
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            // Извличане на стойности от резултатния ред на процедурата GetCustomerInfo
                            string name = reader["Name"].ToString();
                            string houseNo = reader["House_no"].ToString();
                            string street = reader["Street"].ToString();
                            string contactNo = reader["Contact_no"].ToString();

                            // Сглобете адресната информация в един низ
                            customerAddress = $"Име: {name}, Номер на къща: {houseNo}, Улица: {street}, Телефон: {contactNo}";
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Обработка на грешките
                    customerAddress = "Грешка: " + ex.Message;
                }
            }
            Debug.WriteLine("GetCustomerAddressByCustID finished ");
            Debug.WriteLine("customerAddress= " + customerAddress);
            return customerAddress;
        }
        protected void btnShowAddress_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int order_id = Convert.ToInt32(btn.CommandArgument);
            Debug.WriteLine("OrderID= " + order_id);

            // Извикване на новия метод за намиране на cust_id по order_id
            int cust_id = GetCustomerIDByOrderID(order_id);
            Debug.WriteLine("Cust_ID= " + cust_id);

            // Извикване на метода, който извлича адреса на клиента по cust_id
            string customerAddress = GetCustomerAddressByCustID(cust_id);
            Debug.WriteLine("customerAddress= " + customerAddress);

            if (!string.IsNullOrEmpty(customerAddress))
            {
                string[] customerDetails = customerAddress.Split(','); // Разделяме информацията в масив

                if (customerDetails.Length >= 4)
                {
                    string name = customerDetails[0];
                    string houseNo = customerDetails[1];
                    string street = customerDetails[2];
                    string contactNo = customerDetails[3];

                    // Сега можете да зададете източника на данни на GridView
                    gridview_customer_address.DataSource = new[]
                    {
            new { Name = name, House_no = houseNo, Street = street, Contact_no = contactNo }
        };
                    gridview_customer_address.DataBind();
                }
            }

            // Зареждане на адреса в новия GridView
            //gridview_customer_address.DataSource = new[] { new { Name = name, House_no = houseNo, Street = street, Contact_no = contactNo }};
           // gridview_customer_address.DataBind();

            // Показване на новия GridView
            gridview_customer_address.Visible = true;
        }
        private int GetCustomerIDByOrderID(int order_id)
        {
            int cust_id = 0; // Инициализирайте го с подходяща стойност за липса на данни

            string constr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\ofos.mdf;Integrated Security=True";

            using (SqlConnection con = new SqlConnection(constr))
            {
                try
                {
                    con.Open();

                    string query = "SELECT Cust_ID FROM Orders WHERE Order_ID = @Order_ID";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Order_ID", order_id);
                        object result = cmd.ExecuteScalar();

                        if (result != null)
                        {
                            cust_id = Convert.ToInt32(result);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Обработка на грешките
                    Debug.WriteLine("Грешка при извличане на Cust_ID: " + ex.Message);
                }
            }

            return cust_id;
        }
    }
}