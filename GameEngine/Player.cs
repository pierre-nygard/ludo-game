using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GameEngine
{
    [Table("Player")]
    public class Player
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PlayerID { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }
        [Column("GameID")]
        public int GameID { get; set; }
        public Game Game { get; set; }
        public PlayerType PlayerType { get; set; }
        [NotMapped]
        public Path InnerPath { get; set; }

        public ConsoleColor GetColor()
        {
            return PlayerType switch
            {
                PlayerType.Red => ConsoleColor.Red,
                PlayerType.Blue => ConsoleColor.Blue,
                PlayerType.Green => ConsoleColor.Green,
                PlayerType.Yellow => ConsoleColor.DarkYellow,
                _ => ConsoleColor.White
            };
        }

        public int[] GetStartPoint()
        {
            return PlayerType switch
            {
                PlayerType.Blue => new int[] { 1, 6 },
                PlayerType.Red => new int[] { 8, 1 },
                PlayerType.Yellow => new int[] { 6, 13 },
                PlayerType.Green => new int[] { 13, 8 },
                _ => throw new Exception("Could not find start point for type " + PlayerType + "."),
            };
        }
        
        public string ColorName() => GetType().ToString().Substring(11);
    }

    public enum PlayerType
    {
        Blue,
        Red,
        Yellow,
        Green
    }
}
