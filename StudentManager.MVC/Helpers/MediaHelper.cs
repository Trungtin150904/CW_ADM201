using StudentManager.MVC.Models;

namespace StudentManager.MVC.Helpers
{
    public static class MediaHelper
    {
        public static async Task<bool> SaveMedia(StudentVM studentVM, string wwwRootFolder)
        {
            var isOK = false;
            if (studentVM.Avatar != null && studentVM.Avatar.Length > 0)
            {
                var file = studentVM.Avatar;
                var extension = Path.GetExtension(file.FileName).ToLower();
                var arrayAllowedFile = new string[] { ".jpg", ".jpeg", ".png" };
                if (arrayAllowedFile.Contains(extension))
                {
                    var allowedSize = 5 * 1024 * 1024;
                    if (file.Length < allowedSize)
                    {
                        var fileName = Guid.NewGuid().ToString() + extension;
                        //var wwwRootFolder = _environment.WebRootPath;
                        var mediaFolder = Path.Combine(wwwRootFolder, "media", "images");
                        if (!Directory.Exists(mediaFolder))
                        {
                            Directory.CreateDirectory(mediaFolder);
                        }
                        var destinationFile = Path.Combine(mediaFolder, fileName);
                        using (var fileStream = new FileStream(destinationFile, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                        studentVM.AvatarPath = fileName;
                    }
                }
            }
            return isOK;
        }


    }
}
