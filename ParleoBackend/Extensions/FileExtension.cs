using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ParleoBackend.Extensions
{
    public static class FileExtension
    {
        public static async Task<string> SaveAsync(this IFormFile image, string directoryPath)
        {
            string imageUniqueName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            string path = Path.Combine(directoryPath, imageUniqueName);

            if (image != null && image.Length > 0)
            {
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }
            }

            return imageUniqueName;
        }

        public static string GetFullFilePath(string baseUrl, string folder, string fileName) => 
            !string.IsNullOrEmpty(fileName)
            ? string.Format("{0}{1}/{2}", baseUrl, folder, fileName)
            : null;
    }
}
