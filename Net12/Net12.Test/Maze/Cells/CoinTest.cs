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
        public void TryToStep(int coinCountInit, int coinCountInitResult)
        {
            //Preparing
            var mazeMock = new Mock<IMazeLevel>();
            var heroMock = new Mock<IHero>();

            heroMock.SetupProperty(x => x.Money);
            heroMock.Object.Money = coinCountInit;

            mazeMock
                .Setup(x => x.Hero)
                .Returns(heroMock.Object);

            //var coin = new Coin(0, 0, mazeMock.Object, heroMock.Object.Money);
            //Act
            //var answer = coin.TryToStep();

            //Assert
            //Assert.AreEqual(true, answer, "possibility to step exist");
            //Assert.AreEqual(coinCountInitResult, heroMock.Object.Money);
            //mazeMock.Verify(x => x.ReplaceCell(It.IsAny<BaseCell>()), Times.AtLeastOnce);
        }
    }
}
