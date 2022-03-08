using System;
using System.Threading.Tasks;
using Elect.Web.Api;
using Elect.Web.Api.Models;
using Elect.Web.DataTable.Models;
using Elect.Web.DataTable.Models.Request;
using Elect.Web.DataTable.Processing.Response;
using Elect.Web.IUrlHelperUtils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TIGE.Contract.Service;
using TIGE.Core.Constants;
using TIGE.Core.Models;
using TIGE.Core.Share.Constants;
using TIGE.Core.Utils;
using TIGE.Utils;
using Swashbuckle.AspNetCore.Annotations;
using TIGE.Core.Share.Attributes.Auth;
using PagedRequestModel = Elect.Web.Api.Models.PagedRequestModel;

namespace TIGE.Areas.Api.Controllers
{
  public class ImageController : BaseController
    {
        private readonly IImageService _imageService;

        public ImageController(IImageService imageService)
        {
            _imageService = imageService;
        }
        
        /// <summary>
        ///     Get Paged File
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(ApiEndpoint.Image.GetPagedEndpoint)]
        [SwaggerResponse(StatusCodes.Status200OK, "List Image information.", typeof(PagedMetaModel<PagedRequestModel, ImageModel>))]
        public async Task<IActionResult> GetPaged([FromQuery] Core.Models.PagedRequestModel model)
        {
            var pagedResponse = await _imageService.GetPagedAsync(model, this.GetRequestCancellationToken()).ConfigureAwait(true);

            var pagedMeta = Url.GetPagedMeta(model, pagedResponse);

                foreach (var item in pagedMeta.Items)
                {
                    item.Url = Url.AbsoluteContent(PortalEndpoint.Image.DownloadEndpoint.Replace("{id}", item.Id));
                }

            return Ok(pagedMeta);
        }
        
        /// <summary>
        ///     Get DataTale File
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(ApiEndpoint.Image.GetDataTableEndpoint)]
        [Auth(Permission.Admin, Permission.Manager)]
        public async Task<DataTableActionResult<ImageModel>> GetDataTable([FromForm] DataTableRequestModel model)
        {
            var files = await _imageService.GetDataTableAsync(model);

            var result = files.GetDataTableActionResult(model, x => new
            {
                IsImage = x.IsImage ? Messages.Common.Yes : Messages.Common.No,
                IsFeatured = x.IsFeatured ? Messages.Common.Yes : Messages.Common.No,
                ContentLength = x.ContentLength.ToString("N0"),
                CategoryNames = string.Join(", ", x.CategoryNames),
                Url = Url.AbsoluteContent(PortalEndpoint.Image.DownloadEndpoint.Replace("{id}", x.Id))
            });

            return result;
        }
        
        /// <summary>
        ///     Get Detail Image 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(ApiEndpoint.Image.GetEndpoint)]
        [SwaggerResponse(StatusCodes.Status200OK, "Image information.", typeof(ImageModel))]
        public async Task<IActionResult> Get([FromRoute] string id)
        {
            var fileModel = await _imageService.GetByIdAsync(id, this.GetRequestCancellationToken()).ConfigureAwait(true);

            fileModel.Url = Url.AbsoluteContent(PortalEndpoint.Image.DownloadEndpoint.Replace("{id}", fileModel.Id));

            return Ok(fileModel);
        }

        /// <summary>
        ///     Upload File
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route(ApiEndpoint.Image.UploadBase64Endpoint)]
        [SwaggerResponse(StatusCodes.Status201Created, "Image information.", typeof(ImageModel))]
        public async Task<IActionResult> UploadBase64([FromBody] CreateImageModel model)
        {
            model.ImageMaxWidthPx = 1920;
            model.ImageMaxHeightPx = 1080;
            
            var fileModel = await _imageService.SaveAsync(model, this.GetRequestCancellationToken()).ConfigureAwait(true);

            fileModel.Url = Url.AbsoluteContent(PortalEndpoint.Image.DownloadEndpoint.Replace("{id}", fileModel.Id));

            var getDetailUri = new Uri(Url.AbsoluteAction("Get", "Image", new {id = fileModel.Id}));

            return Created(getDetailUri, fileModel);
        }

        /// <summary>
        ///     Upload File Multi-Part
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route(ApiEndpoint.Image.UploadMultiPartEndpoint)]
        [SwaggerResponse(StatusCodes.Status201Created, "Image information.", typeof(ImageModel))]
        public async Task<IActionResult> UploadMultiPart(IFormFile file, int? imageMaxWidthPx = null, int? imageMaxHeightPx = null)
        {
            var model = file.ToCreateFileModel(imageMaxWidthPx, imageMaxHeightPx);
                
            model.ImageMaxWidthPx = 1920;
            model.ImageMaxHeightPx = 1080;
            
            var fileModel = await _imageService.SaveAsync(model, this.GetRequestCancellationToken()).ConfigureAwait(true);

            fileModel.Url = Url.AbsoluteContent(PortalEndpoint.Image.DownloadEndpoint.Replace("{id}", fileModel.Id));

            var getDetailUri = new Uri(Url.AbsoluteAction("Get", "Image", new {id = fileModel.Id}));

            return Created(getDetailUri, fileModel);
        }

        /// <summary>
        ///     Delete Image 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route(ApiEndpoint.Image.DeleteEndpoint)]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Image deleted.")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            await _imageService.DeleteAsync(id, this.GetRequestCancellationToken()).ConfigureAwait(true);

            return NoContent();
        }
    }
}