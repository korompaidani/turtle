using System;
using System.Collections.Generic;
using System.Drawing;

namespace turtle
{
    class Program
    {
        static void Main(string[] args)
        {
            InputFileProcessor.ReadFile();
            var sequence = new List<char> { 'M', 'M', 'M', 'M', 'M' };

            var game = new Game(new Point(5, 5), new List<Point> {  }, new Point(4, 6), new KeyValuePair<char, Point>('E', new Point(3, 3)), sequence);
                       
            game.Play();
            game.PrintResult();

#if DEBUG
            game.PrintPositions();
            game.PrintRoute();
#endif
        }
    }
}
