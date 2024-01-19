using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoobChessServer.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = "example@gmail.com";
        public DateTime DateJoined { get; set; } = DateTime.Today;
    }
}