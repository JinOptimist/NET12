using Moq;
using Net12.Maze;
using Net12.Maze.Cells;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Net12.Test.Maze.Cells
{
    public class HealPotionTest
    {
        [Test]
        [TestCase(80,90)]
        [TestCase(0,10)]
        [TestCase(-10,0)]
        [TestCase(200,100)]
        public void TryToStepTest(int hpInit, int hpResult)
        {
            var mazeMock = new Mock<IMazeLevel>();
            var heroMock = new Mock<IHero>();

            heroMock.SetupProperty(x => x.Hp);
            heroMock.Object.Hp = hpInit;

            mazeMock
                .Setup(x => x.Hero)
                .Returns(heroMock.Object);
            var healPotion = new HealPotion(0, 0, mazeMock.Object);

            var answer = healPotion.TryToStep();

            Assert.AreEqual(true, answer);
            Assert.AreEqual(hpResult, heroMock.Object.Hp);

        }

    }

}
