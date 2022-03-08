using System;
using System.Collections.Generic;

namespace TIGE.Models
{
    public static class StaticFiles
    {
        public static readonly List<StaticFileModel> ListStaticFile = new List<StaticFileModel>
        {
            new StaticFileModel
            {
                AreaFolderName = string.Empty,
                AreaName = string.Empty,
                FolderRelativePath = "wwwroot",
                HttpRequestPath = string.Empty,
                MaxAgeResponseHeader = new TimeSpan(365, 0, 0)
            },

            new StaticFileModel
            {
                AreaFolderName = "Areas",
                AreaName = "Portal",
                FolderRelativePath = "wwwroot",
                HttpRequestPath = "/portal",
                MaxAgeResponseHeader = new TimeSpan(365, 0, 0)
            }
        };
        
        public static readonly List<string> ListStaticResourceExtension = new List<string>
        {
            ".css",
            ".css.map",
            ".scss",
            ".less",
            ".js",
            ".js.map",
            ".json",
            ".rss",
            ".xml",
            ".mp3",
            ".mp4",
            ".ogg",
            ".ogv",
            ".webm",
            ".svg",
            ".svgz",
            ".eot",
            ".tff",
            ".otf",
            ".woff",
            ".woff2",
            ".crx",
            ".xpi",
            ".safariextz",
            ".flv",
            ".f4v",
            ".png",
            ".jpeg",
            ".jpg",
            ".bmp",
            "ico",
            "xlsx",
            "xls",
            "doc",
            "docx",
            "pdf",
        };
    }
}