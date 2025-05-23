using AcceptDocs.SharedKernel.Dto;
using FluentValidation;

namespace AcceptDocs.Application.Validators
{
    public class RegisterAddDocumentFlowDtoValidator : AbstractValidator<AddDocumentFlowDto>
    {
        public RegisterAddDocumentFlowDtoValidator()
        {
            RuleFor(f => f.Name)
                .NotEmpty().WithMessage("Nazwa jest wymagana")
                .MaximumLength(20).WithMessage("Wymagana długość nazwy to 1-20 znaków");

            RuleFor(f => f.Description)
                .NotEmpty().WithMessage("Opis jest wymagany")
                .MinimumLength(5).WithMessage("Wymagana długość nazwy to 5-500 znaków")
                .MaximumLength(500).WithMessage("Wymagana długość nazwy to 5-500 znaków");
        }
    }
}
