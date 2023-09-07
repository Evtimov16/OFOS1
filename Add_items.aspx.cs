using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;

namespace OFOS
{
    public partial class Add_items : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["admin"] == null)
            {
                Debug.WriteLine("Page_Load: Session[\"admin\"] is null. Redirecting to Admin_Login.aspx.");
                Response.Redirect("Admin_Login.aspx?You need to login first");
            }
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("LinkButton1_Click: Clearing Session[\"admin\"] and redirecting to Admin_Login.aspx.");
            Session["admin"] = null;
            Response.Redirect("Admin_Login.aspx");
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("Button1_Click: Processing form submission.");

            String conStrng = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\ofos.mdf;Integrated Security=True";
            using (SqlConnection con = new SqlConnection(conStrng))
            {
                try
                {
                    con.Open();
                    Debug.WriteLine("Button1_Click: Database connection opened.");

                    SqlCommand validationCmd = new SqlCommand("CheckItemExistence", con);
                    validationCmd.CommandType = CommandType.StoredProcedure;
                    validationCmd.Parameters.AddWithValue("@item_no", Int32.Parse(TextBox1.Text));

                    int exist = (int)validationCmd.ExecuteScalar();
                    Debug.WriteLine("Button1_Click: Executed CheckItemExistence stored procedure. Existence result: " + exist);

                    if (exist > 0)
                    {
                        Label1.Text = "Item No already exists. Enter another one.";
                        Debug.WriteLine("Button1_Click: Item No already exists. Displayed error message.");
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
                        Debug.WriteLine("Button1_Click: Executed InsertItem stored procedure. Item Added successfully.");
                        Label1.Text = "Item Added successfully.";
                    }
                }
                catch (Exception err)
                {
                    Label1.Text = "Error Inserting Record.";
                    Debug.WriteLine("Button1_Click: Error Inserting Record. Error message: " + err.ToString());
                }
                finally
                {
                    con.Close();
                    Debug.WriteLine("Button1_Click: Database connection closed.");
                }
            }
        }
    }
}