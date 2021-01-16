using System;
using System.Collections.Generic;
using System.Drawing;

namespace turtle
{
    class Program
    {
        static void Main(string[] args)
        {
            Point fieldSize = Point.Empty;
            List<Point> mines = new List<Point>();
            Point exitCoordinate = Point.Empty;
            KeyValuePair<char, Point> startCoordinate = new KeyValuePair<char, Point>();
            List<char> gameSequence = new List<char>();

            InputFileProcessor.TryProcessInputs(ref fieldSize, ref mines, ref exitCoordinate, ref startCoordinate, ref gameSequence);
            var sequence = new List<char> { 'M', 'M', 'M', 'M', 'M' };

            var game = new Game(fieldSize, mines, exitCoordinate, startCoordinate, gameSequence);
                       
            game.Play();
            game.PrintResult();

#if DEBUG
            game.PrintPositions();
            game.PrintRoute();
#endif
        }
    }
}
