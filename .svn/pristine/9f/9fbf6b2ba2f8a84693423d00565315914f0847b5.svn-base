using System;
using System.Threading.Tasks;
using TIGE.Contract.Service;
using TIGE.Core.Utils;
using TIGE.Utils;
using Elect.Web.Api;
using Elect.Web.Api.Models;
using Elect.Web.DataTable.Models;
using Elect.Web.DataTable.Models.Request;
using Elect.Web.DataTable.Processing.Response;
using Elect.Web.IUrlHelperUtils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TIGE.Core.Constants;
using TIGE.Core.Share.Attributes.Auth;
using TIGE.Core.Share.Constants;
using TIGE.Core.Share.Models.User;
using TIGE.Core.Share.Utils;
using Swashbuckle.AspNetCore.Annotations;
using PagedRequestModel = TIGE.Core.Models.PagedRequestModel;

namespace TIGE.Areas.Api.Controllers
{
    [Auth(Permission.Admin, Permission.Manager)]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        ///     Get Paged User 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(ApiEndpoint.User.GetPagedEndpoint)]
        [SwaggerResponse(StatusCodes.Status200OK, "List User information.", typeof(PagedMetaModel<PagedRequestModel, UserModel>))]
        public async Task<IActionResult> GetPaged([FromQuery] PagedRequestModel model)
        {
            var pagedResponse = await _userService.GetPagedAsync(model, this.GetRequestCancellationToken())
                .ConfigureAwait(true);

            var pagedMeta = Url.GetPagedMeta(model, pagedResponse);

            return Ok(pagedMeta);
        }
        
        /// <summary>
        ///     Get DataTale User
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(ApiEndpoint.User.GetDataTableEndpoint)]
        public async Task<DataTableActionResult<UserModel>> GetDataTable([FromForm] DataTableRequestModel model)
        {
            var users = await _userService.GetDataTableAsync(model);

            var result = users.GetDataTableActionResult(model, x => new
            {
                IsBanned = x.IsBanned ? Messages.Common.Yes : Messages.Common.No,
            });

            return result;
        }

        /// <summary>
        ///     Get Detail User 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(ApiEndpoint.User.GetEndpoint)]
        [SwaggerResponse(StatusCodes.Status200OK, "User detail information.", typeof(UserModel))]
        public async Task<IActionResult> Get([FromRoute] string id)
        {
            var user = await _userService.GetByIdAsync(id, this.GetRequestCancellationToken()).ConfigureAwait(true);

            return Ok(user);
        }

        /// <summary>
        ///     Get Logged In User Profile: Basic info and permissions.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(ApiEndpoint.User.ProfileEndpoint)]
        [Auth]
        [SwaggerResponse(StatusCodes.Status200OK, "User profile information.", typeof(UserModel))]
        public async Task<IActionResult> GetProfile()
        {
            var user = await _userService.GetByIdAsync(LoggedInUser.Current.Id, this.GetRequestCancellationToken())
                .ConfigureAwait(true);

            return Ok(user);
        }

        /// <summary>
        ///     Create User 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route(ApiEndpoint.User.CreateEndpoint)]
        [SwaggerResponse(StatusCodes.Status201Created, "Id of new User.", typeof(Guid))]
        public async Task<IActionResult> Create([FromBody] CreateUserModel model)
        {
            var userId = await _userService.CreateAsync(model, this.GetRequestCancellationToken()).ConfigureAwait(true);

            Uri getDetailUri = new Uri(Url.AbsoluteAction("Get", "User", new {id = userId}));

            return Created(getDetailUri, new
            {
                id = userId
            });
        }

        /// <summary>
        ///     Update User 
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route(ApiEndpoint.User.UpdateEndpoint)]
        [SwaggerResponse(StatusCodes.Status204NoContent, "User updated.")]
        public async Task<IActionResult> Update([FromBody] UserModel model)
        {
            await _userService.UpdateAsync(model, this.GetRequestCancellationToken()).ConfigureAwait(true);

            return NoContent();
        }

        /// <summary>
        ///     Delete User 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route(ApiEndpoint.User.DeleteEndpoint)]
        [SwaggerResponse(StatusCodes.Status204NoContent, "User deleted.")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            await _userService.DeleteAsync(id, this.GetRequestCancellationToken()).ConfigureAwait(true);

            return NoContent();
        }


        [HttpPost]
        [Route(ApiEndpoint.User.GetStackDataTableEndpoint)]
        public async Task<DataTableActionResult<StackUserModel>> GetStackUserDataTable([FromForm] DataTableRequestModel model)
        {
            var users = await _userService.GetStackDataTableAsync(model);

            var result = users.GetDataTableActionResult(model, x => new
            {
                IsBanned = x.IsBanned ? Messages.Common.Yes : Messages.Common.No,
            });

            return result;
        }
    }
}