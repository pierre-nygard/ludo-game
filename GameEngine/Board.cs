using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace GameEngine
{
    [Table("Board")]
    public class Board
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BoardID { get; set; }
        public List<Piece> Pieces { get; set; }

        [NotMapped]
        public OuterPath OuterPath { get; set; }
        [NotMapped]
        public RedInnerPath RedPath { get; set; }
        [NotMapped]
        public BlueInnerPath BluePath { get; set; }
        [NotMapped]
        public GreenInnerPath GreenPath { get; set; }
        [NotMapped]
        public YellowInnerPath YellowPath { get; set; }

        private readonly int PADDING_LEFT = 5;
        private readonly int PADDING_TOP = 2;
        
        public Board New()
        {
            Pieces = new List<Piece>();
            return this;
        }

        public Board Build(List<Player> players)
        {
            OuterPath = (OuterPath)new OuterPath().Build();
            RedPath = (RedInnerPath)new RedInnerPath().Build();
            BluePath = (BlueInnerPath)new BlueInnerPath().Build();
            GreenPath = (GreenInnerPath)new GreenInnerPath().Build();
            YellowPath = (YellowInnerPath)new YellowInnerPath().Build();

            DistributeInnerPaths(players);

            return this;
        }

        /// <summary>
        /// Distributes the inner paths for every player using their PlayerType.
        /// </summary>
        /// <param name="players">The players in-game.</param>
        public void DistributeInnerPaths(List<Player> players) =>
            players.ForEach(p => {
                switch (p.PlayerType)
                {
                    case PlayerType.Red:
                        p.InnerPath = RedPath;
                        break;
                    case PlayerType.Blue:
                        p.InnerPath = BluePath;
                        break;
                    case PlayerType.Green:
                        p.InnerPath = GreenPath;
                        break;
                    case PlayerType.Yellow:
                        p.InnerPath = YellowPath;
                        break;
                    default:
                        new Exception("Could not find path for player type: " + p.PlayerType + ".");
                        break;
                }
            });

        public void Draw() 
        {
            OuterPath.Draw();
            RedPath.Draw();
            BluePath.Draw();
            GreenPath.Draw();
            YellowPath.Draw();
            DrawPieces();
        }

        public void DrawPieces() => Pieces.Where(p => p.InPlay).ToList().ForEach(p =>
        {
            Console.ForegroundColor = p.Player.GetColor();
            Console.BackgroundColor = ConsoleColor.White;
            Console.SetCursorPosition(p.X + PADDING_LEFT, p.Y + PADDING_TOP);
            Console.WriteLine(p.Visual);
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
        });

        public void MovePiece(Player player, int steps)
        {
            var piece = Pieces.Where(p => p.InPlay && p.Player == player && !p.Completed).FirstOrDefault();
            Path path = null;

            if (piece.Steps > 50)
            {
                path = player.InnerPath;
                CalculateInnerPathMovement(piece, path, steps);
            }
            else if (piece.Steps + steps > 50)
            {
                path = player.InnerPath;
                CalculatePathTransition(piece, path, steps);
            }
            else
            {
                path = OuterPath;
                CalculateOuterPathMovement(piece, path, steps);
            }

            CollisionHandle(CollisionDetection(piece));
        }

        public Piece CollisionDetection(Piece pieceToMove) => Pieces.FirstOrDefault(p => p.Equals(pieceToMove) && p != pieceToMove && !p.Completed);

        public void CollisionHandle(Piece existingPiece)
        {
            if (existingPiece == null)
                return;
            existingPiece.MoveOut();
        }

        public void PlacePiece(Player activePlayer)
        {
            var piece = Pieces
               .Where(p => p.Player == activePlayer && !p.InPlay && !p.Completed)
               .First();
            int[] playerStartPoint = activePlayer.GetStartPoint();
            piece.Move(playerStartPoint[0], playerStartPoint[1]);
        }

        public void CalculateInnerPathMovement(Piece piece, Path path, int steps)
        {
            var pathLength = path.Tiles.Count;
            var currentLocationIndex = path.Tiles
                .IndexOf(path.Tiles.First(t => t.Equals(piece)));
            int nextLocationIndex;

            if (currentLocationIndex == pathLength - 1)
            {
                if (steps != 6)
                    return;

                piece.PassGoal();
                return;
            }
            else if (currentLocationIndex + steps > pathLength - 1)
            {
                nextLocationIndex = (pathLength - 1) - Math.Abs(currentLocationIndex + steps - (pathLength - 1));
                if (nextLocationIndex < 1) nextLocationIndex = 0;
            }
            else
                nextLocationIndex = currentLocationIndex + steps;

            piece.Move(path.Tiles[nextLocationIndex].X, path.Tiles[nextLocationIndex].Y, steps);
        }

        public void CalculatePathTransition(Piece piece, Path path, int steps)
        {
            int nextLocationIndex = piece.Steps + (steps - 1) - 50;
            if (nextLocationIndex > path.Tiles.Count - 1)
                nextLocationIndex--;
            piece.Move(path.Tiles[nextLocationIndex].X, path.Tiles[nextLocationIndex].Y, steps);
        }

        public void CalculateOuterPathMovement(Piece piece, Path path, int steps)
        {
            var currentTile = path.Tiles.First(t => t.Equals(piece));

            int nextLocationIndex = path.Tiles.IndexOf(currentTile) + steps;

            if (nextLocationIndex >= path.Tiles.Count)
                nextLocationIndex -= path.Tiles.Count;

            piece.Move(path.Tiles[nextLocationIndex].X, path.Tiles[nextLocationIndex].Y, steps);
        }
    }
}
