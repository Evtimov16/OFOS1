using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
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

                int userTypeValue;
                if (ddlUserType.SelectedValue == "All")
                {
                    userTypeValue = 2;
                }
                else if (ddlUserType.SelectedValue == "RegisteredUser")
                {
                    userTypeValue = 0;
                }
                else
                {
                    userTypeValue = 1;
                }

                cmd.Parameters.AddWithValue("@passwordType", userTypeValue);
                if (clndr.SelectedDate.Date != DateTime.MinValue.Date)
                {
                    DateTime selectedDate = clndr.SelectedDate;
                    DateTime startDate = selectedDate.Date;
                    DateTime endDate = startDate.AddDays(1).AddSeconds(-1);

                    cmd.Parameters.AddWithValue("@date", startDate);

                    Debug.WriteLine("Start Date:" + startDate.ToString("dd.MM.yyyy г. H:mm:ss"));
                    Debug.WriteLine("End Date:" + endDate.ToString("dd.MM.yyyy г. H:mm:ss"));
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
                Debug.WriteLine("PopulateOrdersForToday executed." );
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
                string orderStatus = e.Row.Cells[3].Text;

                if (orderStatus == "Нова") // Проверете според вашия статус
                {
                    e.Row.CssClass = "new-order";
                }
            }
            Debug.WriteLine("gridview1_RowDataBound executed.");
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
                return 2; // "Всички"
            }
            else if (ddlUserType.SelectedValue == "RegisteredUser")
            {
                return 0; // "Регистриран потребител"
            }
            else // ddlUserType.SelectedValue == "Guest"
            {
                return 1; // "Гост"
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
    }
}