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
        public string Promotion { get; set; } = "";
        public bool IsGameOver { get; set; } = false;
        public string WonSide { get; set; } = "";
        public string EndgameType { get; set; } = "";
    }
}