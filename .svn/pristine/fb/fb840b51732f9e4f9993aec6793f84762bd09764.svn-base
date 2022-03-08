using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Elect.Core.SecurityUtils;
using Elect.Data.IO;
using Elect.Data.IO.FileUtils;
using Elect.Data.IO.ImageUtils;
using Elect.Data.IO.ImageUtils.CompressUtils;
using Elect.Data.IO.ImageUtils.ResizeUtils;
using Elect.DI.Attributes;
using Elect.Mapper.AutoMapper.IQueryableUtils;
using Elect.Mapper.AutoMapper.ObjUtils;
using Elect.Web.Api.Models;
using Elect.Web.DataTable.Models.Request;
using Elect.Web.DataTable.Models.Response;
using Elect.Web.DataTable.Processing.Response;
using Microsoft.Extensions.Caching.Memory;
using TIGE.Contract.Repository.Interfaces;
using TIGE.Contract.Repository.Models;
using TIGE.Contract.Service;
using TIGE.Core.Models;
using TIGE.Core.Share.Constants;
using TIGE.Core.Share.Exceptions;
using TIGE.Core.Share.Utils;
using TIGE.Core.Utils;
using PagedRequestModel = TIGE.Core.Models.PagedRequestModel;

namespace TIGE.Service
{
    [ScopedDependency(ServiceType = typeof(IImageService))]
    public class ImageService : Base.Service, IImageService
    {
        private readonly IMemoryCache _memoryCache;

        private readonly string _cacheKeyPrefix = "Image_";

        private readonly IRepository<ImageEntity> _imageRepo;

        private readonly TimeSpan _cacheSliding = TimeSpan.FromHours(4);

        public ImageService(IUnitOfWork unitOfWork, IMemoryCache memoryCache) : base(unitOfWork)
        {
            _memoryCache = memoryCache;
            _imageRepo = unitOfWork.GetRepository<ImageEntity>();
        }

        public Task UpdateAsync(ImageModel model, CancellationToken cancellationToken = default)
        {
            using (var transaction = UnitOfWork.BeginTransaction())
            {
                CheckExist(model.Id);

                var image = model.MapTo<ImageEntity>();

                if (string.IsNullOrWhiteSpace(image.ProjectId))
                {
                    image.ProjectId = null;
                }

                _imageRepo.Update(image,
                    x => x.Name,
                    x => x.ProjectId,
                    x => x.IsFeatured
                );

                UnitOfWork.SaveChanges();

                //_imageCategoryRepo.DeleteWhere(x => x.ImageId == model.Id, true);

                //UnitOfWork.SaveChanges();

                //if (model.CategoryIds?.Any() == true)
                //{
                //    foreach (var categoryId in model.CategoryIds)
                //    {
                //        if (string.IsNullOrWhiteSpace(categoryId))
                //        {
                //            continue;
                //        }

                //        _imageCategoryRepo.Add(new ImageCategoryEntity
                //        {
                //            ImageId = model.Id,
                //            CategoryId = categoryId
                //        });
                //    }
                //}

                UnitOfWork.SaveChanges();

                cancellationToken.ThrowIfCancellationRequested();

                transaction.Commit();

                return Task.CompletedTask;
            }
        }

        public Task<List<ImageModel>> GetAllFeaturedImageAsync(CancellationToken cancellationToken = default)
        {
            var featuredImages = _imageRepo.Get(x => x.IsFeatured).OrderByDescending(x => x.LastUpdatedTime)
                .QueryTo<ImageModel>().ToList();

            return Task.FromResult(featuredImages);
        }

        public async Task<ImageModel> SaveAsync(CreateImageModel model, CancellationToken cancellationToken = default)
        {
            var image = model.MapTo<ImageEntity>();

            model.ImageContentBase64 = ImageHelper.GetBase64Format(model.ImageContentBase64);

            image.OriginalImageHash = SecurityHelper.EncryptSha256(model.ImageContentBase64);

            // Check Image is exist to prevent duplicate
            var imageModel = _imageRepo.Get(x => x.OriginalImageHash == image.OriginalImageHash).QueryTo<ImageModel>()
                .SingleOrDefault();

            if (imageModel != null)
            {
                return imageModel;
            }

            // Image Info
            var imageBytes = Convert.FromBase64String(model.ImageContentBase64);

            var imageInfo = ImageHelper.GetImageInfo(imageBytes);

            if (imageInfo != null)
            {
                image.IsImage = true;
                image.ImageDominantHexColor = imageInfo.DominantHexColor;
                image.ImageWidthPx = imageInfo.WidthPx;
                image.ImageHeightPx = imageInfo.HeightPx;
                image.MimeType = imageInfo.MimeType;
                image.Extension = imageInfo.Extension;

                // Resize
                imageBytes = ResizeImage(imageBytes,
                    imageInfo.WidthPx,
                    imageInfo.HeightPx,
                    model.ImageMaxWidthPx,
                    model.ImageMaxHeightPx);

                // Compress
                imageBytes = CompressImage(imageBytes);

                imageInfo = ImageHelper.GetImageInfo(imageBytes);

                image.ImageWidthPx = imageInfo.WidthPx;
                image.ImageHeightPx = imageInfo.HeightPx;
            }
            else
            {
                image.Extension = Path.GetExtension(image.Name);

                image.MimeType = string.IsNullOrWhiteSpace(image.Extension)
                    ? "application/octet-stream"
                    : MimeTypeHelper.GetMimeType(image.Extension);
            }

            image.ContentLength = imageBytes.Length;

            // Proceed Save Image

            var imageStorageName = image.Id + image.Extension;
            image.StorageLocation = SystemHelper.GetSaveImageLocation(imageStorageName);

            var storageLocationFullPath = PathHelper.GetFullPath(image.StorageLocation);

            // Save Image
            File.WriteAllBytes(storageLocationFullPath, imageBytes);

            cancellationToken.ThrowIfCancellationRequested();

            _imageRepo.Add(image);

            UnitOfWork.SaveChanges();

            imageModel = await GetByIdAsync(image.Id, cancellationToken);

            return imageModel;
        }

        public byte[] ResizeImage(byte[] imageBytes,
            int originalWidthPx,
            int originalHeightPx,
            int? maxWidthPx,
            int? maxHeightPx)
        {
            bool isResize = false;
            int newWidthPx = originalWidthPx;
            int newHeightPx = originalHeightPx;

            if (maxWidthPx.HasValue)
            {
                isResize = true;
                newWidthPx = Math.Min(newWidthPx, maxWidthPx.Value);
            }

            if (maxHeightPx.HasValue)
            {
                isResize = true;
                newHeightPx = Math.Min(newHeightPx, maxHeightPx.Value);
            }

            if (isResize)
            {
                imageBytes = ImageResizeHelper.Resize(imageBytes, newWidthPx, newHeightPx);
            }

            return imageBytes;
        }

        public byte[] CompressImage(byte[] imageBytes)
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT
                || Environment.OSVersion.Platform == PlatformID.Win32S
                || Environment.OSVersion.Platform == PlatformID.Win32Windows
                || Environment.OSVersion.Platform == PlatformID.WinCE
            )
            {
                using (var streamToCompress = new MemoryStream(imageBytes))
                {
                    var compressResult = ImageCompressor.Compress(streamToCompress);

                    // Update Image bytes and string base 64
                    imageBytes = compressResult.ResultFileStream.ToArray();
                }
            }

            return imageBytes;
        }

        public async Task<ImageDownloadModel> DownloadAsync(string id, CancellationToken cancellationToken = default)
        {
            var cacheKey = _cacheKeyPrefix + id;

            // Check Cache
            _memoryCache.TryGetValue(cacheKey, out var data);

            if (data is ImageDownloadModel imageDownloadModel)
            {
                // Ignore
            }
            else
            {
                imageDownloadModel = new ImageDownloadModel();

                // Get Data
                var imageModel = await GetByIdAsync(id, cancellationToken);

                if (imageModel == null)
                {
                    return null;
                }

                imageDownloadModel.ImageInfo = imageModel;

                var imageFullPath = PathHelper.GetFullPath(imageDownloadModel.ImageInfo.StorageLocation);

                if (!File.Exists(imageFullPath))
                {
                    return null;
                }

                imageDownloadModel.ImageBytes = File.ReadAllBytes(imageFullPath);

                // Set Cache
                _memoryCache.Set(cacheKey, imageDownloadModel, new MemoryCacheEntryOptions
                {
                    SlidingExpiration = _cacheSliding
                });
            }

            // Increase Access Time

            var currentTotalAccess = _imageRepo.Get(x => x.Id == id).Select(x => x.TotalAccess).Single();

            currentTotalAccess++;

            _imageRepo.Update(new ImageEntity
            {
                Id = id,
                TotalAccess = currentTotalAccess,
                LastAccessTime = CoreHelper.SystemTimeNow
            }, x => x.TotalAccess, x => x.LastAccessTime);

            cancellationToken.ThrowIfCancellationRequested();

            UnitOfWork.SaveChanges();

            return imageDownloadModel;
        }

        // Get

        public Task<PagedResponseModel<ImageModel>> GetPagedAsync(PagedRequestModel model,
            CancellationToken cancellationToken = default)
        {
            var query = _imageRepo.Get();

            var total = query.Count();

            var items = query
                .QueryTo<ImageModel>()
                .OrderByDescending(x => x.CreatedTime)
                .Skip(model.Skip)
                .Take(model.Take).ToList();

            var pagedResponse = new PagedResponseModel<ImageModel> {Total = total, Items = items};

            return Task.FromResult(pagedResponse);
        }

        public Task<DataTableResponseModel<ImageModel>> GetDataTableAsync(DataTableRequestModel model,
            CancellationToken cancellationToken = default)
        {
            var query = _imageRepo.Get();

            var listData = query.QueryTo<ImageModel>();

            var result = listData.GetDataTableResponse(model);

            return Task.FromResult(result);
        }

        public Task<ImageModel> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var file = _imageRepo.Get(x => x.Id == id).QueryTo<ImageModel>().SingleOrDefault();

            return Task.FromResult(file);
        }

        // Delete

        public Task DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            using (var transaction = UnitOfWork.BeginTransaction())
            {
                CheckExist(id);

                var image = _imageRepo.Get(x => x.Id == id).Single();
                
                var imageFullPath = PathHelper.GetFullPath(image.StorageLocation);
                
                //_imageCategoryRepo.DeleteWhere(x => x.ImageId == id, true);
                
                //_imageRepo.Delete(image, true);

                //cancellationToken.ThrowIfCancellationRequested();

                //UnitOfWork.SaveChanges();

                //// Remove Physical File
                //if (File.Exists(imageFullPath))
                //{
                //    File.Delete(imageFullPath);
                //}
                
                // Clear Cache
                var cacheKey = _cacheKeyPrefix + id;
                _memoryCache.Remove(cacheKey);

                transaction.Commit();
               
                return Task.CompletedTask;
            }
        }

        #region Helper

        private void CheckExist(string id)
        {
            var isExist = _imageRepo.Get(x => x.Id == id).Any();

            if (!isExist)
            {
                throw new CoreException(nameof(ErrorCode.NotFound), "The image is not found.");
            }
        }

        #endregion
    }
}