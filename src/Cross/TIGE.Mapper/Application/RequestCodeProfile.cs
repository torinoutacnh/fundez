#region	License

//--------------------------------------------------
// <License>
//     <Copyright> 2018 © Linh Nguyen </Copyright>
//     <Url> http://linhnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> TIGE </Project>
//     <File>
//         <Name> RequestCodeProfile.cs </Name>
//         <Created> 14/05/2018 2:23:30 PM </Created>
//         <Key> e60341ad-a214-4135-9571-7ffb6b73eb01 </Key>
//     </File>
//     <Summary>
//         RequestCodeProfile.cs is a part of TIGE
//     </Summary>
// <License>
//--------------------------------------------------

#endregion License

using AutoMapper;
using TIGE.Core.Models.Authentication;
using Elect.Mapper.AutoMapper.IMappingExpressionUtils;

namespace TIGE.Mapper.Application
{
    public class RequestCodeProfile : Profile
    {
        public RequestCodeProfile()
        {
            CreateMap<RequestCodeModel, AuthorizeCodeStorageModel>().IgnoreAllNonExisting();
        }
    }
}