

using NexusWeb.Models;
using System.Text;

namespace NexusWeb.Helpers
{
    public class MyUtil
    {
        public static string UploadHinh(IFormFile Hinh, string folder)
        {
            try
            {
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "admin", folder, Hinh.FileName);
                using (var myfile = new FileStream(fullPath, FileMode.CreateNew))
                {
                    Hinh.CopyTo(myfile);
                }
                return Hinh.FileName;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static string GenerateRamdomKey(int length = 5)
        {
            var pattern = @"qazwsxedcrfvtgbyhnujmiklopQAZWSXEDCRFVTGBYHNUJMIKLOP!";
            var sb = new StringBuilder();
            var rd = new Random();
            for (int i = 0; i < length; i++)
            {
                sb.Append(pattern[rd.Next(0, pattern.Length)]);
            }

            return sb.ToString();
        }

        public static decimal? TotalMoneyInOrder(List<Order> List)
        {
            decimal? total = 0;
            foreach (var item in List)
            {
                total = total + item.Total;
            }
            return total;
        }
    }
}
