using System.IO;
using System.Threading.Tasks;
using Domain.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Domain.Utilities
{
    public class ImageProcesser
    {
        public virtual async Task SaveImage(IFormFile file, string path, string fileName)
        {
            Directory.CreateDirectory(path);

            if (file.Length <= 0) throw new FileException("The file may not be empty");

            var fullPath = Path.Combine(path, fileName);
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
        }
    }
}
