using System;
using System.Data.SqlClient;
using System.Diagnostics;

namespace OFOS
{
    public partial class Login : System.Web.UI.Page
    {
        private const int SessionTimeoutMinutes = 5;
        string constr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\ofos.mdf;Integrated Security=True";

        protected void Page_Load(object sender, EventArgs e)
        {
            status0.Text = Request.QueryString["msg"];
        }
        private string GetSecretKeyForCurrentUser(string username)
        {
            string secretKey = string.Empty;

            using (SqlConnection con = new SqlConnection(constr))
            {
                string selectSQL = "SELECT UniqueID FROM [dbo].[Customers] WHERE Username = @username";
                SqlCommand cmd = new SqlCommand(selectSQL, con);
                cmd.Parameters.AddWithValue("@username", username);

                try
                {
                    con.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        secretKey = result.ToString();
                    }
                }
                catch (Exception err)
                {
                    status.Text = err.Message;
                }
            }

            return secretKey;
        }
        protected void Login_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                string selectSQL = "select * from [dbo].[Customers] where Username=@username AND Password=@password";
                SqlCommand cmd = new SqlCommand(selectSQL, con);
                SqlDataReader reader;

                try
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@username", tb_user.Text);
                    cmd.Parameters.AddWithValue("@password", tb_pwd.Text);

                    // Дебъг инструкция: Изведете съставената SQL заявка и параметрите
                    Debug.WriteLine($"SQL Query: {selectSQL}, Parameters: @username = {tb_user.Text}, @password = {tb_pwd.Text}");

                    reader = cmd.ExecuteReader();
                    if (reader.Read() == false)
                    {
                        status.Text = "Invalid Username or Password.";
                    }
                    else
                    {
                        int customer_id = (int)reader["Cust_Id"];
                        Random random = new Random();
                        int twoFactorCode = random.Next(100000, 999999);

                        Session["2fa_code"] = twoFactorCode;
                        Session["customer_id"] = (int)reader["Cust_Id"];
                        Session["user"] = tb_user.Text; // Запазваме само потребителското име

                        // Запазване на потребителското име и секретния ключ в сесията за 5 минути
                        string secretKey = GetSecretKeyForCurrentUser(tb_user.Text);
                        Session["username"] = tb_user.Text;
                        Session["secretKey"] = secretKey;
                        Session.Timeout = SessionTimeoutMinutes;

                        // Дебъг инструкция: Изведете информация за създадените сесионни променливи
                        Debug.WriteLine($"Session created: 2fa_code = {twoFactorCode}, customer_id = {Session["customer_id"]}, user = {Session["user"]}, username = {Session["username"]}, secretKey = {Session["secretKey"]}");

                        Response.Redirect("~/FoodItems.aspx");
                    }
                }
                catch (Exception err)
                {
                    status.Text = err.Message;
                }
            }
        }

        protected void user_click(object sender, EventArgs e)
        {
            registered.Visible = true;
            guest.Visible = false;
        }

        protected void guest_click(object sender, EventArgs e)
        {
            registered.Visible = false;
            guest.Visible = true;
        }

        protected void Button1_register_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                try
                {
                    con.Open();
                    string insertSQL = "EXEC Add_UniqueUsername @Username, @Name" ;
                    SqlCommand cmd = new SqlCommand(insertSQL, con);
                    cmd.Parameters.AddWithValue("@Username", "Guest" + System.DateTime.Now.ToString());
                    cmd.Parameters.AddWithValue("@Name", tb_name.Text);
                   
                    int added;
                    added = cmd.ExecuteNonQuery();

                    string select = "select MAX(Cust_Id) from [dbo].[Customers]";
                    cmd = new SqlCommand(select, con);
                    int x = (int)cmd.ExecuteScalar();
                    Session["customer_id"] = x;
                    Session["user"] = "Guest";

                    Session.Timeout = 5;
                    Response.Redirect("~/FoodItems.aspx");
                }
                catch (Exception err)
                {
                    status.Text = err.Message;
                }
            }
        }
    }
}
