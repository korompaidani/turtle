using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace turtle
{
    public interface IInputFileProcessor
    {
        bool TryProcessInputs(ref Point fieldSize, ref List<Point> mines, ref Point exitCoordinate, ref KeyValuePair<char, Point> startCoordinate, ref List<char> gameSequence);
    }
}
