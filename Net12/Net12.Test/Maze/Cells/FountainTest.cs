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
        public void TryToStepTest()
        {
            //Arrange
            var mazeMock = new Mock<IMazeLevel>();
            var heroMock = new Mock<IHero>();
            mazeMock.Setup(x => x.Hero).Returns(heroMock.Object);
            var fount = new Fountain(0, 0, mazeMock.Object);
            //Act
            var answer = fount.TryToStep();

            //Assert
            Assert.AreEqual(true, answer);
           

        }
    }
}
