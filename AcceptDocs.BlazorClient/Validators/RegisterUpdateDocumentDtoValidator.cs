using AcceptDocs.SharedKernel.Dto;
using FluentValidation;

namespace AcceptDocs.BlazorClient.Validators
{
    public class RegisterUpdateDocumentDtoValidator : AbstractValidator<UpdateDocumentDto>
    {
        public RegisterUpdateDocumentDtoValidator()
        {

            RuleFor(d => d.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Nazwa jest wymagana")
                .MinimumLength(5).WithMessage("Wymagana długość nazwy to 5-50 znaków")
                .MaximumLength(500).WithMessage("Wymagana długość nazwy to 5-50 znaków");

            RuleFor(d => d.Description)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Opis jest wymagany")
                .MinimumLength(5).WithMessage("Wymagana długość opisu to 5-500 znaków")
                .MaximumLength(500).WithMessage("Wymagana długość opisu to 5-500 znaków");

            RuleFor(d => d.Value)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0).WithMessage("Wartość musi mieścić się w przedziale 1-" + (decimal.MaxValue - 1))
                .LessThan(decimal.MaxValue).WithMessage("Wartość musi mieścić się w przedziale 1-" + (decimal.MaxValue - 1));

            When(d => d.File != null, () =>
            {
                RuleFor(d => d.File)
                    .Cascade(CascadeMode.Stop)
                    .Must(f => f.Size <= 5 * 1024 * 1024).WithMessage("Dokument nie może być większy niż 5MB")
                    .Must(f => HasValidExtension(f.Name)).WithMessage("Niedozwolone rozszerzenie dokumentu. Dozwolone: .pdf, .jpg, .png, .docx, .doc")
                    .Must(f => f.Name.Length <= 100).WithMessage("Nazwa dokumentu jest zbyt długa (max 100 znaków).");
            });

            RuleFor(d => d.DocumentTypeId)
                .Must(dt => dt > 0).WithMessage("Rodzaj dokumentu jest wymagany");
        }

        private bool HasValidExtension(string fileName)
        {
            var allowedExtensions = new[] { ".pdf", ".jpg", ".png", ".docx", ".doc" };
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            return allowedExtensions.Contains(extension);
        }
    }
}
