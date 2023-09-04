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
            if (Session["admin"] == null)
            {
                Response.Redirect("Admin_Login.aspx?msg=You need to login first");
            }

            if (!IsPostBack)
            {
                dropdown_city.SelectedIndex = 0;
                ddlUserType.SelectedIndex = 0; // Избира "Всички" като стойност по подразбиране
            }


            Debug.WriteLine("Page_Load executed.");
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
                    // Ако е избрано "Всички", използвайте стойност 2 или друга, която означава "Всички".
                    userTypeValue = 2;
                }
                else if (ddlUserType.SelectedValue == "RegisteredUser")
                {
                    // Ако е избрано "Регистриран потребител", използвайте стойност 0 или друга, която означава "Регистриран потребител".
                    userTypeValue = 0;
                }
                else // ddlUserType.SelectedValue == "Guest"
                {
                    // Ако е избрано "Гост", използвайте стойност 1 или друга, която означава "Гост".
                    userTypeValue = 1;
                }
                cmd.Parameters.AddWithValue("@passwordType", userTypeValue);
                if (clndr.SelectedDate.Date != DateTime.MinValue.Date)
                {
                    // Изчислете началната и крайната дата и час
                    DateTime selectedDate = clndr.SelectedDate;
                    DateTime startDate = selectedDate.Date; // Началната дата е 0:00:00 на избраната дата
                    DateTime endDate = startDate.AddDays(1).AddSeconds(-1); // Крайната дата е 23:59:59 на избраната дата

                    // Добавете началната и крайната дата към параметрите на командата
                    cmd.Parameters.AddWithValue("@date", startDate);
                    //cmd.Parameters.AddWithValue("@endDate", endDate);
                    Debug.WriteLine("Start Date:" + startDate.ToString("dd.MM.yyyy г. H:mm:ss"));
                    Debug.WriteLine("End Date:" + endDate.ToString("dd.MM.yyyy г. H:mm:ss"));
                }
                else
                {
                    // Ако не е избрана дата, оставете параметрите празни
                    cmd.Parameters.AddWithValue("@date", DBNull.Value);
                    //cmd.Parameters.AddWithValue("@endDate", DBNull.Value);
                }
                var a = cmd.ExecuteReader();
                //while (a.Read())
                //{
                //    int count = a.VisibleFieldCount;
                //    for (int i = 0; i < count; i++)
                //    {
                //        Debug.WriteLine(a[i]);
                //    }
               // }
                Debug.WriteLine("A=" + a.HasRows);
                Debug.WriteLine("City:" + dropdown_city);
                Debug.WriteLine("Password:" + userTypeValue);
                Debug.WriteLine("Date" + clndr.SelectedDate.ToString("dd.MM.yyyy г. H:mm:ss"));

                
                
                gridview_orders.DataSource = a;
                gridview_orders.DataBind();
                
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
                    e.Row.CssClass = "highlight";
                }
            }
            Debug.WriteLine("Error in gridview1_Click: " );
        }

        protected void gridview1_Click(object sender, EventArgs e)
        {
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
    }
}