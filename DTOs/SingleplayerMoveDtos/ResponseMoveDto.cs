using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoobChessServer.DTOs.SingleplayerMoveDtos
{
    public class ResponseMoveDto
    {
        public string From { get; set; } = "";
        public string To { get; set; } = "";
    }
}