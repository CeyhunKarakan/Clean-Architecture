using CleanArchitecture.Application.Features.AuthFeatures.Commands.Login;
using CleanArchitecture.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Abstraction
{
    public interface IJwtProvider
    {
        Task<LoginCommandResponse> CreateTokenAsync(User user);
    }
}
