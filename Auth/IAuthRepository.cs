using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NoobChessServer.DTOs.LoginDtos;

namespace NoobChessServer.Auth
{
    public interface IAuthRepository
    {
        // This will be call after OAuth complete
        Task<ServiceResponse<LoginResponseDto>> LoginWithGoogle(GoogleLoginDto googleLoginDto);
        Task<ServiceResponse<string>> LoginWithFacebook(FacebookLoginDto facebookLoginDto);
    }
}