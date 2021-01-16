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
        private SmartPoint exitPoint;
        IList<char> gameSequence;
        string gameResult;

        private Point fieldSize = new Point(10, 10);

        public Game(Point fieldSize, Point turtleInitialPosition, Point exitPosition, IList<Point> minePositions, IList<char> gameSequence)
        {
            this.gameSequence = gameSequence ??
              throw new ArgumentNullException(nameof(gameSequence));

            var mines = minePositions ??
              throw new ArgumentNullException(nameof(gameSequence));
            
            this.minePositions = mines.Select(x => new SmartPoint(x)).ToList();

            exitPoint = new SmartPoint(exitPosition);
            turtlePositions.Add(new SmartPoint(turtleInitialPosition));
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
                }

                gameResult = "Still in Danger";
            }
        }

        public void PrintPositions()
        {
            foreach(var position in turtlePositions)
            {
                Console.WriteLine($"({position.X}, {position.Y})");
            }
            Console.WriteLine(gameResult);
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
                        Console.Write(y);
                    }
                    else
                    {
                        if (turtlePositions.Any(p => (p.X == x && p.Y == y)))
                        {
                            Console.Write("T");
                        }
                        else
                        {
                            Console.Write(" ");
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

            if (dirCommand == 'N') { formerLast.SetNord(); }
            else if (dirCommand == 'S') { formerLast.SetSouth(); }
            else if (dirCommand == 'W') { formerLast.SetWest(); }
            else if (dirCommand == 'E') { formerLast.SetEast(); }
            else if (dirCommand == 'R') { formerLast.ClockWise(); }
            else if (dirCommand == 'L') { formerLast.CounterClockWise(); }
            else if (dirCommand == 'M')
            {
                var step = formerLast.Move();

                if (exitPoint == step) { return 1; }
                if (minePositions.Any(x => x == step)) { return 2; }

                turtlePositions.Add(step); 
            }

            return 0;
        }
    }
}
