using AcceptDocs.Domain.Contracts;
using AcceptDocs.SharedKernel.Dto;
using FluentValidation;

namespace AcceptDocs.Application.Validators
{
    public class RegisterUpdateDocumentDtoValidator : AbstractValidator<UpdateDocumentDto>
    {
        public RegisterUpdateDocumentDtoValidator(IAppUnitOfWork appUnitOfWork)
        {
            RuleFor(d => d.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Nazwa jest wymagana")
                .MinimumLength(2).WithMessage("Wymagana długość nazwy to 2-50 znaków")
                .MaximumLength(500).WithMessage("Wymagana długość nazwy to 2-50 znaków");

            RuleFor(d => d.Description)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Opis jest wymagany")
                .MinimumLength(2).WithMessage("Wymagana długość opisu to 2-500 znaków")
                .MaximumLength(500).WithMessage("Wymagana długość opisu to 2-500 znaków");

            RuleFor(d => d.Value)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0).WithMessage("Wartość musi mieścić się w przedziale 1-" + (decimal.MaxValue - 1))
                .LessThan(decimal.MaxValue).WithMessage("Wartość musi mieścić się w przedziale 1-" + (decimal.MaxValue - 1));

            RuleFor(d => d.FileName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Załącznik jest wymagany")
                .Must(f => HasValidExtension(f)).WithMessage("Niedozwolone rozszerzenie załącznika. Dozwolone: .pdf, .jpg, .png, .docx, .doc")
                .Must(f => f.Length <= 100).WithMessage("Nazwa załącznika jest zbyt długa (max 100 znaków).");

            RuleFor(d => d.DocumentTypeId)
                .Cascade(CascadeMode.Stop)
                .Must(dt => dt > 0).WithMessage("Rodzaj dokumentu jest wymagany")
                .Custom((value, context) => {
                     if (appUnitOfWork.DocumentTypeRepository.Get(value) is null)
                         context.AddFailure("DocumentTypeId", "Ten rodzaj dokumentu nie istnieje");
                 });

            RuleFor(d => d.DocumentFlowId)
                .Custom((value, context) => {
                    if (appUnitOfWork.DocumentFlowRepository.Get(value??-1) is null && value is not null)
                        context.AddFailure("DocumentFlowId", "Ten przepływ nie istnieje");
                });
        }

        private bool HasValidExtension(string fileName)
        {
            var allowedExtensions = new[] { ".pdf", ".jpg", ".png", ".docx", ".doc" };
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            return allowedExtensions.Contains(extension);
        }
    }
}
