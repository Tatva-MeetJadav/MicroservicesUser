using Microsoft.AspNetCore.Http;

namespace MicroservicesUser.BusinessLogic.Utilities
{
    public static class UploadFile
    {
        public static async Task<string?> UploadPhotoAsync(IFormFile file, string webRootPath, string imagePath)
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }

            string uploadsFolder = Path.Combine(webRootPath, "images", imagePath);
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (FileStream stream = new(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return uniqueFileName;
        }

        public static string GetOriginalPhotoName(string storedFileName)
        {
            if (string.IsNullOrEmpty(storedFileName))
            {
                return string.Empty;
            }

            int index = storedFileName.IndexOf('_');
            if (index < 0 || index == storedFileName.Length - 1)
            {
                return string.Empty;
            }
            return storedFileName[(index + 1)..];
        }
    }
}