#region	License

//--------------------------------------------------
// <License>
//     <Copyright> 2018 © Linh Nguyen </Copyright>
//     <Url> http://linhnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> TIGE </Project>
//     <File>
//         <Name> RefreshTokenProfile.cs </Name>
//         <Created> 23/04/2018 6:51:43 PM </Created>
//         <Key> 23a972e3-47d7-42bd-89d5-cb658b73d7fe </Key>
//     </File>
//     <Summary>
//         RefreshTokenProfile.cs is a part of TIGE
//     </Summary>
// <License>
//--------------------------------------------------

#endregion License

using AutoMapper;
using TIGE.Contract.Repository.Models.Application;
using Elect.Mapper.AutoMapper.IMappingExpressionUtils;
using Elect.Web.HttpDetection.Models;

namespace TIGE.Mapper.Application
{
    public class RefreshTokenProfile : Profile
    {
        public RefreshTokenProfile()
        {
            CreateMap<DeviceModel, RefreshTokenEntity>().IgnoreAllNonExisting();
        }
    }
}