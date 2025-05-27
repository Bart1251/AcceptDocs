using AcceptDocs.SharedKernel.Dto;
using FluentValidation;

namespace AcceptDocs.Application.Validators
{
    public class RegisterDocumentTypeDtoValidator : AbstractValidator<DocumentTypeDto>
    {
        public RegisterDocumentTypeDtoValidator()
        {
            RuleFor(dt => dt.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Nazwa jest wymagana")
                .MinimumLength(2).WithMessage("Wymagana długość nazwy to 2-30 znaków")
                .MaximumLength(30).WithMessage("Wymagana długość nazwy to 2-30 znaków");
        }
    }
}
