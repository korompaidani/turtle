using NUnit.Framework;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using turtle;

namespace turtleUnitTest
{
    class InputProcessorTest
    {
        class InputFileProcessorTest
        {
            private const string testFilePath = @".\test.txt";
            private InputFileProcessor inputFileProcessor;
            private Point fieldSize = Point.Empty;
            private List<Point> mines = new List<Point>();
            private Point exitCoordinate = Point.Empty;
            private KeyValuePair<char, Point> startCoordinate = new KeyValuePair<char, Point>();
            private List<char> gameSequence = new List<char>();

            private List<string> testParameters = new List<string>
            {
                "5 4",
                "1,1 1,3 3,3",
                "4 2",
                "0 1 N",
                "R M L M M",
                "R M M M"
            };

            [TearDown]
            public void TearDown()
            {
                DeleteTestTxtFile();
            }

            [Test]
            public void TrueResult()
            {
                WriteFile();
                inputFileProcessor = new InputFileProcessor(testFilePath);
                var result = inputFileProcessor.TryProcessInputs(ref fieldSize, ref mines, ref exitCoordinate, ref startCoordinate, ref gameSequence);

                var expectedMinesList = new List<Point> { new Point(1, 1), new Point(1, 3), new Point(3, 3) };
                var expectedGameSequenceList = new List<char> { 'R', 'M', 'L', 'M', 'M', 'R', 'M', 'M', 'M' };

                Assert.IsTrue(result);
                Assert.AreEqual(new Point(5, 4), fieldSize);
                CompaireTwoLists(expectedMinesList, mines);
                Assert.AreEqual(new Point(4, 2), exitCoordinate);
                Assert.AreEqual('N', startCoordinate.Key);
                Assert.AreEqual(new Point(0, 1), startCoordinate.Value);
                CompaireTwoLists(expectedGameSequenceList, gameSequence);
            }

            [Test]
            [TestCase(3, "6 6", TestName = "OnlyCoordinatesAsStartCoordinates", Description = "Test that result is false due to fieldSize.")]
            public void OnlyCoordinatesAsStartCoordinates(int lineNumber, string lineValue)
            {
                WriteFile(lineNumber, lineValue);
                inputFileProcessor = new InputFileProcessor(testFilePath);
                var result = inputFileProcessor.TryProcessInputs(ref fieldSize, ref mines, ref exitCoordinate, ref startCoordinate, ref gameSequence);

                var expectedMinesList = new List<Point> { new Point(1, 1), new Point(1, 3), new Point(3, 3) };
                var expectedGameSequenceList = new List<char> { 'R', 'M', 'L', 'M', 'M', 'R', 'M', 'M', 'M' };

                Assert.IsTrue(result);
                Assert.AreEqual(new Point(6, 6), startCoordinate.Value);
                Assert.AreEqual(' ', startCoordinate.Key);
            }

            [Test]
            public void ThereIsNoFile()
            {
                WriteFile();
                inputFileProcessor = new InputFileProcessor("");
                var result = inputFileProcessor.TryProcessInputs(ref fieldSize, ref mines, ref exitCoordinate, ref startCoordinate, ref gameSequence);

                Assert.IsFalse(result);
            }

            [Test]
            [TestCase(0, "WRONGPARAMETER", TestName = "FalseResultDueToFieldSizeTest", Description = "Test that result is false due to fieldSize.")]
            public void FalseResultDueToFieldSizeTest(int lineNumber, string lineValue)
            {
                WriteFile(lineNumber, lineValue);
                inputFileProcessor = new InputFileProcessor(testFilePath);
                var result = inputFileProcessor.TryProcessInputs(ref fieldSize, ref mines, ref exitCoordinate, ref startCoordinate, ref gameSequence);
                Assert.IsFalse(result);
            }

            [Test]
            [TestCase(1, "WRONGPARAMETER", TestName = "FalseResultDueToMinesTest", Description = "Test that result is false due to wrong mines parameter.")]
            public void FalseResultDueToMinesTest(int lineNumber, string lineValue)
            {
                WriteFile(lineNumber, lineValue);
                inputFileProcessor = new InputFileProcessor(testFilePath);
                var result = inputFileProcessor.TryProcessInputs(ref fieldSize, ref mines, ref exitCoordinate, ref startCoordinate, ref gameSequence);
                
                Assert.IsFalse(result);
                Assert.AreEqual(new Point(5, 4), fieldSize);
                Assert.IsTrue(mines.Count == 0);
            }

            [Test]
            [TestCase(2, "WRONGPARAMETER", TestName = "FalseResultDueToExitCoordinateTest", Description = "Test that result is false due to wrong exit parameter.")]
            public void FalseResultDueToExitCoordinateTest(int lineNumber, string lineValue)
            {
                WriteFile(lineNumber, lineValue);
                inputFileProcessor = new InputFileProcessor(testFilePath);
                var result = inputFileProcessor.TryProcessInputs(ref fieldSize, ref mines, ref exitCoordinate, ref startCoordinate, ref gameSequence);

                var expectedMinesList = new List<Point> { new Point(1, 1), new Point(1, 3), new Point(3, 3) };

                Assert.IsFalse(result);
                Assert.AreEqual(new Point(5, 4), fieldSize);
                CompaireTwoLists(expectedMinesList, mines);
                Assert.AreNotEqual(new Point(4, 2), exitCoordinate);
            }

            [Test]
            [TestCase(3, "WRONGPARAMETER", TestName = "FalseResultDueToStartCoordinateTest", Description = "Test that result is false due to wrong start coordinate parameter.")]
            public void FalseResultDueToStartCoordinateTest(int lineNumber, string lineValue)
            {
                WriteFile(lineNumber, lineValue);
                inputFileProcessor = new InputFileProcessor(testFilePath);
                var result = inputFileProcessor.TryProcessInputs(ref fieldSize, ref mines, ref exitCoordinate, ref startCoordinate, ref gameSequence);

                var expectedMinesList = new List<Point> { new Point(1, 1), new Point(1, 3), new Point(3, 3) };

                Assert.IsFalse(result);
                Assert.AreEqual(new Point(5, 4), fieldSize);
                CompaireTwoLists(expectedMinesList, mines);
                Assert.AreEqual(new Point(4, 2), exitCoordinate);
                Assert.AreNotEqual(new Point(0, 1), startCoordinate.Value);
            }

            private void WriteFile(int lineNumber = 0, string customText = null)
            {
                using (StreamWriter file = new StreamWriter(testFilePath))
                {
                    int counter = 0;
                    foreach (string line in testParameters)
                    {
                        if (counter == lineNumber && customText != null)
                        {
                            file.WriteLine(customText);
                        }
                        else
                        {
                            file.WriteLine(line);
                        }
                        counter++;
                    }
                }
            }

            private void DeleteTestTxtFile()
            {
                if (File.Exists(testFilePath))
                {
                    File.Delete(testFilePath);
                }
            }

            private void CompaireTwoLists<T>(List<T> list1, List<T> list2) where T : struct
            {
                Assert.AreEqual(list1.Count, list2.Count);

                for (int i = 0; i < list1.Count; i++)
                {
                    if (list1[i] is Point && list2[i] is Point)
                    {
                        Assert.AreEqual(list1[i], list2[i]);
                    }
                    if (list1[i] is char && list2[i] is char)
                    {
                        Assert.AreEqual(list1[i], list2[i]);
                    }
                }
            }
        }
    }
}
