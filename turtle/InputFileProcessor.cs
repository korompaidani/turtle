using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace turtle
{
    public class InputFileProcessor
    {
        private readonly string textFile = @".\..\..\..\Input\input.txt";
        private const int fieldSizeLinePosition = 0;
        private const int mineCoordinatesLinePosition = 1;
        private const int exitCoordinatesLinePosition = 2;
        private const int startCoordinateLinePosition = 3;
        private const int gameSequenceFromLine = 4;
        private const int numberOfParameters = 5;

        public InputFileProcessor(string textFile = null)
        {
            if(textFile != null)
            {
                this.textFile = textFile;
            }
        }

        public bool TryProcessInputs(ref Point fieldSize, ref List<Point> mines, ref Point exitCoordinate, ref KeyValuePair<char, Point> startCoordinate, ref List<char> gameSequence)
        {
            var lines = new List<string>();
            ReadFile(ref lines);
            if(lines.Count < numberOfParameters) { ErrorLog.AddErrorMessage("Input file is empty"); return false; }

            if (!TryProcessFieldSize(in lines, out fieldSize)) { ErrorLog.AddErrorMessage("Field size parameter is wrong"); return false; }
            if(!TryProcessMineCoordinates(in lines, out mines)) { ErrorLog.AddErrorMessage("Mines parameter are wrong"); return false; }
            if(!TryProcessExitCoordinate(in lines, out exitCoordinate)) { ErrorLog.AddErrorMessage("Exit coordinate parameter is wrong"); return false; }
            if(!TryProcessStartCoordinate(in lines, out startCoordinate)) { ErrorLog.AddErrorMessage("Start coordinate parameter is wrong"); return false; }
            if(!TryProcessGameSequence(in lines, out gameSequence)) { ErrorLog.AddErrorMessage("Game sequence parameter is wrong"); return false; }

            return true;
        }

        private bool TryProcessFieldSize(in List<string> lines, out Point fieldSize) 
        {
            var line = lines[fieldSizeLinePosition];
            var coordinatesAsStringArray = line.Split(" ");

            int x = 0;
            int y = 0;
            if(coordinatesAsStringArray.Length < 2) { fieldSize = Point.Empty; return false; }
            if(!Int32.TryParse(coordinatesAsStringArray[0], out x)) { fieldSize = Point.Empty; return false; }
            if(!Int32.TryParse(coordinatesAsStringArray[1], out y)) { fieldSize = Point.Empty; return false; }

            fieldSize = new Point(x, y);
            return true;
        }

        private bool TryProcessMineCoordinates(in List<string> lines, out List<Point> mines)
        {
            mines = new List<Point>();
            var line = lines[mineCoordinatesLinePosition];
            var coordinatesAsStringArray = line.Split(" ");
            foreach(var coordString in coordinatesAsStringArray)
            {
                var xAndyinStringArray = coordString.Split(",");

                int x = 0;
                int y = 0;
                if (coordinatesAsStringArray.Length < 2) { return false; }
                if (!Int32.TryParse(xAndyinStringArray[0].TrimStart().TrimEnd(), out x)) { return false; }
                if (!Int32.TryParse(xAndyinStringArray[1].TrimStart().TrimEnd(), out y)) { return false; }

                mines.Add(new Point(x, y));
            }

            return true;
        }

        private bool TryProcessExitCoordinate(in List<string> lines, out Point exitCoordinate)
        {
            var line = lines[exitCoordinatesLinePosition];
            var coordinatesAsStringArray = line.Split(" ");

            int x = 0;
            int y = 0;
            if (coordinatesAsStringArray.Length < 2) { exitCoordinate = Point.Empty; return false; }
            if (!Int32.TryParse(coordinatesAsStringArray[0], out x)) { exitCoordinate = Point.Empty; return false; }
            if (!Int32.TryParse(coordinatesAsStringArray[1], out y)) { exitCoordinate = Point.Empty; return false; }

            exitCoordinate = new Point(x, y);
            return true;
        }

        private bool TryProcessStartCoordinate(in List<string> lines, out KeyValuePair<char, Point> startCoordinate)
        {
            startCoordinate = new KeyValuePair<char, Point>();
            var line = lines[startCoordinateLinePosition];
            var coordinatesAsStringArray = line.Split(" ");

            int x = 0;
            int y = 0;
            char initDir = ' ';
            if(coordinatesAsStringArray.Length < 2) { return false; }
            if (!Int32.TryParse(coordinatesAsStringArray[0], out x)) { return false; }
            if (!Int32.TryParse(coordinatesAsStringArray[1], out y)) { return false; }
            if (coordinatesAsStringArray.Length == 3) { initDir = coordinatesAsStringArray[2][0]; }

            startCoordinate = new KeyValuePair<char, Point>(initDir, new Point(x, y));
            return true;
        }

        private bool TryProcessGameSequence(in List<string> lines, out List<char> gameSequence)
        {            
            gameSequence = new List<char>();
            var sb = new StringBuilder();
            for(int i = gameSequenceFromLine + 1; i < lines.Count + 1; i++)
            {
                sb.Append(" ");
                sb.Append(lines[i - 1]);
            }
            var split = sb.ToString().Split(" ");
            foreach(var s in split)
            {
                if(s != null && s != String.Empty)
                {
                    gameSequence.Add(s[0]);
                }
            }

            return true;
        }

        private void ReadFile(ref List<string> lines)
        {
            if (File.Exists(textFile))
            {
                using (StreamReader file = new StreamReader(textFile))
                {
                    string line;

                    while ((line = file.ReadLine()) != null)
                    {
                        lines.Add(line);
                    }
                }
            }
        }

        


    }
}
