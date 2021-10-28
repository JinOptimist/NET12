using Moq;
using Net12.Maze;
using Net12.Maze.Cells;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Net12.Test.Maze.Cells
{
    public class FountainTest
    {
        [Test]
        [TestCase(9, 8)]
        [TestCase(100, 99)]
        [TestCase(0, 0)]
        public void TryToStepTest(int hpInit, int hpResult)
        {
            //Preparing
            var mazeMock = new Mock<IMazeLevel>();
            var heroMock = new Mock<IHero>();

            heroMock.SetupProperty(x => x.Hp);
            heroMock.Object.Hp = hpInit;

            mazeMock
                .Setup(x => x.Hero)
                .Returns(heroMock.Object);
            var trap = new Trap(0,0, mazeMock.Object);

            //Act
            var answer = trap.TryToStep();

            //Assert
            Assert.AreEqual(true, answer, "We must have possibility to step on the Fountain");
            Assert.AreEqual(hpResult, heroMock.Object.Hp);
            mazeMock.Verify(x => x.ReplaceCell(It.IsAny<BaseCell>()), Times.AtLeastOnce);
        }
    }
}
