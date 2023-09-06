using System;
using System.Diagnostics;

namespace OFOS
{
    public partial class Admin_tasks : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            Debug.WriteLine("Page_Load: Initializing the page.");
            if (!IsPostBack)
            {
                Debug.WriteLine("Page_Load: Not a postback. Checking if the user is logged in.");
                if (Session["admin"] == null)
                {
                    Debug.WriteLine("Page_Load: User is not logged in. Redirecting to Admin_Login.aspx.");
                    Response.Redirect("Admin_Login.aspx?You need to login first");
                }
            }

            if (Session["admin"] != null)
            {
                Debug.WriteLine("Page_Load: User is logged in. Displaying a welcome message.");
                Label1.Text = "Hello" + " , " + Session["admin"].ToString();
            }
            else
            {
                Debug.WriteLine("Page_Load: User is not logged in. Not displaying the welcome message.");
            }
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("LinkButton1_Click: Logout button clicked. Clearing the session and redirecting to Admin_Login.aspx.");
            Session["admin"] = null;
            Response.Redirect("Admin_Login.aspx");
        }
    }
}
