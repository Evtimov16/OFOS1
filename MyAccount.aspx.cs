using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Diagnostics;

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

            // Добавете съобщение за вход
            Debug.WriteLine($"User '{Session["user"]}' has logged in.");
        }

        protected void Logout1_click(object sender, EventArgs e)
        {
            // Добавете съобщение за излизане
            Debug.WriteLine($"User '{Session["user"]}' is logging out.");

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

                
                Debug.WriteLine($"User '{Session["user"]}' is viewing account details in edit mode.");
            }
        }

        protected void AccDetails_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
        {
            DropDownList ddlcity = (DropDownList)((DetailsView)sender).FindControl("ddlcity");
            sql1.UpdateParameters["City"].DefaultValue = ddlcity.SelectedValue;

            
            Debug.WriteLine($"User '{Session["user"]}' is updating account details.");
        }
    }
}
