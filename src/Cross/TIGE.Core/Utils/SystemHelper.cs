#region	License

//--------------------------------------------------
// <License>
//     <Copyright> 2018 © Linh Nguyen </Copyright>
//     <Url> http://linhnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> TIGE </Project>
//     <File>
//         <Name> SystemHelper.cs </Name>
//         <Created> 21/04/2018 5:34:40 PM </Created>
//         <Key> 3bd06eed-ba10-4098-8ed5-aa26ce08dae1 </Key>
//     </File>
//     <Summary>
//         SystemHelper.cs is a part of TIGE
//     </Summary>
// <License>
//--------------------------------------------------

#endregion License

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Elect.Core.CheckUtils;
using Elect.Data.IO;
using Elect.Data.IO.DirectoryUtils;
using Flurl;
using Flurl.Http;
using Microsoft.AspNetCore.Http;
using Mono.Unix.Native;
using TIGE.Core.Constants;
using TIGE.Core.Models;
using TIGE.Core.Share.Models.Crypto;

namespace TIGE.Core.Utils
{
    public static class SystemHelper
    {
        public static SystemSettingModel Setting => SystemSettingModel.Instance;

        public static string GetAbsoluteEndpoint(string endpoint)
        {
            endpoint = endpoint.Trim(' ', '~');

            return Setting.Domain.AppendPathSegment(endpoint);
        }

        public static string GetSaveImageLocation(string fileName)
        {
            CheckHelper.CheckNullOrWhiteSpace(fileName, nameof(fileName));

            var folderLocation = Locations.SavePath;

            DirectoryHelper.CreateIfNotExist(PathHelper.GetFullPath(folderLocation));

            var fileLocation = Path.Combine(folderLocation, fileName);

            return fileLocation;
        }

        public static CreateImageModel ToCreateFileModel(this IFormFile file, int? imageMaxWidthPx = null,
            int? imageMaxHeightPx = null)
        {
            byte[] imageBytes;

            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);

                imageBytes = stream.ToArray();
            }

            var createFileModel = new CreateImageModel
            {
                Name = file.FileName,
                ImageContentBase64 = Convert.ToBase64String(imageBytes),
                ImageMaxWidthPx = imageMaxWidthPx,
                ImageMaxHeightPx = imageMaxHeightPx
            };
            return createFileModel;
        }

        public static ImageModel GetRandomImage(IEnumerable<ImageModel> images)
        {
            var listImage = images.ToList();

            if (listImage.Any())
            {
                return null;
            }
            
            var random = new Random();

            var i = random.Next(0, listImage.Count - 1);

            return listImage.ElementAt(i);
        }
        
        public static string GetRandomImageUrl(IEnumerable<string> imageUrls)
        {
            var listImage = imageUrls.ToList();
            
            if (listImage.Any())
            {
                return null;
            }

            var random = new Random();

            var i = random.Next(0, listImage.Count - 1);

            return listImage.ElementAt(i);
        }

        public static string GetYoutubeThumbnailUrl(string youtubeUrl)
        {
            var id = youtubeUrl.Split('=').LastOrDefault();

            var thumbnailUrl = $"https://img.youtube.com/vi/{id}/maxresdefault.jpg";

            return thumbnailUrl;
        }


        public static double GetETHRate()
        {
            var dataTask = SystemHelper.Setting.ExchangeRateEndpoint.GetJsonAsync<ApiResponseModel<RateResponseModel>>();
            dataTask.Wait();
            var dataRate = dataTask.Result;
            if (dataRate.Data.Rates.ETH != null)
            {
                var ethRate = (double) 1/double.Parse(dataRate.Data.Rates.ETH);
                return ethRate;
            }

            return 0;
        }
    }
}