using System;
using System.Data;
using System.Data.SqlClient;

namespace OFOS
{
    public partial class Admin_Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["admin"] = null;
            status.Text = Request.QueryString["msg"];
        }

        protected void Admin_Login_Click(object sender, EventArgs e)
        {
            String constrng = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\ofos.mdf;Integrated Security=True";
            using (SqlConnection con = new SqlConnection(constrng))
            {
                con.Open();

                SqlCommand validationCmd = new SqlCommand("ValidateAdminLogin", con);
                validationCmd.CommandType = CommandType.StoredProcedure;
                validationCmd.Parameters.AddWithValue("@username", tb_admin.Text);
                validationCmd.Parameters.AddWithValue("@password", tb_pwd.Text);

                int isValid = (int)validationCmd.ExecuteScalar();

                if (isValid > 0)
                {
                    Session["admin"] = tb_admin.Text;
                    Response.Redirect("AdminSearch.aspx");
                }
                else
                {
                    status.Text = "Invalid Username or Password.";
                }
            }
        }

    }
}