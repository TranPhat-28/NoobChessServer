using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace NoobChessServer.Stockfish
{
    public class StockfishService
    {
        private readonly Process process = new Process();

        public StockfishService()
        {
            process.StartInfo = new ProcessStartInfo
            {
                FileName = $@"{System.IO.Directory.GetCurrentDirectory().ToString()}\Stockfish\stockfish.exe",
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true
            };

            process.OutputDataReceived += (sender, args) => this.DataReceived.Invoke(sender, args);
        }

        public event DataReceivedEventHandler DataReceived = (sender, args) => { };

        public void Start()
        {
            process.Start();
            process.BeginOutputReadLine();
        }

        public void Wait(int millisecond)
        {
            this.process.WaitForExit(millisecond);
        }

        public void SendUciCommand(string command)
        {
            process.StandardInput.WriteLine(command);
            process.StandardInput.Flush();
        }
    }
}