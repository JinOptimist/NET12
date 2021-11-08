using Moq;
using Net12.Maze;
using Net12.Maze.Cells;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
namespace Net12.Test.Maze.Cells
{
    class TeleportTest
    {
        [Test]
        [TestCase(1,2)]
        [TestCase(5,2)]
        [TestCase(9,9)]
        public void TryToTeleport(int x, int y)
        {
            //Prepearing
            var mazeMock = new Mock<IMazeLevel>();
            var mazeHero = new Mock<IHero>();
            var TeleportOutMock = new Mock<ITeleportOut>();

            mazeHero.SetupProperty(x => x.X);
            mazeHero.SetupProperty(y => y.Y);

       

            mazeMock.Setup(x => x.Hero).Returns(mazeHero.Object);
            
            TeleportOutMock.SetupProperty(x => x.X);
            TeleportOutMock.SetupProperty(y => y.Y);
            TeleportOutMock.Object.X = x;
            TeleportOutMock.Object.Y = y;

            var teleportIn = new TeleportIn(0, 0, mazeMock.Object, TeleportOutMock.Object);






            // Act
            teleportIn.TryToStep();

            // Assert
            Assert.AreEqual(TeleportOutMock.Object.X, mazeHero.Object.X, "Coord X not match");
            Assert.AreEqual(TeleportOutMock.Object.Y, mazeHero.Object.Y, "Coord Y not match");
          



        }

    }
}
