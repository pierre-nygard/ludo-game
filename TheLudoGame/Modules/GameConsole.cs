using GameEngine;
using System;
using System.Collections.Generic;
using System.Text;

namespace TheLudoGame.Modules
{
    public class GameConsole
    {
        private int linesCount = 0;

        private readonly int CONSOLE_CAPACITY = 20;

        private readonly int PADDING_LEFT = 30;
        private readonly int PADDING_TOP = 3;

        public GameConsole() => Clear();

        public void ConsolePrint(string data)
        {
            if(linesCount > CONSOLE_CAPACITY - 1)
                Clear();

            Console.SetCursorPosition(PADDING_LEFT, linesCount + PADDING_TOP);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{data}");
            linesCount++;
        }

        public void ConsolePrint(Player player, string data)
        {
            if (linesCount > CONSOLE_CAPACITY - 1)
                Clear();

            Console.SetCursorPosition(PADDING_LEFT, linesCount + PADDING_TOP);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(player.Name + " [");
            Console.ForegroundColor = player.GetColor();
            Console.Write(player.PlayerType);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"] {data}");
            linesCount++;
        }

        private void Clear()
        {
            Console.Clear();
            Reset();
        }

        internal void Reset() => linesCount = 0;
    }
}
