using Moq;
using Net12.Maze;
using Net12.Maze.Cells;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Net12.Test.Maze.Cells
{
    class PuddleTest
    {
        [Test]
        [TestCase("wap wap")]
        public void TryToStepTest(string WapMessage)
        {
            //Preparing
            var mazeMock = new Mock<IMazeLevel>();
            mazeMock.SetupProperty(x => x.Message);
            mazeMock.Object.Message = WapMessage;

            var puddle = new Puddle(0, 0, mazeMock.Object);
            //Act
            var answer = puddle.TryToStep();

            //Assert
            Assert.AreEqual(true, answer, "We must have possibility to step on the puddle");
            Assert.AreEqual(WapMessage, mazeMock.Object.Message);
        }
    }
}
