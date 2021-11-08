using Moq;
using Net12.Maze;
using Net12.Maze.Cells;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Net12.Test.Maze.Cells
{
    public class VitalityPotionTest
    {
        [Test]
        [TestCase(30, 35, 5)]
        [TestCase(100, 107, 7)]
        [TestCase(10, 20, 10)]
        public void TryToStepTest(int maxFatigueinit, int maxFatigueResult, int vitalityPotionEffect)
        {

            var mazeMock = new Mock<IMazeLevel>();
            var heroMock = new Mock<IHero>();
            heroMock.SetupProperty(x => x.MaxFatigue);
            heroMock.Object.MaxFatigue = maxFatigueinit;

            mazeMock.Setup(x => x.Hero).Returns(heroMock.Object);

            var vitalityPotion = new VitalityPotion(0, 0, mazeMock.Object, vitalityPotionEffect);


            var answer = vitalityPotion.TryToStep();

            Assert.AreEqual(true, answer, "We must have possibility to step on the trap");
            Assert.AreEqual(maxFatigueResult, heroMock.Object.MaxFatigue);
            mazeMock.Verify(x => x.ReplaceCell(It.IsAny<BaseCell>()), Times.AtLeastOnce);
        }

    }
}
