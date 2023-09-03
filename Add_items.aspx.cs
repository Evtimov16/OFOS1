using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace OFOS
{
    public partial class Add_items : System.Web.UI.Page
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
            Response.Redirect("Admin_Login.aspx");
        }

        protected void Button1_Click(object sender, EventArgs e)
        {

            String conStrng = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\ofos.mdf;Integrated Security=True";
            using (SqlConnection con = new SqlConnection(conStrng))
            {

                try
                {
                    con.Open();

                    SqlCommand validationCmd = new SqlCommand("CheckItemExistence", con);
                    validationCmd.CommandType = CommandType.StoredProcedure;
                    validationCmd.Parameters.AddWithValue("@item_no", Int32.Parse(TextBox1.Text));

                    int exist = (int)validationCmd.ExecuteScalar();

                    if (exist > 0)
                    {
                        Label1.Text = "Item No already exists. Enter another one.";
                    }
                    else
                    {
                        SqlCommand insertCmd = new SqlCommand("InsertItem", con);
                        insertCmd.CommandType = CommandType.StoredProcedure;
                        insertCmd.Parameters.AddWithValue("@item_no", Int32.Parse(TextBox1.Text));
                        insertCmd.Parameters.AddWithValue("@item_name", TextBox2.Text);
                        insertCmd.Parameters.AddWithValue("@type", dropdown_type.SelectedItem.Text);
                        insertCmd.Parameters.AddWithValue("@price", float.Parse(TextBox4.Text));
                        insertCmd.Parameters.AddWithValue("@description", TextBox5.Text);

                        if (FileUpload1.HasFile)
                        {
                            string fileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
                            string imageurl = "IMAGES\\" + dropdown_type.SelectedItem.Text + "\\" + fileName;

                            insertCmd.Parameters.AddWithValue("@image_url", imageurl);
                        }

                        insertCmd.ExecuteNonQuery();

                        Label1.Text = "Item Added successfully.";
                    }
                }
                catch (Exception err)
                {
                    Label1.Text = "Error Inserting Record.";
                    Label1.Text = err.ToString();
                }
                finally
                {
                    con.Close();
                }
            }
        }

    }
}