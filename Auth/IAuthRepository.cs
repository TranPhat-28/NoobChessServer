using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NoobChessServer.DTOs.LoginDtos;
using NoobChessServer.DTOs.UserDtos;

namespace NoobChessServer.Auth
{
    public interface IAuthRepository
    {
        // This will be call after OAuth complete
        Task<ServiceResponse<GetUserDto>> LoginWithGoogle(GoogleLoginDto googleLoginDto);
        Task<ServiceResponse<GetUserDto>> LoginWithFacebook(FacebookLoginDto facebookLoginDto);
    }
}