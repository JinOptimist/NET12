using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Moq;
using Net12.Maze;
using Net12.Maze.Cells;

namespace Net12.Test.Maze.Cells
{
    public class CoinTest
    {
        [Test]
        [TestCase(10, 11)]
        [TestCase(1, 2)]
        public void TryToStep(int coinCountInit, int heroMoneyInit)
        {
            //Preparing
            var mazeMock = new Mock<IMazeLevel>();
            var heroMock = new Mock<IHero>();

            heroMock.SetupProperty(x => x.Money);
            heroMock.Object.Money = heroMoneyInit;


            mazeMock
                .Setup(x => x.Hero)
                .Returns(heroMock.Object);
            

            var coin = new Coin(0, 0, mazeMock.Object, coinCountInit);


            //Act
            var answer = coin.TryToStep();

            //Assert
            Assert.AreEqual(coinCountInit, coin.CoinCount, "initian cointCount is incorrect");
            Assert.AreEqual(true, answer, "We must have possibility to step on the trap");
            Assert.AreEqual(coin.CoinCount+ heroMoneyInit, heroMock.Object.Money);
            //mazeMock.Verify(x => x[(It.IsAny<BaseCell>()), Times.AtLeastOnce);
        }
    }
}
