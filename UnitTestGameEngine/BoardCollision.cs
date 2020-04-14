using GameEngine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace UnitTestGameEngine
{
    [TestClass]
    public class BoardCollision
    {
        [TestMethod]
        public void Collision_Detection_Returns_CollidingPiece()
        {
            //Arrange
            var playerRed = new Player { PlayerType = PlayerType.Red };
            var playerBlue = new Player { PlayerType = PlayerType.Blue };
            Game game = new Game()
                .New()
                .AddPlayer(playerRed)
                .AddPlayer(playerBlue)
                .Build();

            var pieceRed = game.Board.Pieces.FirstOrDefault(p => p.Player == playerRed);
            pieceRed.Move(6, 0); 
            var pieceBlue = game.Board.Pieces.FirstOrDefault(p => p.Player == playerBlue);
            pieceRed.Move(6, 0);

            //Act
            Piece result = game.Board.CollisionDetection(pieceBlue);

            //Assert
            Assert.IsNotNull(result);
        }
    }
}
