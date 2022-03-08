#region	License
//--------------------------------------------------
// <License>
//     <Copyright> 2018 © Linh Nguyen </Copyright>
//     <Url> http://linhnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> TIGE </Project>
//     <File>
//         <Name> Entity.cs </Name>
//         <Created> 20/04/2018 10:21:32 AM </Created>
//         <Key> 8b71ba77-8d67-445d-91d8-be6e9dd98918 </Key>
//     </File>
//     <Summary>
//         Entity.cs is a part of TIGE
//     </Summary>
// <License>
//--------------------------------------------------
#endregion License

using System;
using TIGE.Core.Share.Utils;

namespace TIGE.Contract.Repository.Models
{
    public abstract class Entity : Elect.Data.EF.Models.StringEntity
    {
        protected Entity()
        {
            Id = Guid.NewGuid().ToString("N");

            CreatedTime = LastUpdatedTime = CoreHelper.SystemTimeNow;
        }
    }
}