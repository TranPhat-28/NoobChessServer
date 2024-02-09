using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoobChessServer.DTOs.LoginDtos
{
    public class GoogleLoginDto
    {
        public string Email { get; set; } = "";
        public string GoogleAccessToken { get; set; } = "";
    }
}