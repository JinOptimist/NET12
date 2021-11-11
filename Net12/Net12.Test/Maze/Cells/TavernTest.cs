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
        private Mock<IMazeLevel> _mazeMock;
        private Mock<IHero> _heroMock;

        [SetUp]
        public void SetUp()
        {
            _mazeMock = new Mock<IMazeLevel>();
            _heroMock = new Mock<IHero>();

            _mazeMock
                .Setup(x => x.Hero)
                .Returns(_heroMock.Object);

            _heroMock.SetupProperty(x => x.Money);
            _heroMock.SetupProperty(x => x.CurrentFatigue);
        }

        [TestCase(1, 0, 0, 0)]
        [TestCase(2, 1, 4, 0)]
        [TestCase(2, 1, 50, 45)]
        public void TryToStep_RichHero(int moneyInit, int moneyResult, int currentFatiqueInit , int currentFatiqueResult)
        {
            //Preparing
            var tavern = new Tavern(0, 0, _mazeMock.Object);
            _heroMock.Object.Money = moneyInit;
            _heroMock.Object.CurrentFatigue = currentFatiqueInit;

            //Act
            var ansver = tavern.TryToStep();

            //Accert
            Assert.AreEqual(true, ansver);
            Assert.AreEqual(moneyResult, _heroMock.Object.Money);
            Assert.AreEqual(currentFatiqueResult, _heroMock.Object.CurrentFatigue);
        }

        [TestCase(0, 0)]
        [TestCase(0, 10)]
        [TestCase(0, 13)]
        public void TryToStep_NoMoneyNoHonie(int currentFatiqueInit)
        {
            //Preparing
            _heroMock.Object.Money = 0;
            _heroMock.Object.CurrentFatigue = currentFatiqueInit;

            var tavern = new Tavern(0, 0, _mazeMock.Object);

            //Act
            var answer = tavern.TryToStep();

            //Accert
            Assert.AreEqual(false, answer, "if Hero doesn't have money, he can't to step on the Tavern");
            Assert.AreEqual(currentFatiqueInit, _heroMock.Object.CurrentFatigue);
        }
    }
}