using QRCoder;
using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OFOS
{
    public partial class TwoFactorAuthentication : Page
    {
        protected Label lblMessage;
        string constr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\ofos.mdf;Integrated Security=True";

        private string GetSecretKeyForCurrentUser(string username)
        {
            string secretKey = string.Empty;

            using (SqlConnection con = new SqlConnection(constr))
            {
                string selectSQL = "SELECT UniqueID FROM [dbo].[Customers] WHERE Username = @username";
                SqlCommand cmd = new SqlCommand(selectSQL, con);
                cmd.Parameters.AddWithValue("@username", username);

                try
                {
                    con.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        secretKey = result.ToString();
                    }
                }
                catch (Exception err)
                {
                    Debug.WriteLine(err.Message);
                }
            }

            return secretKey;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            foreach (string key in Session.Keys)
            {
                Debug.WriteLine($"Session key: {key}, Value: {Session[key]}");
            }
            if (Session["user"] == null)
            {
                lblMessage.Text = "User session is missing or expired. Please login again.";
                return;
            }

            if (Session["2fa_code"] == null)
            {
                lblMessage.Text = "2FA code session is missing or expired. Please login again.";
                return;
            }

            if (!IsPostBack)
            {
                string username = Session["user"] as string;
                string secretKey = GetSecretKeyForCurrentUser(username);
                string uniqueID = secretKey;
                Debug.WriteLine($"Username: {username}, Secret Key: {uniqueID}");
                Debug.WriteLine($"Username: {username}, Secret Key: {UniqueID}");
                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(UniqueID))
                {
                    // Проверка дали сте влезли успешно
                    if (Session["2fa_code"] != null)
                    {
                        Debug.WriteLine($"2FA Code: {Session["2fa_code"]}");
                        GenerateAndDisplayQRCode();
                        lblMessage.Text = "Please enter the 2FA code sent to your device.";
                    }
                    else
                    {
                        // Вече сте влезли успешно, няма нужда от QR код и съобщение
                        Response.Redirect("~/FoodItems.aspx");
                    }
                }
                else
                {
                    Debug.WriteLine("Username or Secret Key is missing or empty.");
                    Response.Redirect("~/Login.aspx");
                }
            }
        }

        private void GenerateAndDisplayQRCode()
        {
            string appName = "OnlineFoodApp";
            string accountId = Session["user"] as string;
            string secretKey = GetSecretKeyForCurrentUser(accountId);
            string uniqueID = secretKey;

            string qrCodeUrl = TwoFactorAuthenticator.GenerateQRCodeUrl(appName, accountId, uniqueID);

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrCodeUrl, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            System.Drawing.Bitmap qrCodeBitmap = qrCode.GetGraphic(10);
            string base64QRCode = ImageToBase64(qrCodeBitmap);
            System.Web.UI.WebControls.Image qrCodeImage = new System.Web.UI.WebControls.Image();
            qrCodeImage.ImageUrl = "data:image/png;base64," + base64QRCode;

            divQRCode.Controls.Add(qrCodeImage);
        }

        private string ImageToBase64(System.Drawing.Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                byte[] imageBytes = ms.ToArray();
                return Convert.ToBase64String(imageBytes);
            }
        }

        protected void btnVerify_Click(object sender, EventArgs e)
        {
            if (Session["2fa_code"] != null)
            {
                int expectedCode = (int)Session["2fa_code"];
                int enteredCode = Convert.ToInt32(tbTwoFactorCode.Text);

                string username = Session["user"] as string;
                string uniqueID = GetSecretKeyForCurrentUser(username);

                bool isCodeValid = TwoFactorAuthenticator.VerifyTwoFactorCode(uniqueID, enteredCode);

                if (isCodeValid)
                {
                    Response.Redirect("~/FoodItems.aspx");
                }
                else
                {
                    lblMessage.Text = "Invalid 2FA code. Please try again.";
                }
            }
        }
    }
}
