using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoobChessServer.Models
{
    public class GoogleUser
    {
        public string Sub { get; set; } = "";
        public string Name { get; set; } = "";
        public string GivenName { get; set; } = "";
        public string FamilyName { get; set; } = "";
        public string Picture { get; set; } = "";
        public string Email { get; set; } = "";
        public bool EmailVerified { get; set; } = false;
        public string Locale { get; set; } = "";
    }
}