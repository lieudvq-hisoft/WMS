using System.Drawing;
using System.Drawing.Imaging;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using BarcodeLib;
using Data.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using QRCoder;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Services.Utils
{
    public static class MyFunction
    {
        public static string GetId(this ClaimsPrincipal user)
        {
            var idClaim = user.Claims.FirstOrDefault(i => i.Type.Equals("UserId"));
            if (idClaim != null)
            {
                return idClaim.Value;
            }
            return "";
        }
        public static string ConvertToUnSign(string input)
        {
            input = input.Trim();
            for (int i = 0x20; i < 0x30; i++)
            {
                input = input.Replace(((char)i).ToString(), " ");
            }
            Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
            string str = input.Normalize(NormalizationForm.FormD);
            string str2 = regex.Replace(str, string.Empty).Replace('đ', 'd').Replace('Đ', 'D');
            while (str2.IndexOf("?") >= 0)
            {
                str2 = str2.Remove(str2.IndexOf("?"), 1);
            }
            return str2;
        }

        public static TimeSpan TimespanCalculate(DateTime dateTime)
        {
            return dateTime - DateTime.Now;
        }

        public static string uploadImage(IFormFile file, string path)
        {

            if (!Directory.Exists(path))
            { 
                Directory.CreateDirectory(path);
            }
            var extension = Path.GetExtension(file.FileName);

            var imageName = DateTime.Now.ToBinary() + Path.GetFileName(file.FileName);

            string filePath = Path.Combine(path, imageName);

            using (Stream fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyToAsync(fileStream);
            }
            
            return filePath.Split("/app/wwwroot")[1];
        }

        public static void deleteImage(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        public static (int position, DateTime[] days) GetWeekInfo()
        {
            var today = DateTime.Today;
            var dayOfWeek = (int)today.DayOfWeek;

            var days = new DateTime[7];
            for (int i = 0; i < 7; i++)
            {
                days[i] = today.AddDays(-dayOfWeek + i);
            }

            return (dayOfWeek, days);
        }

        public static List<DateTime> Get7DaysWithToday()
        {
            var today = DateTime.Today;
            var dates = new List<DateTime>();

            for (int i = 0; i < 7; i++)
            {
                dates.Insert(0, today.AddDays(-i));
            }

            return dates;
        }

        public static byte[] GenerateBarcode(string content, BarcodeLib.TYPE barcodeType = BarcodeLib.TYPE.CODE128, int width = 700, int height = 200)
        {
            Barcode barcode = new Barcode();
            barcode.IncludeLabel = true;
            barcode.Alignment = AlignmentPositions.CENTER;

            Image barcodeImage = barcode.Encode(BarcodeLib.TYPE.CODE128, content, width, height);
            using (MemoryStream stream = new MemoryStream())
            {

                barcodeImage.Save(stream, ImageFormat.Png);
                return stream.ToArray();
            }
        }

        public static byte[] GenerateQrcode(string content)
        {
            byte[] QRCode = null;
            QRCodeGenerator qRCodeGenerator = new QRCodeGenerator();
            QRCodeData dataQr = qRCodeGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);
            BitmapByteQRCode bitmap = new BitmapByteQRCode(dataQr);
            QRCode = bitmap.GetGraphic(20);
            return QRCode;
        }
    }
}

