using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TheLudoGame.Context;

namespace GameEngine
{
    [Table("Game")]
    public class Game
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GameID { get; set; }
        public DateTime LastAction { get; set; }
        public bool Completed { get; set; }
        [ForeignKey("Board")]
        public int BoardID { get; set; }
        public Board Board { get; set; }

        public List<Player> Players { get; set; }

        public Game New()
        {
            Completed = false;
            Players = new List<Player>();
            Board = new Board().New();
            return this;
        } 

        public Game AddPlayer(Player player)
        {
            ValidateNewPlayerEntry(player);
            player.Game = this;
            AddPieces(player);

            Players.Add(player);
            return this;
        }

        private void AddPieces(Player owner)
        {
            for (int i = 0; i < 4; i++)
            {
                Board.Pieces.Add(new Piece 
                { 
                    Player = owner, 
                    X = 0, 
                    Y = 0, 
                    Steps = 0,
                    Completed = false
                });
            }
        }

        /// <summary>
        /// Throws Exception if player of Type T already exists
        /// Throws Exception if Player count more than 4
        /// </summary>
        public void ValidateNewPlayerEntry(Player player)
        {
            if (Players.Count == 4)
                throw new ArgumentOutOfRangeException("Can't add more than four players.");
            else if (PlayerOfTypeExists(player.PlayerType))
                throw new ArgumentException("Player of type " + player.GetType() + " already exists.");
        }

        public Game Build()
        {
            CheckPlayerAmountRequirement();
            Board.Build(Players);
            return this;
        }

        public void DrawBoard() => Board.Draw(); 

        public bool Finished()
        {
            Player winner = GetWinner();

            if (winner == null)
                return false;

            return true;
        }

        public Player GetWinner() => Players.FirstOrDefault(x => x.Score == 4);

        public void Action(int result)
        {
            var player = Players.First();

            if (result == 6)
            {
                HighRoll(player);
            }
            else
            {
                Roll(player, result);
            }
        }

        public void Roll(Player player, int result)
        {
            if (Board.Pieces.Any(p => p.InPlay && p.Player == player && !p.Completed))
                Board.MovePiece(player, result);
        }

        public void HighRoll(Player activePlayer)
        {
            if (Board.Pieces.Any(p => p.InPlay && p.Player == activePlayer))
            {
                Board.MovePiece(activePlayer, 6);
            }
            else
            {
                Board.PlacePiece(activePlayer);
            }
        }

        /// <summary>
        /// Throws exception if game has less than 2 or higher than 4 players.
        /// </summary>
        public void CheckPlayerAmountRequirement()
        {
            if (Players.Count() < 2 || Players.Count() > 4)
                throw new Exception("Game needs 2-4 players to start.");
        }

        /// <summary>
        /// Turn succession. Next players turn (Rotates Player List).
        /// </summary>
        public void NextPlayer() => Players = Players.Skip(1).Concat(Players.Take(1)).ToList();

        public bool PlayerOfTypeExists(PlayerType playerType) => Players.Any(x => x.PlayerType == playerType);

        public async Task Update(LudoContext ludoContext)
        {
            LastAction = DateTime.Now;
            ludoContext.Game.Update(this);
            await ludoContext.SaveChangesAsync();
        }

        public void Save(LudoContext ludoContext)
        {
            LastAction = DateTime.Now;
            ludoContext.Game.Add(this);
            ludoContext.SaveChanges();
        }

        public static Game Load(LudoContext ludoContext) =>
            ludoContext.Game
                .Include(b => b.Board)
                .ThenInclude(p => p.Pieces)
                .ThenInclude(a => a.Player)
                .FirstOrDefault(g => g.Completed == false);
    }
}
