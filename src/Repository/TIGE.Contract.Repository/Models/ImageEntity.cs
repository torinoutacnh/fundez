using System;
using System.Collections.Generic;
using TIGE.Core.Share.Utils;

namespace TIGE.Contract.Repository.Models
{
    public class ImageEntity : Entity
    {
        public ImageEntity()
        {
            CreatedTime = LastUpdatedTime = LastAccessTime = CoreHelper.SystemTimeNow;
        }

        public string StorageLocation { get; set; }

        public string Name { get; set; }

        public string Extension { get; set; }

        public string MimeType { get; set; }

        public bool IsImage { get; set; }
        
        public bool IsFeatured { get; set; }

        public long ContentLength { get; set; }

        public long TotalAccess { get; set; }

        public DateTimeOffset LastAccessTime { get; set; }

        // Image

        public string ImageDominantHexColor { get; set; }

        public int ImageWidthPx { get; set; } = -1;

        public int ImageHeightPx { get; set; } = -1;

        // Hash to prevent duplicate
        public string OriginalImageHash { get; set; }

        // Project
        
        public string ProjectId { get; set; }
        
        // Categories
    }
}