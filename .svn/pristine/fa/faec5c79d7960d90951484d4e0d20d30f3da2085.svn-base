using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TIGE.Contract.Service.Base;
using TIGE.Core.Models;

namespace TIGE.Contract.Service
{
    public interface IImageService :
        IGetPagedable<ImageModel>,
        IGetDataTableable<ImageModel>,
        IGetable<ImageModel, string>,
        IUpdateable<ImageModel>,
        IDeleteable<string>
    {
        Task<List<ImageModel>> GetAllFeaturedImageAsync(CancellationToken cancellationToken = default);
        
        Task<ImageModel> SaveAsync(CreateImageModel model, CancellationToken cancellationToken = default);

        byte[] ResizeImage(byte[] imageBytes,
            int originalWidthPx,
            int originalHeightPx,
            int? maxWidthPx,
            int? maxHeightPx);

        byte[] CompressImage(byte[] imageBytes);

        Task<ImageDownloadModel> DownloadAsync(string id, CancellationToken cancellationToken = default);
    }
}