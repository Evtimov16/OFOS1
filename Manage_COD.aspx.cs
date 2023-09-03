using System;
using System.Data;
using System.Data.SqlClient;


namespace OFOS
{
    public partial class Manage_COD : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["admin"] == null)
            {
                Response.Redirect("Admin_Login.aspx?You need to login first");
            }
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            Session["admin"] = null;

            Response.Redirect("Admin_Login.aspx");
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string conStrng = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\ofos.mdf;Integrated Security=True";

            using (SqlConnection con = new SqlConnection(conStrng))
            {
                try
                {
                    con.Open();

                    using (SqlCommand cmd1 = new SqlCommand("SELECT COD_Pay_Status FROM [dbo].[Payment] WHERE Order_Id = @order_id", con))
                    {
                        cmd1.Parameters.AddWithValue("@order_id", TextBox1.Text);

                        using (SqlDataReader rd = cmd1.ExecuteReader())
                        {
                            if (rd.Read() && rd["COD_Pay_Status"].ToString() == "Received")
                            {
                                Label1.Text = "<h5>Status for Order ID " + TextBox1.Text + " is already updated.</h5>";
                            }
                            else
                            {
                                rd.Close();

                                using (SqlCommand cmd = new SqlCommand("UpdatePaymentStatus", con))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@order_id", TextBox1.Text);

                                    int rowsAffected = cmd.ExecuteNonQuery();

                                    if (rowsAffected == 0)
                                    {
                                        Label1.Text = "<h5>Invalid Order ID " + TextBox1.Text + "</h5>";
                                    }
                                    else
                                    {
                                        Label1.Text = "<h5>Status for the entered Order_Id " + TextBox1.Text + " is updated.</h5>";
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception err)
                {
                    Label1.Text = "Invalid Order Id";
                }
            }
        }
    }
}