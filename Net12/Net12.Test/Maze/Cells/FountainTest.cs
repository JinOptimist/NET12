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
        [TestCase(21, 1)]
        [TestCase(20, 0)]
        [TestCase(19, 0)]
        public void TryToStepTest(int fatiqueInit, int fatiqueResult)
        {
            //Arrange
            var mazeMock = new Mock<IMazeLevel>();
            var heroMock = new Mock<IHero>();

            mazeMock.
                Setup(x => x.Hero).
                Returns(heroMock.Object);

            heroMock.SetupProperty(x => x.CurrentFatigue);
            heroMock.Object.CurrentFatigue = fatiqueInit;

            var fount = new Fountain(0, 0, mazeMock.Object);

            //Act
            var answer = fount.TryToStep();
            //Assert
            Assert.AreEqual(true, answer, "The ability to step on the cell is violated");
            Assert.AreEqual(fatiqueResult, heroMock.Object.CurrentFatigue, "Fatigue calculation is violated. ");


        }
    }
}
