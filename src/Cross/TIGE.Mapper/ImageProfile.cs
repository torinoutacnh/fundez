#region	License

//--------------------------------------------------
// <License>
//     <Copyright> 2018 © Linh Nguyen </Copyright>
//     <Url> http://linhnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> TIGE </Project>
//     <File>
//         <Name> UserProfile.cs </Name>
//         <Created> 20/04/2018 10:37:42 PM </Created>
//         <Key> bad13b73-1a16-4764-96c6-cde147859034 </Key>
//     </File>
//     <Summary>
//         UserProfile.cs is a part of TIGE
//     </Summary>
// <License>
//--------------------------------------------------

#endregion License

using System.Linq;
using AutoMapper;
using Elect.Mapper.AutoMapper.IMappingExpressionUtils;
using TIGE.Contract.Repository.Models;
using TIGE.Core.Models;

namespace TIGE.Mapper
{
    public class ImageProfile : Profile
    {
        public ImageProfile()
        {
            CreateMap<ImageEntity, ImageModel>().IgnoreAllNonExisting();
            
            CreateMap<CreateImageModel, ImageEntity>().IgnoreAllNonExisting();
            
            CreateMap<ImageModel, ImageDownloadModel>().IgnoreAllNonExisting();
            
            CreateMap<ImageModel, ImageEntity>().IgnoreAllNonExisting();
        }
    }
}