using GameEngine;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestGameEngine
{
    [TestClass]
    public class GameCompletion
    {
        [TestMethod]
        public void Game_Not_Finished_Condition()
        {
            var player = new Player { Score = 0 };
            var game = new Game()
                .New()
                .Build()
                .AddPlayer(player);

            var expectedResult = false;

            Assert.AreEqual(expectedResult, game.Finished());
        }

        [TestMethod]
        public void Game_Finished_Condition()
        {
            var player = new Player { Score = 4 };
            var game = new Game()
                .New()
                .Build()
                .AddPlayer(player);

            var expectedResult = true;

            Assert.AreEqual(expectedResult, game.Finished());
        }
    }
}
