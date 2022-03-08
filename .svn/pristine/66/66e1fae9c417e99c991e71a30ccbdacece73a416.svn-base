#region	License
//--------------------------------------------------
// <License>
//     <Copyright> 2018 © Linh Nguyen </Copyright>
//     <Url> http://linhnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> TIGE </Project>
//     <File>
//         <Name> IAuthenticationService.cs </Name>
//         <Created> 21/04/2018 5:45:41 PM </Created>
//         <Key> cbbc3cda-ab65-4ef0-a2ee-7dae4d933a38 </Key>
//     </File>
//     <Summary>
//         IAuthenticationService.cs is a part of TIGE
//     </Summary>
// <License>
//--------------------------------------------------
#endregion License

using TIGE.Core.Models.Authentication;
using System.Threading;
using System.Threading.Tasks;
using TIGE.Core.Share.Models.Authentication;
using TIGE.Core.Share.Models.User;

namespace TIGE.Contract.Service
{
    public interface IAuthenticationService
    {
        // Set Password

        Task<string> SendSetPasswordAsync(string id, CancellationToken cancellationToken = default);

        void CheckSetPasswordToken(string token, CancellationToken cancellationToken = default);

        Task SetPasswordAsync(SetPasswordModel model, CancellationToken cancellationToken = default);

        // Confirm Email

        Task<string> SendConfirmEmailAsync(string id, CancellationToken cancellationToken = default);

        void CheckConfirmEmailToken(string token, CancellationToken cancellationToken = default);

        Task ConfirmEmailAsync(ConfirmEmailModel model, CancellationToken cancellationToken = default);

        // Authentication

        Task<UserModel> SignInAsync(string userName, string password, CancellationToken cancellationToken = default);
        

        Task<TokenModel> GetTokenAsync(RequestTokenModel model, CancellationToken cancellationToken = default);

        Task ExpireRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);

        Task ExpireAllRefreshTokenAsync(string id, CancellationToken cancellationToken = default);

        // Code

        /// <summary>
        ///     Generate Code for Grantype Authorization (with PKCE) 
        /// </summary>
        /// <param name="model">            </param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<string> GetCodeAsync(RequestCodeModel model, CancellationToken cancellationToken = default);

        Task VerifyAuthy(string modelCode, string userId, CancellationToken cancellationToken = default);
    }
}