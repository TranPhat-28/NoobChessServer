using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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
                Console.WriteLine(bestMove);

                response.Data = new ResponseMoveDto()
                {
                    From = bestMove.Substring(9, 2),
                    To = bestMove.Substring(11, 2),
                    Promotion = bestMove.Substring(13, 1)
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.IsSuccess = false;
                response.Message = "Error generating move";
            }

            return response;
        }
    }
}