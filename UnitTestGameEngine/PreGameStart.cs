using GameEngine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTestGameEngine
{
    [TestClass]
    public class PreGameStart
    {
        [TestMethod]
        public void Add_Player_Above_Game_Player_Limit()
        {
            //Arrange
            Game game = new Game()
                .New()
                .AddPlayer(new Player { PlayerType = PlayerType.Red })
                .AddPlayer(new Player { PlayerType = PlayerType.Blue })
                .AddPlayer(new Player { PlayerType = PlayerType.Yellow })
                .AddPlayer(new Player { PlayerType = PlayerType.Green });

            //Act => Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => game.ValidateNewPlayerEntry(new Player()));
        }

        [TestMethod]
        public void Add_Player_With_Duplicate_PlayerType()
        {
            //Arrange
            Game game = new Game()
                .New()
                .AddPlayer(new Player { PlayerType = PlayerType.Red })
                .AddPlayer(new Player { PlayerType = PlayerType.Blue })
                .AddPlayer(new Player { PlayerType = PlayerType.Yellow });

            //Act => Assert
            Assert.ThrowsException<ArgumentException>(() => game.ValidateNewPlayerEntry(new Player { PlayerType = PlayerType.Yellow }));
        }

        [TestMethod]
        public void Start_Game_With_No_Players()
        {
            //Arrange
            Game game = new Game()
                .New();

            //Act => Assert
            Assert.ThrowsException<Exception>(() => game.CheckPlayerAmountRequirement());
        }


        public void Distribute_InnerPath_To_Players()
        {
            //Arrange
            Game game = new Game()
                .New()
                .AddPlayer(new Player { PlayerType = PlayerType.Red })
                .AddPlayer(new Player { PlayerType = PlayerType.Blue })
                .AddPlayer(new Player { PlayerType = PlayerType.Yellow })
                .AddPlayer(new Player { PlayerType = PlayerType.Green });

            //Act
            game.Board.DistributeInnerPaths(game.Players);

            //Assert
            Assert.Equals(game.Players[0].InnerPath, game.Board.RedPath);
            Assert.Equals(game.Players[1].InnerPath, game.Board.BluePath);
            Assert.Equals(game.Players[2].InnerPath, game.Board.YellowPath);
            Assert.Equals(game.Players[3].InnerPath, game.Board.GreenPath);
        }
    }
}
