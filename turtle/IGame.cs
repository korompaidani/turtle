using System.Collections.Generic;
using System.Drawing;

namespace turtle
{
    interface IGame
    {
        bool Play();
        void PrintResult();
        void PrintPositions();
        void PrintRoute();
        IList<Point> GetResultForTest(out string resultMessage);
    }
}
