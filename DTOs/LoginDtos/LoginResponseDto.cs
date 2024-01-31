using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoobChessServer.DTOs.LoginDtos
{
    public class LoginResponseDto
    {
        public string Sub { get; set; } = "";
        public string Name { get; set; } = "";
        public string Picture { get; set; } = "";
        public string Email { get; set; } = "";
        public string Locale { get; set; } = "";
        public string JWTToken { get; set; } = "";
    }
}