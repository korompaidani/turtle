using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace turtle
{
    public class Game : IGame
    {
        private readonly IList<SmartPoint> minePositions = new List<SmartPoint>();
        private IList<SmartPoint> turtlePositions = new List<SmartPoint>();
        private SmartPoint fieldSize;
        private SmartPoint exitPoint;
        private IList<char> gameSequence;
        private string gameResult;
        private bool isSafeModeActive;

        /// <param name="fieldSize">Size of filed. This value is a 0 based range, not a coordinate</param>
        /// <param name="minePositions">This is a collection of mine coordinates</param>
        /// <param name="exitPosition">This is the exit coordinate position</param>
        /// <param name="turtleInitialPosition">This is the initial coordinate of the Turtle</param>
        /// <param name="gameSequence">This collection contains the moving and direction commands</param>
        /// <param name="safeMode">Safe mode is an option which results increased calculation time but checks that given (input.txt) coordinate data is in range</param>
        public Game(Point fieldSize, IList<Point> minePositions, Point exitPosition, KeyValuePair<char, Point> turtleInitialPosition, IList<char> gameSequence, bool safeMode = false)
        {
            this.gameSequence = gameSequence ??
              throw new ArgumentNullException(nameof(gameSequence));

            var mines = minePositions ??
              throw new ArgumentNullException(nameof(gameSequence));

            this.fieldSize = new SmartPoint(fieldSize);
            this.minePositions = mines.Select(x => new SmartPoint(x)).ToList();

            exitPoint = new SmartPoint(exitPosition);
            var initialPosition = new SmartPoint(turtleInitialPosition.Value);
            SetAbsoluteDirection(turtleInitialPosition.Key, ref initialPosition);
            turtlePositions.Add(initialPosition);

            isSafeModeActive = safeMode;
        }        

        public bool Play()
        {
            if (!CheckInitialParametersAreInRange()) { return false; }
            if (gameSequence.Count > 0)
            {
                foreach(var step in gameSequence)
                {
                    var result = DirectionHandler(step);
                    if(result == 1) { gameResult = "Success"; return true; }
                    if(result == 2) { gameResult = "Mine Hit"; return true; }
                    if(result == 3) { gameResult = "Out of Range"; return true; }
                }

                gameResult = "Still in Danger";
            }

            return true;
        }

        public void PrintResult()
        {
            Console.WriteLine(gameResult);
        }

        public void PrintPositions()
        {
            foreach(var position in turtlePositions)
            {
                Console.WriteLine($"({position.X}, {position.Y})");
            }            
        }

        public void PrintRoute()
        {
            string fieldSizeX = fieldSize.X.ToString();
            string fieldSizeY = fieldSize.Y.ToString();

            if(fieldSizeX.Length > 3 || fieldSizeY.Length > 3)
            {
                return;
            }

            Console.WriteLine();
            for (int x = fieldSize.X; x > -1; x--)
            {
                for (int y = 0; y < fieldSize.Y; y++)
                {
                    if (x == 0)
                    {
                        string forNumLength = y.ToString();
                        if(forNumLength.Length == 1) { Console.Write($" {y} "); }
                        if (forNumLength.Length == 2) { Console.Write($" {y}"); }
                        if (forNumLength.Length == 3) { Console.Write($"{y}"); }

                    }
                    else
                    {
                        if (turtlePositions.Any(p => (p.X == x && p.Y == y)))
                        {
                            Console.Write(" T ");
                        }
                        else
                        {
                            Console.Write("   ");
                        }
                    }
                    
                    if(y == fieldSize.Y - 1)
                    {
                        Console.Write($"\t{x}");
                        Console.WriteLine();
                    }
                }
            }
        }

        public IList<Point> GetResultForTest(out string resultMessage)
        {
            var resultList = new List<Point>();
            foreach (var pos in turtlePositions)
            {
                resultList.Add(new Point(pos.X, pos.Y));
            }

            resultMessage = gameResult;
            return resultList;
        }

        private byte DirectionHandler(char dirCommand)
        {
            var formerLast = turtlePositions[turtlePositions.Count - 1];
            SetAbsoluteDirection(dirCommand, ref formerLast);
            SetRelativeDirection(dirCommand, ref formerLast);

            if (dirCommand == 'M')
            {
                var step = formerLast.Move();

                if (exitPoint == step) { return 1; }
                if (minePositions.Any(x => x == step)) { return 2; }
                if (!step.IsPointInRange(fieldSize)) { return 3; }

                turtlePositions.Add(step);
            }

            return 0;
        }

        private void SetAbsoluteDirection(char dirCommand, ref SmartPoint formerStep)
        {
            if (dirCommand == 'N') { formerStep.SetNord(); }
            else if (dirCommand == 'S') { formerStep.SetSouth(); }
            else if (dirCommand == 'W') { formerStep.SetWest(); }
            else if (dirCommand == 'E') { formerStep.SetEast(); }
        }

        private void SetRelativeDirection(char dirCommand, ref SmartPoint formerStep)
        {
            if (dirCommand == 'R') { formerStep.ClockWise(); }
            else if (dirCommand == 'L') { formerStep.CounterClockWise(); }
        }

        private bool CheckInitialParametersAreInRange()
        {
            if (isSafeModeActive)
            {
                bool isStartPosValid = true;
                bool isExitPointValid = true;
                bool isMinePositionValid = true;

                if (turtlePositions.Count > 0)
                {
                    if (!turtlePositions[0].IsPointInRange(fieldSize)) { isStartPosValid = false; ErrorLog.AddErrorMessage("Start Position is out of range"); }
                }
                if (!exitPoint.IsPointInRange(fieldSize)) { isExitPointValid = false; ErrorLog.AddErrorMessage("Exit coordinate is out of range"); }
                if (minePositions.Count > 0)
                {
                    foreach (var pos in minePositions)
                    {
                        if (!pos.IsPointInRange(fieldSize)) { isMinePositionValid = false; ErrorLog.AddErrorMessage($"{pos} mine is out of range"); }
                    }
                }

                return isStartPosValid && isExitPointValid && isMinePositionValid;
            }

            return true;
        }
    }
}
