#region	License

//--------------------------------------------------
// <License>
//     <Copyright> 2018 © Linh Nguyen </Copyright>
//     <Url> http://linhnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> TIGE </Project>
//     <File>
//         <Name> VerifyService.cs </Name>
//         <Created> 20/04/2018 6:50:54 PM </Created>
//         <Key> c9211419-02f0-4cd3-87ac-064059a194e6 </Key>
//     </File>
//     <Summary>
//         VerifyService.cs is a part of TIGE
//     </Summary>
// <License>
//--------------------------------------------------

#endregion License

using TIGE.Contract.Repository.Interfaces;
using TIGE.Contract.Service;
using Elect.DI.Attributes;
using Elect.Mapper.AutoMapper.IQueryableUtils;
using Elect.Mapper.AutoMapper.ObjUtils;
using Elect.Web.Api.Models;
using Elect.Web.DataTable.Models.Request;
using Elect.Web.DataTable.Models.Response;
using Elect.Web.DataTable.Processing.Response;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Elect.Core.SecurityUtils;
using Elect.Core.StringUtils;
using Flurl.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using TIGE.Core.Constants;
using TIGE.Core.Share.Constants;
using TIGE.Core.Share.Exceptions;
using TIGE.Core.Share.Models;
using TIGE.Core.Share.Models.Slot;
using TIGE.Core.Share.Utils;
using TIGE.Core.Utils;
using PagedRequestModel = TIGE.Core.Models.PagedRequestModel;
using SystemHelper = TIGE.Core.Utils.SystemHelper;
using QRCoder;
using ZXing;

namespace TIGE.Service
{
    [ScopedDependency(ServiceType = typeof(IVerifyService))]
    public class VerifyService : Base.Service, IVerifyService
    {
        public VerifyService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}