using System;
using System.Diagnostics;

namespace OFOS
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user"] == null)
            {
                b.Visible = false;
                l2.Visible = false;
                l.Visible = false;
            }
            else
            {
                l2.Visible = true;
                l.Text = Session["user"].ToString();
                if (Session["user"].ToString() == "Guest")
                {
                    b.Visible = false;
                    b1.Visible = true;
                }
                else
                {
                    b.Visible = true;
                    b1.Visible = false;
                }
            }

            if (Session["order_id"] == null)
            {
                Response.Redirect("FoodItems.aspx");
            }
            else
            {
                if (Session["total"] == null)
                {
                    Response.Redirect("MyOrder.aspx");
                }
            }

            Label2.Text = Session["total"].ToString();
            if (!IsPostBack)
            {
                Session["pay"] = null;
            }

            Debug.WriteLine("Debug information for Page_Load event on WebForm1:");
            Debug.WriteLine($"Session[\"user\"] = {Session["user"]}");
            Debug.WriteLine($"Session[\"order_id\"] = {Session["order_id"]}");
            Debug.WriteLine($"Session[\"total\"] = {Session["total"]}");
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Session["pay"] = "OT";
            Response.Redirect("OnlineTrans.aspx");

            Debug.WriteLine("User selected Online Transaction payment method and is navigating to OnlineTrans.aspx.");
        }

        protected void LogOut_click(object sender, EventArgs e)
        {
            Session.Abandon();
            Session.Clear();
            Response.Redirect("~/FoodItems.aspx");

            Debug.WriteLine("User logged out and is redirected to FoodItems.aspx.");
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            Session["pay"] = "COD";
            Response.Redirect("COD_Delivery.aspx");

            Debug.WriteLine("User selected COD payment method and is navigating to COD_Delivery.aspx.");
        }
    }
}
