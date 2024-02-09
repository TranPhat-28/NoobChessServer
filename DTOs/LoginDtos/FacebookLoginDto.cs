using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoobChessServer.DTOs.LoginDtos
{
    public class FacebookLoginDto
    {
        public string Email { get; set; } = "";
        public string FacebookAccessToken { get; set; } = "";
    }
}