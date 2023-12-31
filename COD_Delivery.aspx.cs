﻿using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Web.UI.WebControls;

namespace OFOS
{
    public partial class COD : System.Web.UI.Page
    {

        private void PopulateTimeDropDowns()
        {
            DateTime currentTime = DateTime.Now.AddMinutes(30);
            DateTime endTime = DateTime.Parse("9:30 PM");

            while (currentTime <= endTime)
            {
                ListItem timeItem = new ListItem(currentTime.ToString("hh:mm tt"), currentTime.ToString("hh:mm tt"));
                TimeDropDownList.Items.Add(timeItem);
                PickupTimeDropDownList.Items.Add(timeItem);
                currentTime = currentTime.AddMinutes(30);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Debug.WriteLine("Page_Load executed.");
            if (Session["order_id"] == null)
            {
                Debug.WriteLine("Session['order_id'] is null. Redirecting to FoodItems.aspx.");
                Response.Redirect("FoodItems.aspx");
            }
            else
            {
                Debug.WriteLine("Session['order_id'] is not null.");
                if (Session["pay"] == null)
                {
                    Debug.WriteLine("Session['pay'] is null. Redirecting to MyOrder.aspx.");
                    Response.Redirect("MyOrder.aspx?Payment mode needs to be selected");
                }
            }
            if (Session["user"] == null)
            {

                // b.Visible = false;

            }
            else
            {


                l2.Visible = true;
                l.Text = Session["user"].ToString();
                if (Session["user"].ToString() == "Guest")
                {
                    //  b1.Visible = true;
                    //b.Visible = false;
                }
                else
                {
                    // b.Visible = true;
                    //  b1.Visible = false;
                }

            }
            if (!IsPostBack)
            {
                PopulateTimeDropDowns();
            }

            if (IsPostBack == false)
            {
                if (Session["pay"].ToString() == "OT")
                {
                    Label1.Text = "Transaction successful!" + "<br/>" + "Payment of " + Session["total"] + "лв. received." + "<br/><br/>" + "Please provide Delivery details.";
                }
                else if (Session["pay"].ToString() == "COD")
                {
                    Label1.Text = "Please provide Delivery details.";
                }

                string constr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\ofos.mdf;Integrated Security=True";

                using (SqlConnection con = new SqlConnection(constr))
                {
                    try
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand("GetCustomerInfo", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Cust_Id", Session["customer_id"]);

                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            Name.Text = reader["Name"].ToString();
                            House_no.Text = reader["House_no"].ToString();
                            Street.Text = reader["Street"].ToString();
                            D_city.Text = reader["City"].ToString();
                            Contact.Text = reader["Contact_no"].ToString();
                        }
                    }
                    catch (Exception err)
                    {
                        Label1.Text = err.ToString();
                    }
                    Debug.WriteLine("Page_Load completed.");
                }
            }
        }


        protected void Button2_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("Button2_Click started.");
            string deliveryMethod = RadioButtonList1.SelectedValue;
            string selectedTime = TimeDropDownList.SelectedValue;
            Session["SelectedTime"] = selectedTime;

            string constr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\ofos.mdf;Integrated Security=True";
            if (deliveryMethod == "Delivery")
            {


                using (SqlConnection con = new SqlConnection(constr))
                {
                    try
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand("InsertDelivery", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Order_Id", Session["order_id"]);
                        cmd.Parameters.AddWithValue("@Name", Name.Text);
                        cmd.Parameters.AddWithValue("@House_no", House_no.Text);
                        cmd.Parameters.AddWithValue("@Street", Street.Text);
                        cmd.Parameters.AddWithValue("@City", D_city.Text);
                        cmd.Parameters.AddWithValue("@Contact_No", Contact.Text);
                        cmd.Parameters.AddWithValue("@Type", "Доставка");

                        cmd.ExecuteNonQuery();

                        if (Session["pay"].ToString() == "COD")
                        {
                            SqlCommand cmd2 = new SqlCommand("UpdateOrderStatus", con);
                            cmd2.CommandType = CommandType.StoredProcedure;

                            cmd2.Parameters.AddWithValue("@order_id", (int)Session["order_id"]);
                            cmd2.Parameters.AddWithValue("@date", DateTime.Now);

                            cmd2.ExecuteNonQuery();

                            SqlCommand cmd3 = new SqlCommand("InsertPayment", con);
                            cmd3.CommandType = CommandType.StoredProcedure;

                            cmd3.Parameters.AddWithValue("@Order_Id", (int)Session["order_id"]);
                            cmd3.Parameters.AddWithValue("@Mode", "COD");
                            cmd3.Parameters.AddWithValue("@COD_Pay_Status", "Pending");

                            cmd3.ExecuteNonQuery();
                        }

                        Response.Redirect("Final.aspx");
                    }
                    catch (Exception err)
                    {
                        Label1.Text = err.ToString();
                    }
                }
            }
            else if (deliveryMethod == "Pickup")
            {


                using (SqlConnection con = new SqlConnection(constr))
                {
                    try
                    {

                        con.Open();
                        SqlCommand cmd = new SqlCommand("InsertPickUp", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Order_Id", Session["order_id"]);
                        cmd.Parameters.AddWithValue("@Name", Name.Text);
                        cmd.Parameters.AddWithValue("@City", D_city.Text);
                        cmd.Parameters.AddWithValue("@Contact_No", Contact.Text);
                        cmd.Parameters.AddWithValue("@Type", "Вземане от място");

                        cmd.ExecuteNonQuery();

                        if (Session["pay"].ToString() == "COD")
                        {
                            SqlCommand cmd2 = new SqlCommand("UpdateOrderStatus", con);
                            cmd2.CommandType = CommandType.StoredProcedure;

                            cmd2.Parameters.AddWithValue("@order_id", (int)Session["order_id"]);
                            cmd2.Parameters.AddWithValue("@date", DateTime.Now);

                            cmd2.ExecuteNonQuery();

                            SqlCommand cmd3 = new SqlCommand("InsertPayment", con);
                            cmd3.CommandType = CommandType.StoredProcedure;

                            cmd3.Parameters.AddWithValue("@Order_Id", (int)Session["order_id"]);
                            cmd3.Parameters.AddWithValue("@Mode", "COD");
                            cmd3.Parameters.AddWithValue("@COD_Pay_Status", "Pending");

                            cmd3.ExecuteNonQuery();
                        }

                        Response.Redirect("Final.aspx");
                    }
                    catch (Exception err)
                    {
                        Label1.Text = err.ToString();
                    }
                }
            }
            Debug.WriteLine("Button2_Click completed.");
        }
        protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Debug.WriteLine("RadioButtonList1_SelectedIndexChanged started.");
            if (RadioButtonList1.SelectedValue == "Delivery")
            {
                Debug.WriteLine("RadioButtonList1 selected value is 'Delivery'. Showing DeliveryPanel.");
                DeliveryPanel.Visible = true;
                PickupPanel.Visible = false;
            }
            else if (RadioButtonList1.SelectedValue == "Pickup")
            {
                Debug.WriteLine("RadioButtonList1 selected value is 'Pickup'. Showing PickupPanel.");
                DeliveryPanel.Visible = false;
                PickupPanel.Visible = true;
            }
            Debug.WriteLine("RadioButtonList1_SelectedIndexChanged completed.");
        }


    }
}