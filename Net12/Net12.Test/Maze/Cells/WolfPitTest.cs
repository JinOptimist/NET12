using Moq;
using Net12.Maze;
using Net12.Maze.Cells;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Net12.Test.Maze.Cells
{

    public class WolfPitTest
    {
        [Test]
        [TestCase(10, 9)]
        [TestCase(100, 99)]
        [TestCase(0, -1)]
        public void TryToStepTest(int hpInit, int hpResult)
        {
            //Preparing
            var mazeMock = new Mock<IMazeLevel>();
            var heroMock = new Mock<IHero>();

            heroMock.SetupProperty(x => x.Hp);
            heroMock.Object.Hp=hpInit;

            mazeMock.Setup(x => x.Hero).Returns(heroMock.Object);
            var wolfPit = new WolfPit(0,0,mazeMock.Object);

            //Act
            var answer= wolfPit.TryToStep();


            //Assert
            Assert.AreEqual(true, answer);
            Assert.AreEqual(hpResult, heroMock.Object.Hp);
        }

    }
}
