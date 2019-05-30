using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Domain
{
    public class Image
    {
        public Guid Id { get; private set; }
        public string Extension { get; private set; }
        public string OriginalName { get; private set; }

        private Image()
        {

        }

        public Image(string extension, string originalName)
        {
            Id = Guid.NewGuid();
            Extension = extension;
            OriginalName = originalName;
        }
    }
}
