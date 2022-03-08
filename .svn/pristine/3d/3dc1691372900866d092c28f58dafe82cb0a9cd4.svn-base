using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Elect.Web.DataTable.Attributes;
using Elect.Web.DataTable.Models.Constants;
using Newtonsoft.Json;

namespace TIGE.Core.Models
{
 public class CreateImageModel
    {
        public string Name { get; set; }
        
        public string ImageContentBase64 { get; set; }

        public int? ImageMaxWidthPx { get; set; }
        
        public int? ImageMaxHeightPx { get; set; }
    }
    
    public class ImageModel
    {
        [DataTable(IsVisible = false, Order = 1)]
        public string Id { get; set; }
        
        [DataTable(DisplayName = "Name", Order = 2)]
        public string Name { get; set; }
        
        [DataTable(DisplayName = "Categories", Order = 3, IsSortable = false, IsSearchable = false)]
        public List<string> CategoryNames { get; set; }
          
        [DataTable(DisplayName = "Project", Order = 4)]
        public string ProjectName { get; set; }
        
        [JsonIgnore]
        [DataTableIgnore]
        public string StorageLocation { get; set; }
        
        [DataTableIgnore]
        public string Extension { get; set; }

        [DataTableIgnore]
        public string MimeType { get; set; }

        [Display(Name = "Size (byte)")]
        [DataTable(DisplayName = "Size (byte)", Order = 5, IsVisible = false)]
        public long ContentLength { get; set; }

        [Display(Name = "Total Access")]
        [DataTable(DisplayName = "Total Access", Order = 6)]
        public long TotalAccess { get; set; }

        [Display(Name = "Last Access At")]
        [DataTable(DisplayName = "Last Access At", Order = 7, IsVisible = false)]
        public DateTimeOffset LastAccessTime { get; set; }

        [Display(Name = "Image Width (px)")]
        [DataTable(DisplayName = "Image Width (px)", Order = 8, IsVisible = false)]
        public int ImageWidthPx { get; set; }

        [Display(Name = "Image Height (px)")]
        [DataTable(DisplayName = "Image Height (px)", Order = 9, IsVisible = false)]
        public int ImageHeightPx { get; set; }
        
        [Display(Name = "Image Color")]
        [DataTable(DisplayName = "Image Color", Order = 10, IsSortable = false, IsSearchable = false)]
        public string ImageDominantHexColor { get; set; }
        
        [DataTableIgnore]
        public string CreatedBy { get; set; }

        [DataTableIgnore]
        public string LastUpdatedBy { get; set; }
        
        [Display(Name = "Project")]
        [DataTableIgnore]
        public string ProjectId { get; set; }
      
        [Display(Name = "Categories")]
        [DataTableIgnore]
        public List<string> CategoryIds { get; set; }

        [Display(Name = "Created At")]
        [DataTable(DisplayName = "Created At", Order = 999, IsVisible = false)]
        public DateTimeOffset CreatedTime { get; set; }

        [Display(Name = "Updated At")]
        [DataTable(DisplayName = "Updated At", Order = 1000, IsVisible = false, SortDirection = SortDirection.Descending)]
        public DateTimeOffset LastUpdatedTime { get; set; }
        
        [DataTable(DisplayName = "Url", IsVisible = false, Order = 1001)]
        public string Url { get; set; }
        
        [DataTable(DisplayName = "Image", IsVisible = false, Order = 10002)]
        public bool IsImage { get; set; }
        
        [Display(Name = "Featured")]
        [DataTable(DisplayName = "Featured", Order = 10003)]
        public bool IsFeatured { get; set; }
    }
    
    public class ImageDownloadModel
    {
        public ImageModel ImageInfo { get; set; }

        public byte[] ImageBytes { get; set; }

        public ImageDownloadModel()
        {
        }

        public ImageDownloadModel(ImageModel imageInfo)
        {
            ImageInfo = imageInfo;
        }

        public ImageDownloadModel(ImageModel imageInfo, byte[] imageBytes) : this(imageInfo)
        {
            ImageBytes = imageBytes;
        }
    }
}