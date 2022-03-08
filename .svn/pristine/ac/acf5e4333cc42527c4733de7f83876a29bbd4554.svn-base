#region	License
//--------------------------------------------------
// <License>
//     <Copyright> 2018 © Linh Nguyen </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Elect </Project>
//     <File>
//         <Name> UserEntityMap.cs </Name>
//         <Created> 27/03/2018 4:59:44 PM </Created>
//         <Key> 36a6873f-3968-4f81-83ac-957ee8ef60b2 </Key>
//     </File>
//     <Summary>
//         UserEntityMap.cs is a part of Elect
//     </Summary>
// <License>
//--------------------------------------------------
#endregion License

using System;
using TIGE.Contract.Repository.Models.User;
using Elect.Data.EF.Services.Map;
using TIGE.Contract.Repository.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TIGE.Repository.Maps.User
{
    public class UserEntityMap : StringEntityTypeConfiguration<UserEntity>
    {
        public override void Map(EntityTypeBuilder<UserEntity> builder)
        {
            base.Map(builder);
           
            builder.ToTable(nameof(UserEntity));

            builder.HasIndex(x => x.Phone);

            builder.HasIndex(x => x.Email);

        }
    }
}