using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NoobChessServer.DTOs.SingleplayerMoveDtos;
using NoobChessServer.Services.SingleplayerService;

namespace NoobChessServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SingleplayerController : ControllerBase
    {
        private readonly ISingleplayerGameHandlerService _singleplayerGameHandlerService;

        public SingleplayerController(ISingleplayerGameHandlerService singleplayerGameHandlerService)
        {
            _singleplayerGameHandlerService = singleplayerGameHandlerService;
        }

        // This is Guess mode - no Authentication needed, but will not record the game
        [HttpPost("Guess")]
        public async Task<ActionResult<ServiceResponse<string>>> SingleplayerGuessModeReponse(GuessModeInputMoveDto guessModeInputMoveDto)
        {
            // Regular expression pattern for FEN validation
            string pattern = @"^([rnbqkpRNBQKP1-8]{1,8}\/){7}[rnbqkpRNBQKP1-8]{1,8} [wb] [-KQkq]{1,4} ([a-h][1-8]|-) \d+ \d+$";

            if (guessModeInputMoveDto.GuessModeInputFEN == "")
            {
                // Bad request
                var response = new ServiceResponse<string>
                {
                    IsSuccess = false,
                    Message = "No input move!"
                };

                return BadRequest(response);
            }
            // Validate FEN string
            else if (!Regex.IsMatch(guessModeInputMoveDto.GuessModeInputFEN, pattern))
            {
                // Bad request
                var response = new ServiceResponse<string>
                {
                    IsSuccess = false,
                    Message = "Invalid FEN string"
                };

                return BadRequest(response);
            }
            else
            {
                // Call the service
                var response = await _singleplayerGameHandlerService.SingleplayerGuessModeMoveHandler(guessModeInputMoveDto);
                return Ok(response);
            }
        }

        // This is Singleplayer mode - Authentication required, game state is saved and resume game is available
    }
}