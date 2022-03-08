#region	License

//--------------------------------------------------
// <License>
//     <Copyright> 2018 © Linh Nguyen </Copyright>
//     <Url> http://linhnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> TIGE </Project>
//     <File>
//         <Name> BootstrapperService.cs </Name>
//         <Created> 19/04/2018 6:45:55 PM </Created>
//         <Key> ee899866-2a1d-4f4e-abf2-2bcb5c395eac </Key>
//     </File>
//     <Summary>
//         BootstrapperService.cs is a part of TIGE
//     </Summary>
// <License>
//--------------------------------------------------

#endregion License

using System;
using System.Collections.Generic;
using System.IO;
using TIGE.Contract.Repository.Interfaces;
using TIGE.Contract.Repository.Models.Application;
using TIGE.Contract.Repository.Models.User;
using TIGE.Contract.Service;
using Elect.DI.Attributes;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Flurl.Http;
using TIGE.Contract.Repository.Models;
using TIGE.Core.Constants;
using TIGE.Core.Share.Constants;
using TIGE.Core.Share.Models.Application;
using TIGE.Core.Share.Utils;
using TIGE.Core.Utils;

namespace TIGE.Service
{
    [ScopedDependency(ServiceType = typeof(IBootstrapperService))]
    public class BootstrapperService : Base.Service, IBootstrapperService
    {
        private readonly IRepository<UserEntity> _userRepo;
        private readonly IRepository<ConfigurationEntity> _configurationRepo;
        private readonly IBootstrapper _bootstrapper;

        public BootstrapperService(IUnitOfWork unitOfWork, IBootstrapper bootstrapper) : base(unitOfWork)
        {
            _bootstrapper = bootstrapper;
            _userRepo = unitOfWork.GetRepository<UserEntity>();
            _configurationRepo = unitOfWork.GetRepository<ConfigurationEntity>();
            _userRepo = unitOfWork.GetRepository<UserEntity>();
        }

        public async Task InitialAsync(CancellationToken cancellationToken = default)
        {
            await _bootstrapper.InitialAsync(cancellationToken).ConfigureAwait(true);

            await InitialAccountAsync(cancellationToken).ConfigureAwait(true);
            await InitialConfigAsync(cancellationToken).ConfigureAwait(true);

            await InitialAppAsync(cancellationToken).ConfigureAwait(true);

            await InitialWebhookAsync(cancellationToken).ConfigureAwait(true);
        }

        private async Task InitialWebhookAsync(CancellationToken cancellationToken)
        {
            // get list
            //var url =$"https://api.authy.com/dashboard/json/application/webhooks";
            //var method = "GET";
            //var param = $"{HttpUtility.UrlEncode("app_api_key")}={SystemHelper.Setting.AuthyAppApiKey}&{HttpUtility.UrlEncode(HttpUtility.UrlEncode("access_key"))}={HttpUtility.UrlEncode(SystemHelper.Setting.AuthyAppAccessKey)}";

            ////var paramEncode = HttpUtility.UrlEncode(param);
            //var nonce = 1427849783.886085;

            //var data = nonce + "|"+ method + "|" + url + "|" + param;
            //var hashBase64 = CoreHelper.HashHMACBase64(SystemHelper.Setting.AuthyApiSigningKey.StringEncode(),
            //    data.StringEncode());

            //var request = new FlurlRequest(url+"?" + param);
            //request.Headers["X-Authy-Signature"] = hashBase64;
            //request.Headers["X-Authy-Signature-Nonce"] = nonce;

            //var response = await request.AllowAnyHttpStatus().GetJsonAsync();

            //var responseBody = response.Content != null ? await response.Content.ReadAsStringAsync() : null;

            return;
        }

        private Task InitialConfigAsync(CancellationToken cancellationToken)
        {
            if (!_configurationRepo.Get().Any())
            {
                var product = new ConfigurationEntity();
                product.AboutPage = "";
                product.SlotPrice = 200;
                product.SlotToToken = 100;
                product.TokenPrice = 1;
                product.Level1 = 10;
                product.Level2 = 4;
                product.Level3 = 2;
                product.Level4 = 2;
                product.Level5 = 2;
                product.Level6 = 1;
                product.Level7 = 1;
                product.Level8 = 1;
                product.Level9 = 1;
                product.Level10 = 6;


                product.ConditionLevel1 = 1;
                product.ConditionLevel2 = 2;
                product.ConditionLevel3 = 3;
                product.ConditionLevel4 = 5;
                product.ConditionLevel5 = 7;
                product.ConditionLevel6 = 9;
                product.ConditionLevel7 = 12;
                product.ConditionLevel8 = 15;
                product.ConditionLevel9 = 20;
                product.ConditionLevel10 = 50;



                product.SupportEmail = "support@fundez.co";
                product.TelegramLink = "fundezsupport";
                product.MinWithdraw = 20;
                product.TIGEChartRange = 1;

                _configurationRepo.Add(product);
                UnitOfWork.SaveChanges();
            }

            return Task.CompletedTask;
        }

       
        public Task RebuildAsync(CancellationToken cancellationToken = default)
        {
            _bootstrapper.RebuildAsync(cancellationToken).Wait(cancellationToken);

            return Task.CompletedTask;
        }

        private Task InitialAccountAsync(CancellationToken cancellationToken = default)
        {
            var isHaveAnyUser = _userRepo.Get().Any();

            if (isHaveAnyUser)
            {
                return Task.CompletedTask;
            }

            var systemNow = CoreHelper.SystemTimeNow;

            var user = new UserEntity
            {
                Email = "admin@fundez.co",
                EmailConfirmedTime = systemNow,
                Code = "ADMINSITE",
                Phone = "+0349660191",
                PhoneConfirmedTime = systemNow,
                Permission = string.Join(",", new List<int> { (int)Permission.Admin, (int)Permission.Manager, (int)Permission.Member }),
                PasswordHash = AuthenticationService.HashPassword("Password123@@", systemNow),
                PasswordLastUpdatedTime = systemNow
            };

            _userRepo.Add(user);

            cancellationToken.ThrowIfCancellationRequested();

            UnitOfWork.SaveChanges();

            return Task.CompletedTask;
        }

        private async Task InitialAppAsync(CancellationToken cancellationToken = default)
        {
            
        }


        
    }
}