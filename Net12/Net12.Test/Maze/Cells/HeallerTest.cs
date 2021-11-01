using Moq;
using Net12.Maze;
using Net12.Maze.Cells;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Net12.Test.Maze.Cells
{
    public class HealerTest
    {
        [Test]
        [TestCase(4, 2, 20, 100)]
        [TestCase(10, 5, 50, 100)]

        public void TryToStepTest(int moneyInput, int moneyResult, int hpInput, int hpResult)
        {
            var mazeMock = new Mock<IMazeLevel>();
            var heroMock = new Mock<IHero>();
            var healler = new Healer(0, 0, mazeMock.Object);

            mazeMock
               .Setup(x => x.Hero)
               .Returns(heroMock.Object);

            heroMock.SetupProperty(x => x.Hp);
            //heroMock.SetupProperty(x => x.Max_hp);
            heroMock.SetupProperty(x => x.Money);
            heroMock.Setup(x => x.Max_hp).Returns(100);

            heroMock.Object.Hp = hpInput;
            //heroMock.Object.Max_hp = 100;
            heroMock.Object.Money = moneyInput;

            var answer = healler.TryToStep();

            Assert.AreEqual(true, answer, "Hero must have the possibility to step on the Healler");
            Assert.AreEqual(moneyResult, heroMock.Object.Money);
            Assert.AreEqual(hpResult, heroMock.Object.Hp);
        }
    }
}