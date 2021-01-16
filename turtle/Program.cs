using System;
using System.Collections.Generic;
using System.Drawing;

namespace turtle
{
    class Program
    {
        static void Main(string[] args)
        {
            InputFileReader.ReadFile();
            var sequence = new List<char> { 'M', 'M', 'R', 'M', 'M', 'R', 'M', 'L', 'M', 'M', 'S', 'M', 'M', 'E', 'M' };

            var game = new Game(new Point(10, 10), new Point(3, 3), new Point(4, 6), new List<Point> { new Point(5, 3), new Point(4, 3) }, sequence);
                       
            game.Play();
            game.PrintPositions();

#if DEBUG
            game.PrintRoute();
#endif
        }
    }
}
