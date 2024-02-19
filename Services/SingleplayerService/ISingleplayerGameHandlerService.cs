using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NoobChessServer.DTOs.SingleplayerMoveDtos;

namespace NoobChessServer.Services.SingleplayerService
{
    public interface ISingleplayerGameHandlerService
    {
        Task<ServiceResponse<ResponseMoveDto>> SingleplayerGuessModeMoveHandler(GuessModeInputMoveDto guessModeInputMoveDto);
    }
}