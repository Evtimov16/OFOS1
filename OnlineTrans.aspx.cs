using System;
using System.Data.SqlClient;
using System.Diagnostics;

namespace OFOS
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["order_id"] == null)
            {
                Response.Redirect("FoodItems.aspx");
                Debug.WriteLine("User does not have an active order. Redirecting to FoodItems.aspx.");
            }
            else
            {
                if (Session["pay"] == null)
                {
                    Response.Redirect("FoodItems.aspx?First place the order");
                    Debug.WriteLine("Payment session variable is not set. Redirecting to FoodItems.aspx.");
                }
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string constr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\ofos.mdf;Integrated Security=True";

            using (SqlConnection con = new SqlConnection(constr))
            {
                string q = "EXEC GetAccountBalance @acc, @name, @apwd";
                SqlCommand cmd = new SqlCommand(q, con);
                SqlDataReader reader;

                try
                {
                    con.Open();

                    cmd.Parameters.AddWithValue("@name", Name.Text);
                    cmd.Parameters.AddWithValue("@acc", Int32.Parse(Acc_no.Text));
                    cmd.Parameters.AddWithValue("@apwd", Acc_pwd.Text);

                    reader = cmd.ExecuteReader();

                    if (reader.Read() == false)
                    {
                        Label1.Text = "Invalid details";
                        Debug.WriteLine("Invalid account details provided.");
                    }
                    else
                    {
                        string balance = reader["Balance"].ToString();
                        Label1.Text = balance;
                        reader.Close();
                        float bal = float.Parse(balance);

                        int total = (int)(Session["total"]);

                        if (bal >= total)
                        {
                            float nbal = bal - total;

                            string q1 = "EXEC UpdateAccountBalance @acc, @name, @apwd, @nbal";
                            SqlCommand cmd1 = new SqlCommand(q1, con);
                            cmd1.Parameters.AddWithValue("@nbal", nbal);
                            cmd1.Parameters.AddWithValue("@name", Name.Text);
                            cmd1.Parameters.AddWithValue("@acc", Int32.Parse(Acc_no.Text));
                            cmd1.Parameters.AddWithValue("@apwd", Acc_pwd.Text);

                            cmd1.ExecuteNonQuery();

                            Label1.Text = "Transaction Successful." + "<br/>" +
                                "Payment of  " + total + "лв. received." + "<br/>" + nbal;

                            string q2 = "EXEC UpdateOrderStatus @order_id, @date";
                            SqlCommand cmd2 = new SqlCommand(q2, con);
                            cmd2.Parameters.AddWithValue("@order_id", (int)Session["order_id"]);
                            cmd2.Parameters.AddWithValue("@date", System.DateTime.Now);

                            cmd2.ExecuteNonQuery();

                            string q3 = "EXEC InsertPaymentRecord @Order_Id, @Mode, @Acc_No";
                            SqlCommand cmd3 = new SqlCommand(q3, con);
                            cmd3.Parameters.AddWithValue("@Order_Id", (int)Session["order_id"]);
                            cmd3.Parameters.AddWithValue("@Mode", "Online Transaction");
                            cmd3.Parameters.AddWithValue("@Acc_No", Int32.Parse(Acc_no.Text));
                            cmd3.ExecuteNonQuery();

                            Response.Redirect("COD_Delivery.aspx");

                            Debug.WriteLine("Transaction Successful. Payment received and order status updated.");
                        }
                        else
                        {
                            Label1.Text = "Transaction cancelled due to insufficient balance.";
                            Debug.WriteLine("Transaction cancelled due to insufficient balance.");
                        }
                    }
                }
                catch (Exception err)
                {
                    Label1.Text = "Please provide details.";
                    Debug.WriteLine("Error: " + err.Message);
                }
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            //Acc_pwd.Visible = false;
            Session["pay"] = null;
            Session["pay"] = "COD";
            Response.Redirect("COD_Delivery.aspx");

            Debug.WriteLine("User selected COD payment method and is navigating to COD_Delivery.aspx.");
        }
    }
}
