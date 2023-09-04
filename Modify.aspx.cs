
using System;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Diagnostics;
using System.Data;
using System.Linq;

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

            if (!IsPostBack)
            {
                // Задайте DataKeyNames за GridView
                GridView1.DataKeyNames = new string[] { "Item_no" };
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

        // protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        // {
        // Получете Item_no от DataKeys
        //    int itemNo = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value);

        // Намерете CheckBox контрола
        //   CheckBox chkIsActive = GridView1.Rows[e.RowIndex].FindControl("chkIsActive") as CheckBox;
        //
        //  if (chkIsActive != null)
        //  {
        //      bool isActive = chkIsActive.Checked;

        //     string constr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\ofos.mdf;Integrated Security=True";

        //    using (SqlConnection connection = new SqlConnection(constr))
        //  {
        //  connection.Open();

        //   string updateCommand = "UPDATE Item_Master SET IsActive = @IsActive WHERE Item_no = @Item_no";

        //    using (SqlCommand cmd = new SqlCommand(updateCommand, connection))
        //  {
        //      cmd.Parameters.AddWithValue("@IsActive", isActive);
        //      cmd.Parameters.AddWithValue("@Item_no", itemNo);
        //
        //     cmd.ExecuteNonQuery();
        //  }
        // }
        // }
        // }
        protected void SqlDataSource1_Updating(object sender, SqlDataSourceCommandEventArgs e)
        {
            // Извлечете стойности от параметрите
            int itemNo = (int)e.Command.Parameters["@Item_no"].Value;
            string item_name = e.Command.Parameters["@Item_name"].Value.ToString();
            decimal price = (decimal)e.Command.Parameters["@Price"].Value;
            string description = e.Command.Parameters["@Description"].Value.ToString();
            string image_url = e.Command.Parameters["@Image_url"].Value.ToString();
            string type = e.Command.Parameters["@Type"].Value.ToString();

            bool isActive = false; // Дефинирайте isActive извън блока на условния оператор

            // Намерете GridView реда, който съответства на този елемент
            GridViewRow gridViewRow = GridView1.Rows.Cast<GridViewRow>().FirstOrDefault(row => row.RowIndex == itemNo);

            if (gridViewRow != null)
            {
                CheckBox chkActive = (CheckBox)gridViewRow.FindControl("chkActive");

                // Проверете дали чекбоксът е намерен и извлечете стойността му
                if (chkActive != null)
                {
                    isActive = chkActive.Checked;

                    Debug.WriteLine("1IsActive= " + isActive);
                    isActive = !isActive;

                    // Обновете параметъра @IsActive с новата стойност
                    e.Command.Parameters["@IsActive"].Value = isActive;
                    Debug.WriteLine("2IsActive= " + isActive);
                }
            }

            // Обновете записа в базата данни
            string constr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\ofos.mdf;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(constr))
            {
                connection.Open();
                string updateCommand = "UPDATE [Item_Master] SET Item_name=@Item_name, Price=@Price, Description=@Description, Image_url=@Image_url, Type=@Type, IsActive=@IsActive WHERE Item_no=@Item_no";
                using (SqlCommand cmd = new SqlCommand(updateCommand, connection))
                {
                    cmd.Parameters.AddWithValue("@Item_name", item_name);
                    cmd.Parameters.AddWithValue("@Price", price);
                    cmd.Parameters.AddWithValue("@Description", description);
                    cmd.Parameters.AddWithValue("@Image_url", image_url);
                    cmd.Parameters.AddWithValue("@Type", type);
                    cmd.Parameters.AddWithValue("@Item_no", itemNo);

                    Debug.WriteLine("3IsActive= " + isActive);
                    cmd.Parameters.AddWithValue("@IsActive", isActive);
                    Debug.WriteLine("4IsActive= " + isActive);
                    cmd.ExecuteNonQuery();
                }
            }

            // След като сте обновили данните в базата данни, обновете GridView с актуалните данни
            Debug.WriteLine("5IsActive= " + isActive);
            GridView1.DataBind();
        }




    }
}