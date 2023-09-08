using System;
using System.Diagnostics;

namespace OFOS
{
    public partial class Final : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Debug.WriteLine("Page_Load started.");

            if (Session["order_id"] == null)
            {
                Debug.WriteLine("Order ID session is null. Redirecting to FoodItems.aspx.");
                Response.Redirect("FoodItems.aspx");
            }

            if (Session["user"] == null)
            {
                Debug.WriteLine("User session is null. Hiding 'b' element.");
                b.Visible = false;
            }
            else
            {
                Debug.WriteLine("User session is not null.");
                l2.Visible = true;
                l.Text = Session["user"].ToString();
                if (Session["user"].ToString() == "Guest")
                {
                    Debug.WriteLine("User is a guest. Hiding 'b' element and showing 'btn_fd' and 'btn_ty' elements.");
                    b.Visible = false;
                    btn_fd.Visible = true;
                    btn_ty.Visible = true;
                }
                else
                {
                    Debug.WriteLine("User is not a guest. Showing 'b' element.");
                    b.Visible = true;
                    btn_fd.Visible = true;
                }
            }

            if (Session["pay"].ToString() == "COD")
            {
                string selectedTime = Session["SelectedTime"] as string;
                Debug.WriteLine("Payment method is COD.");
                Label1.Text = "Вашата поръчка беше регистрирана успешно" + "<br/><br/>" + "Ще ви бъде доставена в " + Session["SelectedTime"] + 
                "<br/>" + "Моля, пригответе сумата от " + "<b>" + Session["total"] + "</b>" + " лв. ";
            }
            else if (Session["pay"].ToString() == "OT")
            {
                Debug.WriteLine("Payment method is OT.");
                Label1.Text = "Order has been successfully placed!" + "<br/>" + "Estimated delivery time: 30 MINUTES";
            }

            Debug.WriteLine("Page_Load completed.");
        }

        protected void FeedBack_click(object sender, EventArgs e)
        {
            Debug.WriteLine("FeedBack_click started.");

            Session["pay"] = null;
            Response.Redirect("~/Feedback.aspx");

            Debug.WriteLine("FeedBack_click completed.");
        }

        protected void Logout1_click(object sender, EventArgs e)
        {
            Debug.WriteLine("Logout1_click started.");

            Session.Abandon();
            Session.Clear();
            Response.Redirect("~/FoodItems.aspx");

            Debug.WriteLine("Logout1_click completed.");
        }

        protected void BtnHome_click(object sender, EventArgs e)
        {
            Debug.WriteLine("BtnHome_click started.");

            Session["pay"] = null;
            Session["order_id"] = null;
            Response.Redirect("~/FoodItems.aspx");

            Debug.WriteLine("BtnHome_click completed.");
        }

        protected void thankYou_click(object sender, EventArgs e)
        {
            Debug.WriteLine("thankYou_click started.");

            Session.Abandon();
            Session.Clear();
            Response.Redirect("~/FoodItems.aspx");

            Debug.WriteLine("thankYou_click completed.");
        }
    }
}
