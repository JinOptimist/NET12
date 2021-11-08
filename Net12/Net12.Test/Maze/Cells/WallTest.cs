using Moq;
using NUnit.Framework;
using Net12.Maze;
using Net12.Maze.Cells;
using System;
using System.Collections.Generic;
using System.Text;

namespace Net12.Test.Maze.Cells
{
    
    class WallTest
    {
        [Test]
        public void TryToStep()
        {
            //Preparing
            var mazeMock = new Mock<IMazeLevel>();



            var wall = new Wall(0, 0, mazeMock.Object);


            //Act
            var answer = wall.TryToStep();


            //Assert
            Assert.AreEqual(false, answer, "We can't to step on the wall");
        }
    }
}
