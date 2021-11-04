using Moq;
using NUnit.Framework;
using Net12.Maze;
using Net12.Maze.Cells;
using System;
using System.Collections.Generic;
using System.Text;

namespace Net12.Test.Maze.Cells
{
    class TavernTest
    {
        [Test]
        [TestCase(0, 0, 0, 0)]
        [TestCase(1, 0, 0, 0)]
        [TestCase(2, 1, 4, 0)]
        [TestCase(2, 1, 50, 45)]


        public void TryToStep(int moneyInit, int moneyResult, int CurrentFatiqueInit , int CurrentFatiqueResult)
        {
            //Preparing
            var mazeMock = new Mock<IMazeLevel>();
            var heroMock = new Mock<IHero>();

            mazeMock
                .Setup(x => x.Hero)
                .Returns(heroMock.Object);

            var tavern = new Tavern(0, 0, mazeMock.Object);

            heroMock.SetupProperty(x => x.Money);
            heroMock.Object.Money = moneyInit;

            heroMock.SetupProperty(x => x.CurrentFatigue);
            heroMock.Object.CurrentFatigue = CurrentFatiqueInit;


            //Act
            var ansver = tavern.TryToStep();
            var ansverFatique = CurrentFatiqueResult;

            //Accert
            if (moneyInit <= 0)
            {
                Assert.AreEqual(false, ansver, "if Hero doesn't have money, he can't to step on the Tavern");
                Assert.AreEqual(moneyResult, heroMock.Object.Money);
            }
            else
            {
                if(CurrentFatiqueInit <= 5)
                {
                    CurrentFatiqueInit = 0;
                }
                else
                {
                    CurrentFatiqueInit -= 5;
                }
                Assert.AreEqual(true, ansver);
                Assert.AreEqual(moneyResult, heroMock.Object.Money);
                Assert.AreEqual(CurrentFatiqueResult, ansverFatique);
            }


        }
    }
}