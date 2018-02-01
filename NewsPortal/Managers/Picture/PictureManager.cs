using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace NewsPortal.Managers.Picture
{
    public class PictureManager
    {
        public static string Upload(HttpPostedFileBase uploadedImage, int newsItemId)
        {
            int maxSize = 2 * 1024 * 1024;
            List<string> mimes = new List<string>
            {
                "image/jpeg", "image/jpg", "image/png"
            };
            if (uploadedImage != null)
            {
                if (uploadedImage.ContentLength > maxSize || 
                    mimes.FirstOrDefault(m => m == uploadedImage.ContentType) == null)
                {
                    return null;
                }
                string fileName = System.IO.Path.GetFileName(uploadedImage.FileName);
                string path = ConfigurationManager.AppSettings["pathForImage"] + newsItemId + fileName;
                uploadedImage.SaveAs(HttpContext.Current.Server.MapPath(path));
                return path;
            }
            return null;
        }

        public static void Delete(string path)
        {
            if (path != null)
            {
                System.IO.File.Delete(HttpContext.Current.Server.MapPath(path));
            }
        }
    }
}