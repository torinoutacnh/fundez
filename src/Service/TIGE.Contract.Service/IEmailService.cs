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
using System.Threading;
using System.Threading.Tasks;
using TIGE.Core.Constants;

namespace TIGE.Contract.Service
{
    public interface IEmailService
    {
        Task SendAsync(string userId, EmailTemplate template, CancellationToken cancellationToken = default);

        Task SendVerifyWithdrawAsync(string withDrawId, CancellationToken cancellationToken = default);

        //fix
        Task SendVerifyWithdrawTigeAsync(string withDrawId, CancellationToken cancellationToken = default);
        Task SendVerifyTransferTigeAsync(string withDrawId, CancellationToken cancellationToken);


        void SendBuySlotById(string slotId);
        //fix
        void SendSellToken(string userId);

        Task SendVerifyDepositAsync(string depositId, in CancellationToken cancellationToken = default);
        Task SendVerifyDepositStackAsync(string depositId, in CancellationToken cancellationToken = default);

        Task SendVerifyWithdrawStackAsync(string withDrawId, CancellationToken cancellationToken = default);
        Task SendVerifyStackAsync(string stackid, CancellationToken cancellationToken = default);
        Task SendVerifyWithdrawUSDStackAsync(string withDrawId, CancellationToken cancellationToken = default);
    }
}