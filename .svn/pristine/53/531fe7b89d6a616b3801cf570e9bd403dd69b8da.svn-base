#region	License
//--------------------------------------------------
// <License>
//     <Copyright> 2018 © Linh Nguyen </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> TIGE </Project>
//     <File>
//         <Name> RefreshTokenTokenEntityMap.cs </Name>
//         <Created> 16/04/2018 9:04:45 PM </Created>
//         <Key> 4d674ea6-dc16-4f88-b2f7-7202676eefb0 </Key>
//     </File>
//     <Summary>
//         RefreshTokenTokenEntityMap.cs is a part of TIGE
//     </Summary>
// <License>
//--------------------------------------------------
#endregion License

using System;
using TIGE.Contract.Repository.Models.Application;
using Elect.Data.EF.Services.Map;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TIGE.Repository.Maps.Application
{
    public class RefreshTokenEntityMap : StringEntityTypeConfiguration<RefreshTokenEntity>
    {
        public override void Map(EntityTypeBuilder<RefreshTokenEntity> builder)
        {
            base.Map(builder);

            builder.ToTable(nameof(RefreshTokenEntity));

            builder.HasIndex(x => x.RefreshToken);

            builder.HasOne(x => x.User).WithMany(x => x.RefreshTokens).HasForeignKey(x => x.UserId);
        }
    }
}