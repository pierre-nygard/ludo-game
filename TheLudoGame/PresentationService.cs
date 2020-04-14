using GameEngine;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TheLudoGame.Context;
using TheLudoGame.Modules;

namespace TheLudoGame
{
    public class PresentationService : IHostedService
    {
        private readonly LudoContext ludoContext;
        public Game Game { get; set; }
        public GameConsole GameConsole { get; set; }
        public List<ScoreBoard> ScoreBoards { get; set; }
        public Menu Menu { get; set; }

        public PresentationService(LudoContext ludoContext)
        {
            this.ludoContext = ludoContext;
            Menu = new Menu(ludoContext);
            GameConsole = new GameConsole();
            ScoreBoards = new List<ScoreBoard>();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            ApplyGlobalAppSettings();

            Console.Clear();
            Menu.PrintMenuLogo();
            Console.WriteLine("GAME HISTORY:");
            Menu.GameHistoryView();
            Console.ReadLine();
            
            Console.Clear();
            Menu.PrintMenuLogo();
            Game =  Menu.GameSelectionMenu();
            Console.Clear();

            PopulateScoreBoards();

            Run();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        private Task Run()
        {
            while (true)
            {

                int dice = Dice.Roll();
                GameConsole.ConsolePrint(Game.Players.First(), $"rolls a {dice}");

                Game.DrawBoard();
                ScoreBoards.ForEach(s => s.Draw());

                Game.Action(dice);


                if (Game.Finished())
                {
                    Game.Completed = true;
                    Game.Update(ludoContext).Wait();
                    return Task.CompletedTask;
                }

                Game.NextPlayer();
                Game.Update(ludoContext).Wait();

                GameConsole.ConsolePrint("Autosave complete");
                Console.ReadLine();
            }
        }

        void PopulateScoreBoards() => Game.Players.ForEach(p => ScoreBoards.Add(new ScoreBoard(p)));

        private void ApplyGlobalAppSettings()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.CursorVisible = false;

            Console.WindowHeight = 35;
            Console.WindowWidth = 75;
            Console.BufferHeight = 35;
            Console.BufferWidth = 75;

            Console.Title = "TheLudoGame Group 6";
        }
    }
}
