using NUnit.Framework;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using turtle;

namespace turtleTest
{
    [TestFixture]
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

        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void TestMethod1()
        {
            WriteFile();
            inputFileProcessor = new InputFileProcessor(testFilePath);
            inputFileProcessor.TryProcessInputs(ref fieldSize, ref mines, ref exitCoordinate, ref startCoordinate, ref gameSequence);
        }

        private void WriteFile(int lineNumber = 0, string customText = null)
        {
            using (StreamWriter file = new StreamWriter(testFilePath))
            {
                int counter = 0;
                foreach (string line in testParameters)
                {
                    if(counter == lineNumber && customText != null)
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
    }
}
