using GameEngine;
using System;
using System.Collections.Generic;
using System.Text;

namespace TheLudoGame.Modules
{
    public class ScoreBoard
    {
        private readonly int PaddingY = 26;
        private int PaddingX { get; set; } = 2;

        private readonly int WIDTH = 10;
        private readonly int HEIGHT = 4;

        public Player Player { get; set; }

        private static int scoreBoardCount = 0;

        private const int PADDING_X_MULTIPLIER = 14;

        public ScoreBoard(Player player)
        {
            Player = player;
            PaddingX += PADDING_X_MULTIPLIER * scoreBoardCount;
            scoreBoardCount++;
        }

        public void Draw()
        {
            Console.BackgroundColor = Player.GetColor();

            for (int x = 0; x < WIDTH; x++)
            {
                Console.SetCursorPosition(PaddingX + x, PaddingY);
                Console.Write(" ");
                Console.SetCursorPosition(PaddingX + x, PaddingY + HEIGHT);
                Console.Write(" ");
            }
            for (int y = 0; y < HEIGHT; y++)
            {
                Console.SetCursorPosition(PaddingX, PaddingY + y);
                Console.Write(" ");
                Console.SetCursorPosition(PaddingX + WIDTH - 1, PaddingY + y);
                Console.Write(" ");
            }
            Console.BackgroundColor = ConsoleColor.Black;

            UpdateScore();
        }

        private void UpdateScore()
        {
            Console.SetCursorPosition(PaddingX + 2, PaddingY + 1);
            Console.Write(Player.Score);
        }
    }
}
