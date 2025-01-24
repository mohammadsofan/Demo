namespace Demo.PL.Helpers
{
    public static class FileHelper
    {
        public static async Task<string> UploadImage(IFormFile file, string folderName)
        {
            // Validate the file is not null
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File is empty or null.");
            }

            // Validate the file size (example: max 2MB)
            const long maxFileSize = 2 * 1024 * 1024; // 2MB in bytes
            if (file.Length > maxFileSize)
            {
                throw new ArgumentException($"File size exceeds the limit of {maxFileSize / (1024 * 1024)}MB.");
            }

            // Validate the file type (allow only specific image types)
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(fileExtension))
            {
                throw new ArgumentException($"Invalid file type. Allowed types: {string.Join(", ", allowedExtensions)}");
            }

            // Build the path
            var rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/files");
            var folderPath = Path.Combine(rootPath, folderName);

            // Ensure the directory exists
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Generate a unique file name
            var imageName = Guid.NewGuid().ToString()+file.FileName+ fileExtension;
            var filePath = Path.Combine(folderPath, imageName);

            // Save the file
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return imageName;
        }

        public static  bool DeleteImage(string folderName,string fileName)
        {
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/files", folderName,fileName);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                return true;
            }
            return false;

        }
    }
}
