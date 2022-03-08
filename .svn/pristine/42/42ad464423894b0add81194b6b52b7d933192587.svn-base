#region	License
//--------------------------------------------------
// <License>
//     <Copyright> 2018 © Linh Nguyen </Copyright>
//     <Url> http://linhnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> TIGE </Project>
//     <File>
//         <Name> ApplicationModel.cs </Name>
//         <Created> 20/04/2018 10:07:19 AM </Created>
//         <Key> 002b8b36-a44f-4449-a0a1-f95e774e2c56 </Key>
//     </File>
//     <Summary>
//         ApplicationModel.cs is a part of TIGE
//     </Summary>
// <License>
//--------------------------------------------------
#endregion License

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Elect.Web.DataTable.Attributes;
using Elect.Web.DataTable.Models.Constants;
using Microsoft.AspNetCore.Mvc;
using TIGE.Core.Share.Constants;

namespace TIGE.Core.Share.Models.Application
{
    public class CreateApplicationModel
    {
        [Remote("CheckUniqueName", "App", HttpMethod = "POST", AdditionalFields = "Id", ErrorMessage = "The name is already exist, please try another.")]
        [DataTable(DisplayName = "Name", Order = 3)]
        public string Name { get; set; }

        [DataTable(DisplayName = "Description", Order = 4)]
        public string Description { get; set; }

        [Display(Name = "App Type")]
        [DataTable(DisplayName = "App Type", Order = 5)]
        public ApplicationType Type { get; set; } = ApplicationType.NonInteractiveClient;

        [Display(Name = "Domains")]
        [DataTable(DisplayName = "Domains", Order = 6, IsVisible = false)]
        public string Domains { get; set; }
    }

    public class ApplicationModel : CreateApplicationModel
    {
        [DataTable(IsVisible = false, Order = 1)]
        public string Id { get; set; }

        [Display(Name = "JWT Expiration (seconds)")]
        [DataTableIgnore]
        public int JwtExpirationSecond { get; set; } = 36000;

        [Display(Name = "Icon")]
        [DataTable(DisplayName = "Icon", Order = 2)]
        public string IconUrl { get; set; }

        [Display(Name = "Grant Types")]
        [DataTableIgnore]
        public List<GrantType> ListGrantType { get; set; }
        
        [DataTableIgnore]
        public string GrantTypes { get; set; }
        
        [DataTableIgnore]
        public string Secret { get; set; }

        [Display(Name = "Update Secret At")]
        [DataTableIgnore]
        public DateTimeOffset LastUpdatedSecret { get; set; }
        
        [DataTableIgnore]
        public string CreatedBy { get; set; }

        [DataTableIgnore]
        public string LastUpdatedBy { get; set; }

        [Display(Name = "Created At")]
        [DataTable(DisplayName = "Created At", Order = 999, IsVisible = false)]
        public DateTimeOffset CreatedTime { get; set; }

        [Display(Name = "Updated At")]
        [DataTable(DisplayName = "Updated At", Order = 1000, SortDirection = SortDirection.Descending)]
        public DateTimeOffset LastUpdatedTime { get; set; }
    }
}