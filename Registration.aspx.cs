using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace OFOS
{
    public partial class Registration : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_register_Click(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\ofos.mdf;Integrated Security=True";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    string password = tb_pwd.Text;
                    if (!IsPasswordStrong(password))
                    {
                        lblStatus.Text = "Password must be 8-16 characters long and contain at least one uppercase letter, one lowercase letter, one digit, and one special character.";
                        return;
                    }

                    using (SqlCommand insertCmd = new SqlCommand("CreateUserWithSecretKey", con))
                    {
                        insertCmd.CommandType = CommandType.StoredProcedure;

                        insertCmd.Parameters.AddWithValue("@Name", tb_name.Text);
                        insertCmd.Parameters.AddWithValue("@Username", tb_username.Text);
                        insertCmd.Parameters.AddWithValue("@Password", tb_pwd.Text);
                        insertCmd.Parameters.AddWithValue("@Email", tb_email.Text);
                        insertCmd.Parameters.AddWithValue("@Contact_No", tb_contact.Text);
                        insertCmd.Parameters.AddWithValue("@House_No", tb_house.Text);
                        insertCmd.Parameters.AddWithValue("@Street", tb_street.Text);
                        insertCmd.Parameters.AddWithValue("@City", DropDownList1_city.Text);

                        SqlParameter secretKeyParam = new SqlParameter("@UniqueID", SqlDbType.NVarChar, 50);
                        secretKeyParam.Direction = ParameterDirection.Output;
                        insertCmd.Parameters.Add(secretKeyParam);

                        int added = insertCmd.ExecuteNonQuery();

                        string secretKey = secretKeyParam.Value.ToString();

                        lblStatus.Text = "Registration successful.";


                    }
                }
                catch (Exception err)
                {
                    lblStatus.Text = "Error: " + err.Message;
                }
            }
        }
        private bool IsPasswordStrong(string password)
        {
            
            string pattern = @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&*()_+}{:;'?/><,.[-]).{8,16}$";
            return Regex.IsMatch(password, pattern);
        }
        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            Response.Redirect("FoodItems.aspx");
        }
    }
}