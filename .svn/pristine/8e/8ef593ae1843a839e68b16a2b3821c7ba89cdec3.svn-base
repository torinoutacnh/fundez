#region	License
//--------------------------------------------------
// <License>
//     <Copyright> 2018 © Linh Nguyen </Copyright>
//     <Url> http://linhnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> TIGE </Project>
//     <File>
//         <Name> UserModelValidator.cs </Name>
//         <Created> 20/04/2018 6:43:31 PM </Created>
//         <Key> e9138277-8823-4808-8eb4-5b93ece461da </Key>
//     </File>
//     <Summary>
//         UserModelValidator.cs is a part of TIGE
//     </Summary>
// <License>
//--------------------------------------------------
#endregion License

using FluentValidation;
using TIGE.Core.Share.Models;

namespace TIGE.Core.Share.Validators
{
    public class CreateWithdrawRequestModelValidator : AbstractValidator<CreateWithdrawRequestModel>
    {
        public CreateWithdrawRequestModelValidator()
        {
            RuleFor(x => x.Amount).GreaterThan(0);
        }
    }


    public class WithdrawRequestModelValidator : AbstractValidator<WithdrawRequestModel>
    {
        public WithdrawRequestModelValidator()
        {
            RuleFor(x => x.TxHash).NotEmpty().WithMessage("TxHash cannot be empty");
            RuleFor(x => x.TxHash).Matches("0x([A-Fa-f0-9]{64})").WithMessage("TxHash is invalid format");
        }
    }
}