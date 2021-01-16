using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using turtle;


namespace turtleUnitTest
{
    class GameContextTest
    {
        private Mock<IInputFileProcessor> inputProcessorMock;
        private delegate bool GiveMeFakeData(ref Point fieldSize, ref List<Point> mines, ref Point exitCoordinate, ref KeyValuePair<char, Point>  startCoordinate, ref List<char> gameSequence);

        [SetUp]
        public void Setup()
        {
            Point fieldSize = Point.Empty;
            List<Point> mines = new List<Point>();
            Point exitCoordinate = Point.Empty;
            KeyValuePair<char, Point> startCoordinate = new KeyValuePair<char, Point>();
            List<char> gameSequence = new List<char>();

            inputProcessorMock = new Mock<IInputFileProcessor>();
            inputProcessorMock.Setup(p => p.TryProcessInputs(ref fieldSize, ref mines, ref exitCoordinate, ref startCoordinate, ref gameSequence))
                .Returns(new GiveMeFakeData((ref Point fieldSize, ref List<Point> mines, ref Point exitCoordinate, ref KeyValuePair<char, Point> startCoordinate, ref List<char> gameSequence) =>
                {
                    fieldSize = new Point(10, 10);
                    mines = new List<Point> { new Point(2, 2) };
                    exitCoordinate = new Point(5, 5);
                    startCoordinate = new KeyValuePair<char, Point>(' ', new Point(0, 0));
                    gameSequence = new List<char> { 'M' };
                    return true;
                }));
        }

        [TearDown]
        public void TearDown()
        {
        }

        [Test]
        public void TestNullParameter()
        {
            Assert.Throws<ArgumentNullException>(() =>
            new GameContext(null));
        }

        [Test]
        public void PlayAGameTest()
        {
            var gameContext = new GameContext(inputProcessorMock.Object);
            gameContext.PlayAGame();
        }       
    }
}
