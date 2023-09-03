using OtpNet;
using System;


namespace OFOS
{
    public class TwoFactorAuthenticator
    {
        public static string GenerateQRCodeUrl(string appName, string accountId, string secretKey)
        {
            return $"otpauth://totp/{appName}:{accountId}?secret={secretKey}&issuer={appName}";
        }

        public static string GenerateRandomSecretKey()
        {
            const int secretKeyLength = 20;
            const string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";

            Random random = new Random();
            char[] secretKeyChars = new char[secretKeyLength];

            for (int i = 0; i < secretKeyLength; i++)
            {
                secretKeyChars[i] = validChars[random.Next(validChars.Length)];
            }

            return new string(secretKeyChars);
        }

        public static bool VerifyTwoFactorCode(string secretKey, int code)
        {
            var totp = new Totp(Base32Encoding.ToBytes(secretKey));
            return totp.VerifyTotp(code.ToString(), out _);
        }
    }
}
