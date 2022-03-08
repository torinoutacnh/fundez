using System.Threading.Tasks;
using TIGE.Contract.Service;
using TIGE.Core.Share.Constants;
using TIGE.Core.Share.Models.Authentication;
using TIGE.Core.Utils;
using TIGE.Utils;
using Elect.Web.Swagger.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TIGE.Core.Share.Exceptions;

namespace TIGE.Areas.Api.Controllers
{
    [AllowAnonymous]
    public class AuthController : BaseController
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        #region Token

        /// <summary>
        ///     Token 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <remarks>
        ///     Support GrantType: Refresh Token,Authorization Code (PKCE), Resource Owner Password, Client Credential.
        /// </remarks>
        [ShowInApiDoc]
        [HttpPost]
        [Route(ApiEndpoint.Auth.TokenEndpoint)]
        public async Task<IActionResult> Token([FromBody] RequestTokenModel model)
        {
            try
            {
                GrantTypeHelper.CheckAllowGenerateToken(model.GrantType);

                var tokenModel = await _authenticationService.GetTokenAsync(model, this.GetRequestCancellationToken())
                    .ConfigureAwait(true);

                return Ok(tokenModel);
            }
            catch (CoreException e)
            {
                ErrorModel errorModel = new ErrorModel(e);

                return BadRequest(errorModel);
            }
        }

        #endregion Token
    }
}