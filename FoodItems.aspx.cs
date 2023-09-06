using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Web.UI.WebControls;

namespace OFOS
{
    public partial class FoodItems : System.Web.UI.Page
    {

        string constr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\ofos.mdf;Integrated Security=True";

        protected void Page_Load(object sender, EventArgs e)
        {
            Debug.WriteLine("Page_Load started.");

            if (Session["order_id"] == null)
            {
                Session["order_id"] = 0;
            }

            System.DateTime t1 = System.DateTime.Parse("2016/12/12 00:00:00.000");
            System.DateTime t2 = System.DateTime.Parse("2016/12/12 23:59:00.000");
            System.DateTime t3 = System.DateTime.Now;
            if (t3.TimeOfDay < t1.TimeOfDay || t3.TimeOfDay > t2.TimeOfDay)
            {
                status.Text = "You Can't Order Now...Visit Again During 11:00 AM to 11:00 PM.";
                status1.Text = "Thank You";
                status.Visible = true;
                status1.Visible = true;
                home.Visible = false;
            }

            if (Session["user"] == null)
            {
                Debug.WriteLine("User session is null. Showing 'hl' and 'Register' elements.");
                hl.Visible = true;
                Register.Visible = true;
                b.Visible = false;
                b1.Visible = false;
            }
            else
            {
                Debug.WriteLine("User session is not null.");
                my_order.Visible = true;
                hl.Visible = false;
                u.Text = Session["user"].ToString();
                Label1.Text = u.Text;

                if (Session["user"].ToString() == "Guest")
                {
                    Debug.WriteLine("User is a guest. Hiding 'b' element and showing 'b1', 'Label1', and 'Label2' elements.");
                    b.Visible = false;
                    b1.Visible = true;
                    Label1.Visible = true;
                    Label2.Visible = true;
                }
                else
                {
                    Debug.WriteLine("User is not a guest. Showing 'b' and 'dropdown' elements.");
                    b.Visible = true;
                    b1.Visible = false;
                    dropdown.Visible = true;
                }
            }

            Debug.WriteLine("Page_Load completed.");
        }

        protected void LogOut_click(object sender, EventArgs e)
        {
            Session.Abandon();
            Session.Clear();
            Response.Redirect("~/FoodItems.aspx");
        }

        protected void signin_click(object sender, EventArgs e)
        {
            Response.Redirect("~/Login.aspx");
        }

        protected void FilterItemsByType(string itemType)
        {
            pic.Visible = false;
            string selectSQL = "select Item_no,Item_name,Description,Image_url,Price,Weight from [dbo].[Item_Master] where (Type=@Type) and IsActive=1";
            SqlConnection con = new SqlConnection(constr);
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(selectSQL, con);
                cmd.Parameters.AddWithValue("@Type", itemType);

                griditem.DataSource = cmd.ExecuteReader();

                griditem.DataBind();
            }
            catch (Exception err)
            {
                status.Text = err.Message;
            }
            finally
            {
                con.Close();
            }
            Debug.WriteLine("FilterItemsByType completed.");
        }

        protected void Button_soup_onclick(object sender, EventArgs e)
        {
            FilterItemsByType("SOUPS");
        }

        protected void Button_starter_onclick(object sender, EventArgs e)
        {
            FilterItemsByType("STARTERS");
        }

        protected void Button_salads_onclick(object sender, EventArgs e)
        {
            FilterItemsByType("SALADS");
        }

        protected void Button_sea_onclick(object sender, EventArgs e)
        {
            FilterItemsByType("SEA FOOD");
        }

        protected void Button_maincourse_onclick(object sender, EventArgs e)
        {
            FilterItemsByType("MAIN COURSE");
        }

        protected void Button_dessert_onclick(object sender, EventArgs e)
        {
            FilterItemsByType("DESSERTS");
        }

        protected void griditem_Click(object sender, EventArgs e)
        {
            
            if (Session["user"] == null)
            {
                Response.Redirect("~/Login.aspx?msg=Please Log In");
            }
            else
            {
                Debug.WriteLine("Value of order_id before executing stored procedure: " + Session["order_id"]);
                SqlConnection con = new SqlConnection(constr);
                try
                {
                    con.Open();

                    int customer_id = (int)Session["customer_id"];
                    Debug.WriteLine("Value of customer_id before executing stored procedure: " + customer_id);
                    SqlCommand checkOrderCmd = new SqlCommand("CheckOpenOrder", con);
                    checkOrderCmd.CommandType = CommandType.StoredProcedure;
                    checkOrderCmd.Parameters.AddWithValue("@Cust_Id", customer_id);
                    object resultObject = checkOrderCmd.ExecuteScalar();
                    int result = Convert.ToInt32(resultObject);

                    
                    if ((Session["order_id"] != null && (int)Session["order_id"] == 0) || (int)result != 0)
                    {
                        if (result != 0)
                        {
                            Session["order_id"] = (int)result;
                        }
                        else
                        {
                            Debug.WriteLine("No open order found.");
                            SqlCommand createOrderCmd = new SqlCommand("CreateOrder", con);
                            createOrderCmd.CommandType = CommandType.StoredProcedure;
                            createOrderCmd.Parameters.AddWithValue("@Cust_Id", customer_id);
                            Session["order_id"] = (int)createOrderCmd.ExecuteScalar();
                        }
                    }
                    Button btn = (Button)sender;
                    GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                    TextBox temp = gvr.Cells[5].FindControl("tb_quantity") as TextBox;

                    string itemName = gvr.Cells[1].Text;
                    string weight = gvr.Cells[6].Text;
                    

                    // Check if the item is already in the order
                    SqlCommand checkExistCmd = new SqlCommand("CheckOrderItemExistence", con);
                    checkExistCmd.CommandType = CommandType.StoredProcedure;
                    checkExistCmd.Parameters.AddWithValue("@Order_Id", Session["order_id"]);
                    checkExistCmd.Parameters.AddWithValue("@Item_no", gvr.Cells[0].Text);
                    int exist = (int)checkExistCmd.ExecuteScalar();
                    Debug.WriteLine("CheckOrderItemExistence completed;");

                    SqlCommand addOrUpdateCmd = new SqlCommand("AddOrUpdateOrderDetail", con);
                    addOrUpdateCmd.CommandType = CommandType.StoredProcedure;
                    addOrUpdateCmd.Parameters.AddWithValue("@orderId", Session["order_id"]);
                    addOrUpdateCmd.Parameters.AddWithValue("@itemNo", Convert.ToInt32(gvr.Cells[0].Text));
                    addOrUpdateCmd.Parameters.AddWithValue("@quantity", Convert.ToInt32(temp.Text));
                    addOrUpdateCmd.Parameters.AddWithValue("@price", Convert.ToDecimal(gvr.Cells[4].Text));
                    addOrUpdateCmd.ExecuteNonQuery();
                    Debug.WriteLine("AddOrUpdateOrderDetail completed;");
                    Session["order_id"] = Session["order_id"];

                    Debug.WriteLine("Generated Order_ID: " + Session["order_id"]);
                }
                catch (Exception err)
                {
                    status.Text = err.Message;
                }

                finally
                {
                    con.Close();
                }
            }
        }

        protected void MyOrder_click(Object sender, EventArgs e)
        {

            Response.Redirect("~/MyOrder.aspx");
        }

        protected void Register_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Registration.aspx");
        }

    }
}
