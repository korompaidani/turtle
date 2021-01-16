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
        IList<char> gameSequence;
        string gameResult;

        public Game(Point fieldSize, IList<Point> minePositions, Point exitPosition, KeyValuePair<char, Point> turtleInitialPosition, IList<char> gameSequence)
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
        }

        public void Play()
        {
            if(gameSequence.Count > 0)
            {
                foreach(var step in gameSequence)
                {
                    var result = DirectionHandler(step);
                    if(result == 1) { gameResult = "Success"; return; }
                    if(result == 2) { gameResult = "Mine Hit"; return; }
                    if(result == 3) { gameResult = "Out of Range"; return; }
                }

                gameResult = "Still in Danger";
            }
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
    }
}
