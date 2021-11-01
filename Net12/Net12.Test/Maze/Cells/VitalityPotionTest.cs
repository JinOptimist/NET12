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
        [TestCase(30,35)]
        [TestCase(100,105)]
        [TestCase(10,15)]
        public void TryToStepTest(int maxFatigueinit, int maxFatigueResult)
        {
            
            var mazeMock = new Mock<IMazeLevel>();
            var heroMock = new Mock<IHero>();
            heroMock.SetupProperty(x => x.MaxFatigue);
            heroMock.Object.MaxFatigue = maxFatigueinit;

            mazeMock.Setup(x => x.Hero).Returns(heroMock.Object); 


            
            var vitalityPotion = new VitalityPotion(0, 0, mazeMock.Object,0);


            var answer = vitalityPotion.TryToStep();




            Assert.AreEqual(true,answer, "We must have possibility to step on the trap");
            Assert.AreEqual(maxFatigueResult, heroMock.Object.MaxFatigue +5);
            mazeMock.Verify(x => x.ReplaceCell(It.IsAny<BaseCell>()), Times.AtLeastOnce);
        }

    }
}
