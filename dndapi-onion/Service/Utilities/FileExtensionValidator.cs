using System;
using System.Linq;

namespace Service.Utilities
{
    public static class FileExtensionValidator
    {
        public static bool ValidateImageExtension(string extension)
        {
            var allowedExtension = new[] {".png", ".jpg", ".gif"};

            return allowedExtension.Contains(extension, StringComparer.InvariantCultureIgnoreCase);
        }
    }
}
