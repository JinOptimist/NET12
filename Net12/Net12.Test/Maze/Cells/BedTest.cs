using Moq;
using Net12.Maze;
using Net12.Maze.Cells;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Net12.Test.Maze.Cells
{
    public class BedTest
    {
        [Test]
        [TestCase(50, 0)]

        public void TryToStepTest(int CurrentFatigue, int MaxFatigue)
        {
            //Preparing
            var mazeMock = new Mock<IMazeLevel>();
            var heroMock = new Mock<IHero>();

            heroMock.SetupProperty(x => x.CurrentFatigue);
            heroMock.Object.CurrentFatigue = CurrentFatigue;

            heroMock.SetupProperty(x => x.MaxFatigue);
            heroMock.Object.MaxFatigue = MaxFatigue;

            mazeMock
                .Setup(x => x.Hero)
                .Returns(heroMock.Object);

            var bed = new Bed(0, 0, mazeMock.Object);

            //Act
            var answer = bed.TryToStep();

            //Assert
            Assert.AreEqual(true, answer, "We must have possibility to step on the bed");
            Assert.AreEqual(MaxFatigue, heroMock.Object.CurrentFatigue);

        }
    }
}
