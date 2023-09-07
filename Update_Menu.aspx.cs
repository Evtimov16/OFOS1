using System;
using System.Diagnostics;


namespace OFOS
{
    public partial class Update_Menu : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["admin"] == null)
            {
                // Дебъг съобщение: Администраторът не е влязъл и е пренасочен към страницата за вход
                Debug.WriteLine("Admin is not logged in and is redirected to the login page.");
                Response.Redirect("Admin_Login.aspx?You need to login first");
            }
            else
            {
                string adminUsername = Session["admin"].ToString();
                // Дебъг съобщение: Администраторът е успешно влязъл
                Debug.WriteLine($"Admin '{adminUsername}' is logged in.");
            }
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            Session["admin"] = null;

            Debug.WriteLine("Admin is logged out.");
            Response.Redirect("Admin_Login.aspx");
        }

        protected void Button1_Click(object sender, EventArgs e)
        {

            Debug.WriteLine("Admin is navigating to Add_items.aspx.");
            Response.Redirect("Add_items.aspx");
        }

        protected void Button2_Click(object sender, EventArgs e)
        {

            Debug.WriteLine("Admin is navigating to Modify.aspx.");
            Response.Redirect("Modify.aspx");
        }
    }
}
