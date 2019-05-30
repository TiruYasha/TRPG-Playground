using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Config
{
    public class FileStorageConfig
    {
        public virtual string BigImageLocation { get; set; }
        public virtual string ThumbnailLocation { get; set; }
    }
}
