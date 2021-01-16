using NUnit.Framework;
using System.Collections.Generic;
using System.Drawing;
using turtle;

namespace turtleUnitTest
{
    class GameTest
    {
        public enum Direction { Vertical, Horizontal, Diagonal }

        private Point fieldSize = new Point(5, 5);
        private List<Point> mines = new List<Point> { new Point(0, 4) };
        private Point exitCoordinate = new Point(4, 0);
        private KeyValuePair<char, Point> startCoordinate = new KeyValuePair<char, Point>('N', new Point(0, 0));
        private List<char> gameSequence = new List<char> { 'M' };
        
        [Test]
        [TestCase("Still in Danger", 6, Direction.Vertical, 'M', 'M', 'M', 'M', 'M', Description = "TestStillInDangerScenarios Vertical Still In Range")]
        [TestCase("Still in Danger", 5, Direction.Horizontal, 'R', 'M', 'M', 'M', 'M', Description = "TestStillInDangerScenarios Horizontal Still In Range")]
        [TestCase("Still in Danger", 9, Direction.Diagonal, 'R', 'M', 'L', 'M', 'R', 'M', 'L', 'M', 'R', 'M', 'L', 'M', 'R', 'M', 'L', 'M', Description = "TestStillInDangerScenarios Diagonal Still In Range")]
        [TestCase("Still in Danger", 9, Direction.Diagonal, 'E', 'M', 'N', 'M', 'R', 'M', 'L', 'M', 'E', 'M', 'L', 'M', 'R', 'M', 'L', 'M', Description = "TestStillInDangerScenarios Diagonal Still In Range with mixed abs and rel dir")]
        public void TestStillInDangerScenarios(string expectedMessage, int expectedNumberOfSteps, Direction dir, params char[] sequence)
        {
            mines.Clear();
            fieldSize = new Point(5, 4);
            exitCoordinate = new Point(2, 4);
            startCoordinate = new KeyValuePair<char, Point>('N', new Point(0, 0));
            SetCustomSequence(sequence);

            var game = new Game(fieldSize, mines, exitCoordinate, startCoordinate, sequence);
            var play = game.Play();
            string resultMessage;
            var resultSteps = game.GetResultForTest(out resultMessage);

            Assert.IsTrue(play);
            Assert.IsTrue(resultMessage.Contains(expectedMessage));
            Assert.AreEqual(expectedNumberOfSteps, resultSteps.Count);
            Assert.True(CheckOneChangeStep(dir, resultSteps));
        }

        [Test]
        [TestCase("Out of Range", 6, Direction.Vertical, 'M', 'M', 'M', 'M', 'M', 'M', Description = "TestStillInDangerScenarios Vertical Still In Range + 1 Move")]
        [TestCase("Out of Range", 5, Direction.Horizontal, 'R', 'M', 'M', 'M', 'M', 'M', Description = "TestStillInDangerScenarios Horizontal Still In Range + 1 Move")]
        [TestCase("Out of Range", 9, Direction.Diagonal, 'R', 'M', 'L', 'M', 'R', 'M', 'L', 'M', 'R', 'M', 'L', 'M', 'R', 'M', 'L', 'M', 'R', 'M', Description = "TestStillInDangerScenarios Diagonal Still In Range + 1 Move")]
        [TestCase("Out of Range", 9, Direction.Diagonal, 'E', 'M', 'N', 'M', 'R', 'M', 'L', 'M', 'E', 'M', 'L', 'M', 'R', 'M', 'L', 'M', 'R', 'M', Description = "TestStillInDangerScenarios Diagonal Still In Range + 1 with mixed abs and rel dir")]
        public void TestOutOfRangeScenarios(string expectedMessage, int expectedNumberOfSteps, Direction dir, params char[] sequence)
        {
            mines.Clear();
            fieldSize = new Point(5, 4);
            exitCoordinate = new Point(2, 4);
            startCoordinate = new KeyValuePair<char, Point>('N', new Point(0, 0));
            SetCustomSequence(sequence);

            var game = new Game(fieldSize, mines, exitCoordinate, startCoordinate, sequence);
            var play = game.Play();
            string resultMessage;
            var resultSteps = game.GetResultForTest(out resultMessage);

            Assert.IsTrue(play);
            Assert.IsTrue(resultMessage.Contains(expectedMessage));
            Assert.AreEqual(expectedNumberOfSteps, resultSteps.Count);
            Assert.True(CheckOneChangeStep(dir, resultSteps));
        }

        [Test]
        [TestCase("Success", 0, 4, 5, Direction.Vertical, 'M', 'M', 'M', 'M', 'M', Description = "TestSuccessScenarios Vertical and reach the exit")]
        [TestCase("Success", 5, 0, 4, Direction.Horizontal, 'R', 'M', 'M', 'M', 'M', Description = "TestSuccessScenarios Horizontal and reach the exit")]
        [TestCase("Success", 1, 0, 8, Direction.Diagonal, 'R', 'M', 'L', 'M', 'R', 'M', 'L', 'M', 'R', 'M', 'L', 'M', 'R', 'M', 'L', 'M', Description = "TestSuccessScenarios Diagonal and reach the exit")]
        [TestCase("Success", 1, 0, 8, Direction.Diagonal, 'W', 'M', 'S', 'M', 'R', 'M', 'L', 'M', 'W', 'M', 'L', 'M', 'R', 'M', 'L', 'M', Description = "TestSuccessScenarios Diagonal and reach the exit with mixed abs and rel dir")]

        public void TestSuccessScenarios(string expectedMessage, int exitX, int exitY, int expectedNumberOfSteps, Direction dir, params char[] sequence)
        {
            mines.Clear();
            fieldSize = new Point(5, 4);
            exitCoordinate = new Point(exitX, exitY);
            startCoordinate = new KeyValuePair<char, Point>('S', new Point(5, 4));
            SetCustomSequence(sequence);

            var game = new Game(fieldSize, mines, exitCoordinate, startCoordinate, sequence);
            var play = game.Play();
            string resultMessage;
            var resultSteps = game.GetResultForTest(out resultMessage);

            Assert.IsTrue(play);
            Assert.IsTrue(resultMessage.Contains(expectedMessage));
            Assert.AreEqual(expectedNumberOfSteps, resultSteps.Count);
            Assert.True(CheckOneChangeStep(dir, resultSteps, true));
        }

        [Test]
        [TestCase("Mine Hit", 0, 4, 5, Direction.Vertical, 'M', 'M', 'M', 'M', 'M', Description = "TestMineHitScenarios Vertical and find a mine")]
        [TestCase("Mine Hit", 5, 0, 4, Direction.Horizontal, 'R', 'M', 'M', 'M', 'M', Description = "TestMineHitScenarios Horizontal and find a mine")]
        [TestCase("Mine Hit", 1, 0, 8, Direction.Diagonal, 'R', 'M', 'L', 'M', 'R', 'M', 'L', 'M', 'R', 'M', 'L', 'M', 'R', 'M', 'L', 'M', Description = "TestMineHitScenarios Diagonal and find a mine")]
        [TestCase("Mine Hit", 1, 0, 8, Direction.Diagonal, 'W', 'M', 'S', 'M', 'R', 'M', 'L', 'M', 'W', 'M', 'L', 'M', 'R', 'M', 'L', 'M', Description = "TestMineHitScenarios Diagonal and find a mine with mixed abs and rel dir")]

        public void TestMineHitScenarios(string expectedMessage, int mineX, int mineY, int expectedNumberOfSteps, Direction dir, params char[] sequence)
        {
            mines.Clear();
            mines.Add(new Point(mineX, mineY));
            fieldSize = new Point(5, 4);
            exitCoordinate = new Point(3, 3);
            startCoordinate = new KeyValuePair<char, Point>('S', new Point(5, 4));
            SetCustomSequence(sequence);

            var game = new Game(fieldSize, mines, exitCoordinate, startCoordinate, sequence);
            var play = game.Play();
            string resultMessage;
            var resultSteps = game.GetResultForTest(out resultMessage);

            Assert.IsTrue(play);
            Assert.IsTrue(resultMessage.Contains(expectedMessage));
            Assert.AreEqual(expectedNumberOfSteps, resultSteps.Count);
            Assert.True(CheckOneChangeStep(dir, resultSteps, true));
        }

        [Test]
        [TestCase(Description = "Test if safe mode is active and there is some out of range problems" )]
        public void TestSafeMode()
        {
            fieldSize = new Point(1, 1);
            exitCoordinate = new Point(2, 1);
            startCoordinate = new KeyValuePair<char, Point>('N', new Point(3, 3));
            mines = new List<Point> { new Point(5, 0) };

            var game = new Game(fieldSize, mines, exitCoordinate, startCoordinate, gameSequence, true);

            Assert.IsFalse(game.Play());
            
            var errorResult = ErrorLog.WriteErrorMessagesToConsole();
            Assert.True(errorResult.Contains("Start Position is out of range"));
            Assert.True(errorResult.Contains("Exit coordinate is out of range"));
            Assert.True(errorResult.Contains("mine is out of range"));
        }

        private void SetCustomSequence(char[] sequence)
        {
            gameSequence.Clear();
            foreach (var s in sequence)
            {
                gameSequence.Add(s);
            }
        }

        private bool CheckOneChangeStep(Direction dir, IList<Point> steps, bool isDecreased = false)
        {
            if(steps == null || steps.Count < 0) { return false; }

            int formerX = steps[0].X;
            int formerY = steps[0].Y;

            int diffNumber = 1;
            if (isDecreased) { diffNumber = -1; }

            for(int i = 1; i < steps.Count - 1; i++)
            {
                if (dir == Direction.Vertical)
                {
                    //x changes
                    if (steps[i].X == formerX + diffNumber && steps[i].Y == formerY) { formerX = steps[i].X; }
                    else { return false; }
                }
                else if (dir == Direction.Horizontal)
                {
                    //y changes
                    if (steps[i].Y == formerY + diffNumber && steps[i].X == formerX) { formerY = steps[i].Y; }
                    else { return false; }
                }
                else if (dir == Direction.Diagonal)
                {
                    //x and y changes
                    if(i % 2 == 0)
                    {
                        if (steps[i].Y == formerY && steps[i].X == formerX + diffNumber) { formerY = steps[i].Y; formerX = steps[i].X; }
                        else { return false; }
                    }
                    else
                    {
                        if (steps[i].Y == formerY + diffNumber && steps[i].X == formerX) { formerY = steps[i].Y; formerX = steps[i].X; }
                        else { return false; }
                    }
                }                
            }

            return true;
        }
    }
}
