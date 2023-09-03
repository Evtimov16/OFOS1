using System;
using System.Data;
using System.Web.UI.WebControls;

namespace DemoPro
{
    public partial class MyAccount : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user"] == null || Session["user"].ToString() == "Guest")
            {
                Response.Redirect("FoodItems.aspx");
            }
        }

        protected void Logout1_click(object sender, EventArgs e)
        {

            Session.Abandon();
            Session.Clear();
            Response.Redirect("~/FoodItems.aspx");
        }

        protected void goBack_click(object sender, EventArgs e)
        {
            Response.Redirect("~/FoodItems.aspx");
        }

        protected void AccDetails_DataBound(object sender, EventArgs e)
        {
            if (((DetailsView)sender).CurrentMode == DetailsViewMode.Edit)
            {
                DataRowView row = (DataRowView)((DetailsView)sender).DataItem;

                DropDownList ddlcity = (DropDownList)((DetailsView)sender).FindControl("ddlcity");
                ddlcity.SelectedValue = row[6].ToString();
            }
        }
        protected void AccDetails_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
        {
            DropDownList ddlcity = (DropDownList)((DetailsView)sender).FindControl("ddlcity");
            sql1.UpdateParameters["City"].DefaultValue = ddlcity.SelectedValue;
        }
    }
}