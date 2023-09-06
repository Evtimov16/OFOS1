using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web.UI.WebControls;

namespace OFOS
{
    public partial class MyOrder : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int order_id = (int)Session["order_id"];
            Debug.WriteLine("Value of order_id after retrieving from session: " + order_id);

            if (Session["user"] == null)
            {
                Response.Redirect("FoodItems.aspx");
                b.Visible = false;

                // Добавете съобщение за редирект
                Debug.WriteLine("User not logged in. Redirecting to FoodItems.aspx.");
            }
            else
            {
                b.Visible = true;
                l2.Visible = true;
                l.Text = Session["user"].ToString();
                if (Session["user"].ToString() == "Guest")
                {
                    b.Visible = false;
                    b1.Visible = true;
                }
                else
                {
                    b.Visible = true;
                    b1.Visible = false;
                }

                // Добавете съобщение за вход на потребителя
                Debug.WriteLine($"User '{Session["user"]}' has logged in.");
            }

            if (Session["order_id"] == null)
            {
                lbl.Text = "Все още нямате поръчани неща";
                Button2.Visible = false;

                // Добавете съобщение за липса на поръчка
                Debug.WriteLine("User does not have any orders.");
            }
            else
            {
                lbl.Text = "Код на поръчката: " + ((int)Session["order_id"]).ToString();

                string constr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\ofos.mdf;Integrated Security=True";

                using (SqlConnection con = new SqlConnection(constr))
                {
                    string q = "SELECT SUM(Amount) FROM [dbo].[Order_Details] WHERE Order_Id = @Order_Id";

                    using (SqlCommand cmd = new SqlCommand(q, con))
                    {
                        try
                        {
                            con.Open();
                            cmd.Parameters.AddWithValue("@Order_Id", (int)Session["order_id"]);

                            object sumObj = cmd.ExecuteScalar();
                            if (sumObj != null && sumObj != DBNull.Value)
                            {
                                decimal sum = Convert.ToDecimal(sumObj);
                                Label1.Text = "Total Amount: " + sum.ToString("F2") + "лв.";
                                Session["total"] = sum;
                            }
                            else
                            {
                                Label1.Text = "Все още нямате поръчани неща";
                                Button2.Visible = false;
                            }

                            // Добавете съобщение за зареждане на сумата на поръчката
                            Debug.WriteLine($"Loaded total amount for Order ID {Session["order_id"]}: {Label1.Text}");
                        }
                        catch (Exception err)
                        {
                            Label1.Text = err.ToString();
                        }
                    }
                }
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect("FoodItems.aspx");

            // Добавете съобщение за пренасочване към FoodItems.aspx
            Debug.WriteLine("User is navigating to FoodItems.aspx.");
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            Response.Redirect("Payment.aspx");

            // Добавете съобщение за пренасочване към Payment.aspx
            Debug.WriteLine("User is navigating to Payment.aspx.");
        }

        protected void LogOut_click(object sender, EventArgs e)
        {
            Session.Abandon();
            Session.Clear();
            Response.Redirect("~/FoodItems.aspx");

            // Добавете съобщение за излизане
            Debug.WriteLine("User has logged out.");
        }

        protected void gridorder_RowUpdated(object sender, GridViewUpdatedEventArgs e)
        {
            string arg = e.NewValues[0].ToString();
            try
            {
                int n = Int32.Parse(arg);
                if (n < 1 || n > 10)
                {
                    gridorder.Rows[0].Cells[2].Text = e.OldValues[2].ToString();
                    gridorder.DataBind();
                }
            }
            catch (FormatException)
            {
                Response.Write("Invalid Quantity");
            }
        }

        protected void gridorder_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                string arg = e.NewValues[0].ToString();
                int o;
                if (!arg.All(char.IsDigit))
                {
                    e.Cancel = true;
                    gridorder.Rows[0].Cells[2].Text = e.OldValues[2].ToString();
                    gridorder.DataBind();
                    this.Page_Load(sender, e);
                }
                int n = Int32.Parse(arg);
                if (n < 1 || n > 10)
                {
                    e.Cancel = true;
                    gridorder.Rows[0].Cells[2].Text = e.OldValues[2].ToString();
                    gridorder.DataBind();
                }
            }
            catch (FormatException i)
            {
                e.Cancel = true;
                gridorder.Rows[0].Cells[2].Text = e.OldValues[2].ToString();
                gridorder.DataBind();
            }
        }
    }
}
