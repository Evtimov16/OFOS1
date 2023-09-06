using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;


namespace OFOS
{
    public partial class Feedback : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Debug.WriteLine("Page_Load started.");

            if (Session["user"] == null)
            {
                Debug.WriteLine("User session is null. Redirecting to FoodItems.aspx.");
                b.Visible = false;
                Response.Redirect("FoodItems.aspx");
            }
            else
            {
                Debug.WriteLine("User session is not null.");
                l2.Visible = true;
                l.Text = Session["user"].ToString();
            }

            Debug.WriteLine("Page_Load completed.");
        }

        protected void LogOut_click(object sender, EventArgs e)
        {
            Debug.WriteLine("LogOut_click started.");

            Session.Abandon();
            Session.Clear();
            Response.Redirect("~/FoodItems.aspx");

            Debug.WriteLine("LogOut_click completed.");
        }

        protected void Btn1_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("Btn1_Click started.");

            if (tb_fd.Text.Length == 0)
            {
                Debug.WriteLine("Feedback text is empty.");
                Lbl_status.Visible = true;
                Lbl_status.Text = "Please provide feedback";
            }
            else
            {
                Debug.WriteLine("Feedback text is not empty.");

                string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\ofos.mdf;Integrated Security=True";
                SqlConnection con = new SqlConnection(connectionString);
                try
                {
                    Debug.WriteLine("Opening database connection.");
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
                            Debug.WriteLine("Feedback submitted successfully.");
                            Lbl_status.Visible = true;
                            Lbl_status.Text = "Your Feedback has been submitted.";
                        }
                        else
                        {
                            Debug.WriteLine("Failed to submit feedback.");
                            Lbl_status.Visible = true;
                            Lbl_status.Text = "Failed to submit feedback.";
                        }
                    }
                }
                catch (Exception err)
                {
                    Debug.WriteLine("Error: " + err.ToString());
                    Lbl_status.Text = err.ToString();
                }
                finally
                {
                    Debug.WriteLine("Closing database connection.");
                    con.Close();
                }

                Btn1.Visible = false;
            }

            Debug.WriteLine("Btn1_Click completed.");
        }

        protected void Home_click(object sender, EventArgs e)
        {
            Debug.WriteLine("Home_click started.");

            Response.Redirect("FoodItems.aspx");

            Debug.WriteLine("Home_click completed.");
        }

    }
}