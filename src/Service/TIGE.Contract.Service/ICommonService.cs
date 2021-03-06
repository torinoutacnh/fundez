#region	License
//--------------------------------------------------
// <License>
//     <Copyright> 2018 © Linh Nguyen </Copyright>
//     <Url> http://linhnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> TIGE </Project>
//     <File>
//         <Name> IEmailService.cs </Name>
//         <Created> 21/04/2018 7:30:42 PM </Created>
//         <Key> 9aaf18c2-cef2-4543-b34d-43f393a846c4 </Key>
//     </File>
//     <Summary>
//         IEmailService.cs is a part of TIGE
//     </Summary>
// <License>
//--------------------------------------------------
#endregion License

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TIGE.Core.Constants;
using TIGE.Core.Share.Models.Token;
using TIGE.Core.Share.Models.User;

namespace TIGE.Contract.Service
{
    public interface ICommonService
    {
        Task UpdateWalletBalance(string userId, CancellationToken cancellationToken = default);
        Task UpdateStackWalletBalance(string walletid, CancellationToken cancellationToken = default);
        Task<List<UserModel>> GetTotalMember(string userId, CancellationToken cancellationToken = default);
        Task UpdateCommissionBalance(string slotId, CancellationToken cancellationToken = default);
        Task<SellTokenPriceModel> CalculatingTokenPrice(double sellTokenTokenQuantity, CancellationToken cancellationToken = default);
    }
}