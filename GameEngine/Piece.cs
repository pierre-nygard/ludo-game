using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace GameEngine
{
    public enum PlayerColor
    {
        Red,
        Blue,
        Green,
        Yellow
    }

    [Table("Piece")]
    public class Piece : Point
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PieceID { get; set; }
        [ForeignKey("PlayerID")]
        public int PlayerID { get; set; }
        public Player Player { get; set; }
        public int Steps { get; set; }
        public bool Completed { get; set; }

        [NotMapped]
        public string Visual { get; } = "◙";
        [NotMapped]
        public bool InPlay { get => !(X == 0 && Y == 0); }

        public void New() => Completed = false;

        public void PassGoal()
        {
            this.Completed = true;
            Player.Score++;
            MoveOut();
        }

        public void MoveOut()
        {
            X = 0;
            Y = 0;
            Reset();
        }

        public void Reset() => Steps = 0;

        public void Move(int x, int y, int steps = 0)
        {
            X = x;
            Y = y;
            Steps += steps;
        }
    }
}
