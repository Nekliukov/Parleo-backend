using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ParleoBackend.Extensions
{
    public static class FormFileExtension
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
    }
}
