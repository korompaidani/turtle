using System;
using System.Collections.Generic;
using System.Drawing;

namespace turtle
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputFileProcessor = new InputFileProcessor();
            var gameContext = new GameContext(inputFileProcessor);
            gameContext.PlayAGame();
        }
    }
}
