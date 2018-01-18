using System.Web;

namespace NewsPortal.Managers.Picture
{
    public class PictureManager
    {
        public static string Upload(HttpPostedFileBase uploadedImage, int newsItemId)
        {
            if (uploadedImage != null)
            {
                string fileName = System.IO.Path.GetFileName(uploadedImage.FileName);
                string path = "/Content/UploadedImages/" + newsItemId + fileName;
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