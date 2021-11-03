using Moq;
using Net12.Maze;
using Net12.Maze.Cells;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Net12.Test.Maze.Cells
{
    public class BlessTest
    {
        [Test]
        [TestCase(5, 15)]
        [TestCase(80, 30)]
        [TestCase(10, 20)]
        public void TryToStepTest(int hp, int maxHp)
        {
            //Preparing
            var mazeMock = new Mock<IMazeLevel>();
            var heroMock = new Mock<IHero>();

            heroMock.SetupProperty(x => x.Hp);
            heroMock.Object.Hp = hp;

            heroMock.SetupProperty(x => x.Max_hp);
            heroMock.Object.Max_hp = maxHp;

            mazeMock
                .Setup(x => x.Hero)
                .Returns(heroMock.Object);

            var bless = new Bless(0,0, mazeMock.Object);

            //Act
            var answer = bless.TryToStep();

            //Assert
            Assert.AreEqual(true, answer, "We must have possibility to step on the bless");
            Assert.AreEqual(maxHp, heroMock.Object.Hp);

        }
    }
}
