﻿using AcceptDocs.Domain.Contracts;
using AcceptDocs.SharedKernel.Dto;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcceptDocs.Application.Validators
{
    public class RegisterUpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
    {
        public RegisterUpdateUserDtoValidator(IAppUnitOfWork appUnitOfWork)
        {
            RuleFor(u => u.FirstName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Imię jest wymagane")
                .MinimumLength(2).WithMessage("Wymagana długość imienia to 2-30 znaków")
                .MaximumLength(30).WithMessage("Wymagana długość imienia to 2-30 znaków");

            RuleFor(u => u.LastName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Nazwisko jest wymagane")
                .MinimumLength(2).WithMessage("Wymagana długość nazwiska to 2-30 znaków")
                .MaximumLength(30).WithMessage("Wymagana długość nazwiska to 2-30 znaków");

            RuleFor(u => u.Login)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Login jest wymagany")
                .MinimumLength(2).WithMessage("Wymagana długość loginu to 2-20 znaków")
                .MaximumLength(20).WithMessage("Wymagana długość loginu to 2-20 znaków")
                .Custom((value, context) => {
                    if (appUnitOfWork.UserRepository.IsLoginUsed(value, context.InstanceToValidate.UserId))
                        context.AddFailure("Login", "Login jest już zajęty");
                });

            When(u => !string.IsNullOrEmpty(u.Password), () =>
            {
                RuleFor(u => u.Password)
                .Cascade(CascadeMode.Stop)
                .MinimumLength(5).WithMessage("Wymagana długość hasła to 5-30 znaków")
                .MaximumLength(30).WithMessage("Wymagana długość hasła to 5-30 znaków")
                .Matches("[A-Z]").WithMessage("Hasło musi zawierać co najmniej jedną dużą literę")
                .Matches("[a-z]").WithMessage("Hasło musi zawierać co najmniej jedną małą literę")
                .Matches("[0-9]").WithMessage("Hasło musi zawierać co najmniej jedną cyfrę")
                .Matches("[^a-zA-Z0-9]").WithMessage("Hasło musi zawierać co najmniej jeden znak specjalny");
            });

            RuleFor(u => u.Position)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Stanowisko jest wymagane")
                .MinimumLength(2).WithMessage("Wymagana długość stanowiska to 2-50 znaków")
                .MaximumLength(50).WithMessage("Wymagana długość stanowiska to 2-50 znaków");

            RuleFor(u => u.PositionLevelId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Poziom stanowiska jest wymagany")
                .Custom((value, context) => {
                    if (appUnitOfWork.PositionLevelRepository.Get(value) is null)
                        context.AddFailure("PositionLevelId", "Ten poziom stanowiska nie istnieje");
                });
        }
    }
}
