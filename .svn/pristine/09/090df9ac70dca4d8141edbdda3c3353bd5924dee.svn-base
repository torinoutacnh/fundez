using Elect.Core.StringUtils;
using Elect.Data.IO.ImageUtils;
using FluentValidation;
using TIGE.Core.Models;

namespace TIGE.Core.Validators
{
    public class CreateImageModelValidator : AbstractValidator<CreateImageModel>
    {
        public CreateImageModelValidator()
        {
            RuleFor(x => x.ImageContentBase64)
                .NotEmpty()
                .Must((x, o) => StringHelper.IsBase64(ImageHelper.GetBase64Format(o)))
                .WithMessage("File content must be base 64 format");
            
            RuleFor(x => x.ImageMaxWidthPx)
                .GreaterThan(0)
                .When(x => x.ImageMaxWidthPx.HasValue);
            
            RuleFor(x => x.ImageMaxHeightPx)
                .GreaterThan(0)
                .When(x => x.ImageMaxHeightPx.HasValue);
        }
    }
}