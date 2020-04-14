using System;
using System.Collections.Generic;
using System.Text;

namespace GameEngine
{
    public class Dice
    {
        public static Random Random { get; set; } = new Random();
        public static int Roll() => Random.Next(1, 6 + 1);
    }
}
