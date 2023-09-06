using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace OFOS
{
    public partial class Admin_Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Debug.WriteLine("Page_Load: Initializing the page.");
            Session["admin"] = null;
            status.Text = Request.QueryString["msg"];
            Debug.WriteLine("Page_Load: Query string message set to: " + status.Text);
        }

        protected void Admin_Login_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("Admin_Login_Click: Login button clicked.");

            String constrng = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\ofos.mdf;Integrated Security=True";
            using (SqlConnection con = new SqlConnection(constrng))
            {
                try
                {
                    Debug.WriteLine("Admin_Login_Click: Attempting database connection.");
                    con.Open();
                    Debug.WriteLine("Admin_Login_Click: Database connection opened.");

                    SqlCommand validationCmd = new SqlCommand("ValidateAdminLogin", con);
                    validationCmd.CommandType = CommandType.StoredProcedure;
                    validationCmd.Parameters.AddWithValue("@username", tb_admin.Text);
                    validationCmd.Parameters.AddWithValue("@password", tb_pwd.Text);

                    int isValid = (int)validationCmd.ExecuteScalar();
                    Debug.WriteLine("Admin_Login_Click: Executed ValidateAdminLogin stored procedure. Validation result: " + isValid);

                    if (isValid > 0)
                    {
                        Session["admin"] = tb_admin.Text;
                        Debug.WriteLine("Admin_Login_Click: Admin login successful. Redirecting to AdminSearch.aspx.");
                        Response.Redirect("AdminSearch.aspx");
                    }
                    else
                    {
                        status.Text = "Invalid Username or Password.";
                        Debug.WriteLine("Admin_Login_Click: Invalid Username or Password. Displaying error message.");
                    }
                }
                catch (Exception ex)
                {
                    status.Text = "Error connecting to the database.";
                    Debug.WriteLine("Admin_Login_Click: Error connecting to the database. Error message: " + ex.ToString());
                }
                finally
                {
                    con.Close();
                    Debug.WriteLine("Admin_Login_Click: Database connection closed.");
                }
            }
        }
    }
}