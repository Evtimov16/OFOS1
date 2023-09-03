using System;
using System.Data;
using System.Data.SqlClient;


namespace OFOS
{
    public partial class Feedback : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user"] == null)
            {
                b.Visible = false;
                Response.Redirect("FoodItems.aspx");
            }
            else
            {
                l2.Visible = true;
                l.Text = Session["user"].ToString();
            }
        }

        protected void LogOut_click(object sender, EventArgs e)
        {
            Session.Abandon();
            Session.Clear();
            Response.Redirect("~/FoodItems.aspx");
        }

        protected void Btn1_Click(object sender, EventArgs e)
        {
            if (tb_fd.Text.Length == 0)
            {
                Lbl_status.Visible = true;
                Lbl_status.Text = "Please provide feedback";
            }
            else
            {
                string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\ofos.mdf;Integrated Security=True";
                SqlConnection con = new SqlConnection(connectionString);
                try
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand("InsertFeedback", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Cust_Id", (int)Session["customer_id"]);
                        cmd.Parameters.AddWithValue("@Username", Session["user"]);
                        cmd.Parameters.AddWithValue("@Comment", tb_fd.Text);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            Lbl_status.Visible = true;
                            Lbl_status.Text = "Your Feedback has been submitted.";
                        }
                        else
                        {
                            Lbl_status.Visible = true;
                            Lbl_status.Text = "Failed to submit feedback.";
                        }
                    }
                }
                catch (Exception err)
                {
                    Lbl_status.Text = err.ToString();
                }
                finally
                {
                    con.Close();
                }

                Btn1.Visible = false;
            }
        }

        protected void Home_click(object sender, EventArgs e)
        {
            Response.Redirect("FoodItems.aspx");
        }
    }
}