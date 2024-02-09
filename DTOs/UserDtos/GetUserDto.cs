using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoobChessServer.DTOs.UserDtos
{
    public class GetUserDto
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public string Picture { get; set; } = "";
        public string Email { get; set; } = "";
        public DateTime DateJoined { get; set; }
        public string JWTToken { get; set; } = "";
    }
}