using CleanArchitecture.Domain.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Features.CarFeatures.Commands.CreateCar
{
    public sealed record CreateCarCommand(
        string Name,
        string Model,
        int EnginePower): IRequest<MessageResponse>;
}
