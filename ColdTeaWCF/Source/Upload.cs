using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.IO;
using System.Configuration;


namespace ColdTeaWCF.Source
{
    public static class Upload
    {
        public static string UploadFile(string base64String)
        {
            try
            {
                string returnURL = ConfigurationManager.AppSettings["ReturnURL"];
                string picFolder = ConfigurationManager.AppSettings["ImgDirectory"];
                string FileName = "Upload-" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss-ffff") + ".jpg";
                string FilePath = picFolder + FileName;

                // Convert Base64 String to byte[]
                byte[] imageBytes = Convert.FromBase64String(base64String);
                MemoryStream ms = new MemoryStream(imageBytes, 0,
                  imageBytes.Length);

                // Convert byte[] to Image
                ms.Write(imageBytes, 0, imageBytes.Length);
                var image = Image.FromStream(ms, true);
                image.Save(FilePath);
                string fileout = returnURL + FileName;
                return fileout;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}