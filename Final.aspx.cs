using System;

namespace OFOS
{
    public partial class Final : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["order_id"] == null)
            {
                Response.Redirect("FoodItems.aspx");
            }

            if (Session["user"] == null)
            {
                b.Visible = false;
            }
            else
            {
                l2.Visible = true;
                l.Text = Session["user"].ToString();
                if (Session["user"].ToString() == "Guest")
                {
                    b.Visible = false;
                    btn_fd.Visible = true;
                    btn_ty.Visible = true;
                }
                else
                {
                    b.Visible = true;
                    btn_fd.Visible = true;
                }
            }

            if (Session["pay"].ToString() == "COD")
            {
                Label1.Text = "Вашата поръчка беше регистрирана успешно" + "<br/><br/>" + "Ще ви бъде доставена в следващите 30-60 минути." +
                    "<br/>" + "Моля, пригответе сумата от " + "<b>" + Session["total"] + "</b>" + " лв. за нашия куриер.";
            }
            else if (Session["pay"].ToString() == "OT")
            {
                Label1.Text = "Order has been successfully placed!" + "<br/>" + "Estimated delivery time: 30 MINUTES";
            }

        }
        protected void FeedBack_click(object sender, EventArgs e)
        {
            Session["pay"] = null;
            Response.Redirect("~/Feedback.aspx");
        }

        protected void Logout1_click(object sender, EventArgs e)
        {
            Session.Abandon();
            Session.Clear();
            Response.Redirect("~/FoodItems.aspx");
        }

        protected void BtnHome_click(object sender, EventArgs e)
        {
            Session["pay"] = null;
            Session["order_id"] = null;
            Response.Redirect("~/FoodItems.aspx");
        }

        protected void thankYou_click(object sender, EventArgs e)
        {
            Session.Abandon();
            Session.Clear();
            Response.Redirect("~/FoodItems.aspx");
        }

    }
}