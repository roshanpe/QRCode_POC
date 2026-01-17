using System;
using System.Drawing;
using System.Drawing.Imaging;
using QRCoder;

namespace QR
{
    class Program
    {
        static void Main(string[] args)
        {
            // Sample data
            string name = "John Doe";
            string email = "john.doe@example.com";
            DateTime dateOfBirth = new DateTime(1990, 5, 15);

            // Generate QR code
            string qrFilePath = GenerateQRCode(name, email, dateOfBirth);
            Console.WriteLine($"QR Code generated successfully: {qrFilePath}");
        }

        static string GenerateQRCode(string name, string email, DateTime dateOfBirth)
        {
            // Format the data as a string
            string qrData = $"Name: {name}\nEmail: {email}\nDate of Birth: {dateOfBirth:yyyy-MM-dd}";

            // Alternative: Use VCard format for better compatibility with contact apps
            // string qrData = GenerateVCard(name, email, dateOfBirth);

            // Create QR code generator
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrData, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);

            // Generate bitmap with custom colors
            Bitmap qrCodeImage = qrCode.GetGraphic(20, Color.Black, Color.White, true);

            // Save to file
            string fileName = $"QRCode_{DateTime.Now:yyyyMMddHHmmss}.png";
            qrCodeImage.Save(fileName, ImageFormat.Png);

            return fileName;
        }

        // Optional: Generate VCard format for contact information
        static string GenerateVCard(string name, string email, DateTime dateOfBirth)
        {
            return $@"BEGIN:VCARD
VERSION:3.0
FN:{name}
EMAIL:{email}
BDAY:{dateOfBirth:yyyyMMdd}
END:VCARD";
        }

        // Method to generate QR code as byte array (useful for web apps)
        static byte[] GenerateQRCodeBytes(string name, string email, DateTime dateOfBirth)
        {
            string qrData = $"Name: {name}\nEmail: {email}\nDate of Birth: {dateOfBirth:yyyy-MM-dd}";

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrData, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);

            using (Bitmap qrCodeImage = qrCode.GetGraphic(20))
            {
                using (var ms = new System.IO.MemoryStream())
                {
                    qrCodeImage.Save(ms, ImageFormat.Png);
                    return ms.ToArray();
                }
            }
        }
    }
}