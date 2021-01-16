using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace turtle
{
    public class GameContext
    {
        private IGame game;
        private IInputFileProcessor inputFileProcessor;

        private Point fieldSize = Point.Empty;
        private List<Point> mines = new List<Point>();
        private Point exitCoordinate = Point.Empty;
        private KeyValuePair<char, Point> startCoordinate = new KeyValuePair<char, Point>();
        private List<char> gameSequence = new List<char>();

        public GameContext(IInputFileProcessor inputFileProcessor)
        {
            this.inputFileProcessor = inputFileProcessor ??
              throw new ArgumentNullException(nameof(inputFileProcessor));
        }

        public void PlayAGame()
        {
            if (inputFileProcessor.TryProcessInputs(ref fieldSize, ref mines, ref exitCoordinate, ref startCoordinate, ref gameSequence))
            {
                var game = new Game(fieldSize, mines, exitCoordinate, startCoordinate, gameSequence);

                game.Play();
                game.PrintResult();

#if DEBUG
                game.PrintPositions();
                game.PrintRoute();
#endif
            }
            else
            {
                ErrorLog.WriteErrorMessagesToConsole();
            }
        }
    }
}
