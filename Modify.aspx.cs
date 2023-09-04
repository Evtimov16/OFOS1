
using System;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Diagnostics;
using System.Data;

namespace OFOS
{
    public partial class Modify : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["admin"] == null)
            {
                Response.Redirect("Admin_Login.aspx?You need to login first");
            }



        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            Session["admin"] = null;
            Response.Redirect("~/Admin_Login.aspx");
        }

       // protected void chkIsActive_CheckedChanged(object sender, EventArgs e)
        //{
        //    CheckBox chk = (CheckBox)sender;
        //    GridViewRow row = (GridViewRow)chk.NamingContainer;
        //
            // Проверете дали се извършва редакция
         //   if (row.RowState == DataControlRowState.Edit)
         //   {
          //      int itemNo = Convert.ToInt32(GridView1.DataKeys[row.RowIndex].Value);
          //
          //      Debug.WriteLine("IsActive = " + chk.Checked);
          //      UpdateItemStatus(itemNo, chk.Checked);
          //  }
      //  }

       // private void UpdateItemStatus(int itemNo, bool isActive)
       // {
            // Вашата логика за обновление на статуса на активност в базата данни тук
            //string constr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\ofos.mdf;Integrated Security=True";

            //using (SqlConnection con = new SqlConnection(constr))
            //{
             //   con.Open();
            //
             //   SqlCommand cmd = new SqlCommand("UPDATE Item_Master SET IsActive = 0 WHERE Item_no = 101", con);
                //cmd.Parameters.AddWithValue("@Item_no", itemNo);

               // if (isActive)
               // {
                //    cmd.Parameters.AddWithValue("@IsActive", 1); 
                //}
                //else
                //{
                //    cmd.Parameters.AddWithValue("@IsActive", 0); 
               // }
                
              // SqlParameter isActiveParam = cmd.Parameters.AddWithValue("@IsActive", isActive);
               //isActiveParam.SqlDbType = SqlDbType.Bit;

             //   cmd.ExecuteNonQuery();
           // }
       // }
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkIsActive = (CheckBox)e.Row.FindControl("chkIsActive");
                if (chkIsActive != null)
                {
                    // Покажете CheckBox само при редактиране
                    if (e.Row.RowState == DataControlRowState.Edit)
                    {
                        chkIsActive.Visible = true;
                        // Можете да се свържете с базата данни и заредите стойността на IsActive за текущия ред
                        int itemNo = Convert.ToInt32(GridView1.DataKeys[e.Row.RowIndex].Value);
                        bool isActive = LoadItemStatusFromDatabase(itemNo); // Заменете с вашата логика за зареждане на статуса
                        chkIsActive.Checked = isActive;
                    }
                    else
                    {
                        chkIsActive.Visible = false;
                    }
                }
            }
        }
        private bool LoadItemStatusFromDatabase(int itemNo)
        {
            string constr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\ofos.mdf;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(constr))
            {
                connection.Open();

                string query = "SELECT IsActive FROM [Item_Master] WHERE Item_no = @Item_no";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Item_no", itemNo);

                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        // Ако има стойност, конвертирайте я към bool и върнете
                        return Convert.ToBoolean(result);
                    }
                }
            }

            // Ако не намерите стойност в базата данни, върнете false като стойност по подразбиране
            return false;
        }

       
        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            // Вземете Item_no на елемента, който се редактира
            int itemNo = Convert.ToInt32(GridView1.DataKeys[e.NewEditIndex].Value);

            // Вземете CheckBox контрола
            CheckBox chkIsActive = (CheckBox)GridView1.Rows[e.NewEditIndex].FindControl("chkIsActive");

            // Заредете стойността на CheckBox контрола
            bool isActive = LoadItemStatusFromDatabase(itemNo); // Заменете с вашата логика за зареждане на статуса

            // Задайте стойността на CheckBox контрола
            chkIsActive.Checked = isActive;
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            // Получете Item_no от DataKeys
            int itemNo = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value);

            // Намерете CheckBox контрола
            CheckBox chkIsActive = GridView1.Rows[e.RowIndex].FindControl("chkIsActive") as CheckBox;

            if (chkIsActive != null)
            {
                bool isActive = chkIsActive.Checked;

                string constr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\ofos.mdf;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(constr))
                {
                    connection.Open();

                    string updateCommand = "UPDATE Item_Master SET IsActive = @IsActive WHERE Item_no = @Item_no";

                    using (SqlCommand cmd = new SqlCommand(updateCommand, connection))
                    {
                        cmd.Parameters.AddWithValue("@IsActive", isActive);
                        cmd.Parameters.AddWithValue("@Item_no", itemNo);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }


    }
}