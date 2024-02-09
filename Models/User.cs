using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoobChessServer.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Picture { get; set; } = "";
        public string Email { get; set; } = "example@gmail.com";
        public string DateJoined { get; set; } = DateTime.Now.ToString("dd/MM/yyyy");
    }
}