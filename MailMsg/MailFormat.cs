using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocCrawler.MailMsg
{
    public class MailFormat
    {
        public static string GetBackgound()
        {
            return null;
        }
        /// <summary>
        /// 图片转Base64
        /// </summary>
        /// <param name="ImageFileName">图片的完整路径</param>
        /// <returns></returns>
        public static string ImgToBase64(string ImageFileName)
        {
            try
            {
                Bitmap bmp = new Bitmap(ImageFileName);
                MemoryStream ms = new MemoryStream();
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();
                return Convert.ToBase64String(arr);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
