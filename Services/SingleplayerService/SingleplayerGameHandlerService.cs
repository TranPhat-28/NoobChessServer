using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Chess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using NoobChessServer.DTOs.SingleplayerMoveDtos;
using NoobChessServer.Stockfish;

namespace NoobChessServer.Services.SingleplayerService
{
    public class SingleplayerGameHandlerService : ISingleplayerGameHandlerService
    {
        private readonly StockfishService _stockfish;
        private readonly ObservableCollection<string> _output;

        public SingleplayerGameHandlerService()
        {
            _stockfish = new StockfishService();
            // this will contain all the output of the oracle
            _output = new ObservableCollection<string>();
            // in this way we redirect output to stdout of the main process
            _output.CollectionChanged += (sender, eventArgs) => Console.WriteLine(eventArgs.NewItems[0]);
            // in this way collect all the output
            _stockfish.DataReceived += (sender, eventArgs) => _output.Add(eventArgs.Data);

            _stockfish.Start();
        }
        public async Task<ServiceResponse<ResponseMoveDto>> SingleplayerGuessModeMoveHandler(GuessModeInputMoveDto guessModeInputMoveDto)
        {
            var response = new ServiceResponse<ResponseMoveDto>();

            try
            {
                // Check for endgame before asking stockfish
                var board = new ChessBoard();
                board = ChessBoard.LoadFromFen(guessModeInputMoveDto.GuessModeInputFEN);

                // Check for endgame
                if (board.IsEndGame)
                {
                    // Game ended - player (white) won
                    response.Data = new ResponseMoveDto()
                    {
                        IsGameOver = true,
                        WonSide = board.EndGame!.WonSide!.ToString(),
                        EndgameType = board.EndGame!.EndgameType.ToString(),
                    };
                }
                else
                {
                    // Ask Stockfish
                    _stockfish.SendUciCommand("ucinewgame");
                    _stockfish.SendUciCommand("isready");
                    // Perform calculating
                    _stockfish.SendUciCommand($"position fen {guessModeInputMoveDto.GuessModeInputFEN}");
                    _stockfish.SendUciCommand("go depth 10");

                    _stockfish.Wait(500); // Wait a little

                    while (!_output.Last().Contains("bestmove"))
                    {
                        Console.WriteLine("Still calculating...");
                        _stockfish.Wait(500); // Wait a little
                    }
                    var bestMove = _output.Last();
                    // Console.WriteLine(bestMove);

                    // Make the black move
                    board.Move(new Move(bestMove.Substring(9, 2), bestMove.Substring(11, 2)));
                    // Check if AI (black) won
                    if (board.IsEndGame)
                    {
                        // Game ended - AI (black) won
                        response.Data = new ResponseMoveDto()
                        {
                            From = bestMove.Substring(9, 2),
                            To = bestMove.Substring(11, 2),
                            Promotion = bestMove.Length >= 14 ? bestMove.Substring(13, 1) : "",
                            IsGameOver = true,
                            WonSide = board.EndGame!.WonSide!.ToString(),
                            EndgameType = board.EndGame!.EndgameType.ToString(),
                        };
                    }
                    else
                    {
                        // Game still going, so send the move back to the frontend
                        response.Data = new ResponseMoveDto()
                        {
                            From = bestMove.Substring(9, 2),
                            To = bestMove.Substring(11, 2),
                            Promotion = bestMove.Length >= 14 ? bestMove.Substring(13, 1) : ""
                        };
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.IsSuccess = false;
                response.Message = "Server encountered a problem and the game was exterminated.";
            }

            return response;
        }
    }
}