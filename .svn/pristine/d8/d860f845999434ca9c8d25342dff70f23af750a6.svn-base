#region	License

//--------------------------------------------------
// <License>
//     <Copyright> 2018 © Linh Nguyen </Copyright>
//     <Url> http://linhnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> TIGE </Project>
//     <File>
//         <Name> IUserService.cs </Name>
//         <Created> 20/04/2018 5:26:05 PM </Created>
//         <Key> 2aabd68f-6cdc-4e1f-9e59-5cea5093571a </Key>
//     </File>
//     <Summary>
//         IUserService.cs is a part of TIGE
//     </Summary>
// <License>
//--------------------------------------------------

#endregion License

using Elect.Web.DataTable.Models.Request;
using Elect.Web.DataTable.Models.Response;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TIGE.Contract.Service.Base;
using TIGE.Core.Share.Models;
using TIGE.Core.Share.Models.Slot;
using TIGE.Core.Share.Models.User;

namespace TIGE.Contract.Service
{
    public interface IUserService :
        IGetPagedable<UserModel>,
        IGetDataTableable<UserModel>,
        IGetable<UserModel, string>,
        ICreateable<CreateUserModel, string>,
        IUpdateable<UserModel>,
        IDeleteable<string>
    {
        Task UpdateProfileAsync(ProfileModel model, CancellationToken cancellationToken = default);

        Task<UserModel> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        
        void CheckUniqueEmail(string email, string excludeId);

        void CheckUniquePhone(string phone, string excludeId);

        void CheckUniqueAddress(string address, string id);

        void CheckUniqueTxHash(string txHash, string id);

        Task<string> RegisterAsync(RegisterUserModel model, CancellationToken cancellationToken = default);
        Task<UserModel> GetByCodeAsync(string code, CancellationToken cancellationToken = default);

        Task UpdateProfileAsync(UpdateProfileModel model, CancellationToken cancellationToken = default);
        Task ConfirmUpdateProfileAsync(string token, CancellationToken cancellationToken = default);

        Task<List<DetailSlotRequestModel>> GetSlot(List<string> userIdList);
        Task<DashboardModel> GetDashBoardAsync(CancellationToken cancellationToken = default);
        string GenerateAuthyToken(string userId);
        Task ConfirmAuthy(string userId, CancellationToken cancellationToken = default);
        UpdateProfileModel GetProfileTemp(string userId);

        Task ResendConfirmRegisterAsync(string id, CancellationToken cancellationToken = default);
        Task ResendUpdateProfileAsync(CancellationToken cancellationToken = default);

        Task ConfirmUpdateStackProfileAsync(string token, CancellationToken cancellationToken = default);

        Task<DataTableResponseModel<StackUserModel>> GetStackDataTableAsync(DataTableRequestModel model, CancellationToken cancellationToken = default);
        Task UpdateStackUserAsync(StackUserModel model, CancellationToken cancellationToken);
        Task<StackUserModel> GetStackUserByIdAsync(string id, CancellationToken cancellationToken = default);
    }
}