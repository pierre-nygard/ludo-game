using System;
using System.Collections.Generic;
using System.Text;

namespace GameEngine
{
    public class Tile : Point
    {
        public string Visual { get; set; }
        public ConsoleColor BackgroundColor { get; set; }

        public Tile() => Visual = " ";

        public void SetColor(string colorCode)
            =>
            BackgroundColor = colorCode switch
            {
                "R" => ConsoleColor.Red,
                "G" => ConsoleColor.Green,
                "B" => ConsoleColor.Blue,
                "Y" => ConsoleColor.DarkYellow,
                "W" => ConsoleColor.DarkGray,
                _ => ConsoleColor.Black,
            };
    }
}
