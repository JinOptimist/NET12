using Moq;
using Net12.Maze;
using Net12.Maze.Cells;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Net12.Test.Maze.Cells
{
    public class GoldMineTest
    {
        [Test]
        [TestCase(9, 12)]
        [TestCase(100, 103)]
        public void TryToStepTest(int moneyInit, int moneyResult)
        {
            //Preparing
            var mazeMock = new Mock<IMazeLevel>();
            var heroMock = new Mock<IHero>();

            heroMock.SetupProperty(x => x.Money);
            heroMock.Object.Money = moneyInit;

            mazeMock
                .Setup(x => x.Hero)
                .Returns(heroMock.Object);

            var goldMine = new GoldMine(0, 0, mazeMock.Object);

            //Act      
            var i = goldMine.currentGoldMineMp;
            bool answer;
            do 
            {
                mazeMock.Verify(x => x.ReplaceCell(It.IsAny<BaseCell>()), Times.Never);
                answer = goldMine.TryToStep();              
                i++;
            } while (i < goldMine.goldMineMaxHp && !answer);

            //Assert
            Assert.AreEqual(i, goldMine.goldMineMaxHp, "We mustn't have possibility to step on the goldmine");
            Assert.AreEqual(moneyResult, heroMock.Object.Money);
            mazeMock.Verify(x => x.ReplaceCell(It.IsAny<BaseCell>()), Times.AtLeastOnce);
        }
    }
    
}
