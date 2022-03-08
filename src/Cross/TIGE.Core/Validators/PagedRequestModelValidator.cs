#region	License
//--------------------------------------------------
// <License>
//     <Copyright> 2018 © Linh Nguyen </Copyright>
//     <Url> http://linhnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> TIGE </Project>
//     <File>
//         <Name> PagedRequestModelValidator.cs </Name>
//         <Created> 20/04/2018 3:46:22 PM </Created>
//         <Key> 4fc301f6-5c2c-4f38-9441-58fa8c16042d </Key>
//     </File>
//     <Summary>
//         PagedRequestModelValidator.cs is a part of TIGE
//     </Summary>
// <License>
//--------------------------------------------------
#endregion License

using TIGE.Core.Models;
using FluentValidation;
using TIGE.Core.Share.Models.Slot;

namespace TIGE.Core.Validators
{
    public class ElectPagedRequestModelValidator : AbstractValidator<Elect.Web.Api.Models.PagedRequestModel>
    {
        public ElectPagedRequestModelValidator()
        {
            RuleFor(x => x.Skip)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.Take)
                .GreaterThan(0);
        }
    }

    public class PagedRequestModelValidator : AbstractValidator<PagedRequestModel>
    {
        public PagedRequestModelValidator()
        {
            Include(new ElectPagedRequestModelValidator());
        }
    } 
 
}