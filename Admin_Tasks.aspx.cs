using System;

namespace OFOS
{
    public partial class Admin_tasks : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["admin"] == null)
                {
                    Response.Redirect("Admin_Login.aspx?You need to login first");
                }

            }
            Label1.Text = "Hello" + " , " + Session["admin"].ToString();

        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            Session["admin"] = null;
            Response.Redirect("Admin_Login.aspx");
        }
    }
}