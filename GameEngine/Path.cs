using System.Collections.Generic;
using System.IO;
using System;

namespace GameEngine
{
    public abstract class Path
    {
        public List<Tile> Tiles { get; set; }
        public string PathUrl { get; set; }

        private readonly int PADDING_LEFT = 5;
        private readonly int PADDING_TOP = 2;

        public Path() => Tiles = new List<Tile>();

        public Path Build()
        {
            var pathList = File.ReadAllLines(PathUrl);
            foreach(var data in pathList)
            {
                var tileData = data.Split(',');
                var tile = new Tile
                {
                    X = int.Parse(tileData[0]),
                    Y = int.Parse(tileData[1])
                };
                tile.SetColor(tileData[2]);

                Tiles.Add(tile);
            }
            return this;
        }

        public void Draw() => Tiles.ForEach(t =>
            {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = t.BackgroundColor;
                Console.SetCursorPosition(t.X + PADDING_LEFT, t.Y + PADDING_TOP);
                Console.WriteLine(t.Visual);
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
            });
    }

    public class OuterPath : Path
    {
        public OuterPath()
        {
            PathUrl = "Paths/outer_path_coords.txt";
        }
    }

    public class RedInnerPath : Path
    {
        public RedInnerPath()
        {
            PathUrl = "Paths/red_path_coords.txt";
        }
    }

    public class BlueInnerPath : Path
    {
        public BlueInnerPath()
        {
            PathUrl = "Paths/blue_path_coords.txt";
        }
    }

    public class GreenInnerPath : Path
    {
        public GreenInnerPath()
        {
            PathUrl = "Paths/green_path_coords.txt";
        }
    }

    public class YellowInnerPath : Path
    {
        public YellowInnerPath()
        {
            PathUrl = "Paths/yellow_path_coords.txt";
        }
    }
}
