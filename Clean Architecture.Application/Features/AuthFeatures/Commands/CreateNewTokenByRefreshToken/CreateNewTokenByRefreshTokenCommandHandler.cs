using CleanArchitecture.Application.Features.AuthFeatures.Commands.Login;
using CleanArchitecture.Application.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Features.AuthFeatures.Commands.CreateNewTokenByRefreshToken
{
    public sealed class CreateNewTokenByRefreshTokenCommandHandler :
        IRequestHandler<CreateNewTokenByRefreshTokenCommand, LoginCommandResponse>
    {
        private readonly IAuthService _authservice;

        public CreateNewTokenByRefreshTokenCommandHandler(IAuthService authservice)
        {
            _authservice = authservice;
        }

        public async Task<LoginCommandResponse> Handle(CreateNewTokenByRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            LoginCommandResponse response = await _authservice.CreateTokenByRefreshTokenAsync(request, cancellationToken);
            return response;
        }
    }
}
