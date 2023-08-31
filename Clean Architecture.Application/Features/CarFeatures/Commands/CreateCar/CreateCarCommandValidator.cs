using FluentValidation;
using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Features.CarFeatures.Commands.CreateCar
{
    public sealed class CreateCarCommandValidator : AbstractValidator<CreateCarCommand>
    {
        public CreateCarCommandValidator()
        {
            //Rules for Car Name
            RuleFor(x => x.Name).NotEmpty().WithMessage("Car name is cannot be blank!");
            RuleFor(x => x.Name).NotNull().WithMessage("Car name is cannot be null!");
            RuleFor(x => x.Name).MinimumLength(3).WithMessage("At least 3 chars");

            //Rules for Model
            RuleFor(x => x.Model).NotEmpty().WithMessage("Model name is cannot be blank!");
            RuleFor(x => x.Model).NotNull().WithMessage("Model name is cannot be null!");
            RuleFor(x => x.Model).MinimumLength(3).WithMessage("At least 3 chars");

            //Rules for Engine
            RuleFor(x => x.EnginePower).NotEmpty().WithMessage("Engine name is cannot be blank!");
            RuleFor(x => x.EnginePower).NotNull().WithMessage("Engine name is cannot be null!");
            RuleFor(x => x.EnginePower).GreaterThan(0).WithMessage("Greather than zero!");

        }
    }
}
