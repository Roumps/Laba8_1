using System;
using System.Text;
using System.Threading;
using System.IO;

namespace Program
{
    class Program
    {
        static string[] symbols = new string[] { ":", " - ", " [", "] ", ", " };
        static int width, height, lastSecond;

        public static void Main()
        {
            string[] lines = File.ReadAllLines("Laba_8.txt");
            string[][] parameters = new string[lines.Length][];

            for (int i = 0; i < parameters.Length; i++)
            {
                parameters[i] = GetParameters(lines[i]);
            }

            DeterminatesLastSecond(parameters);
            DrawWindow();

            for (int seconds = 0; seconds <= lastSecond; seconds++)
            {
                CheckUpdateText(parameters, seconds);
                Thread.Sleep(1000);
            }
        }

        public static void DeterminatesLastSecond(string[][] parameters)
        {
            for (int i = 0; i < parameters.Length; i++)
            {
                if (int.Parse(parameters[i][3]) > lastSecond)
                    lastSecond = int.Parse(parameters[i][3]);
            }
        }

        public static string[] GetParameters(string line)
        {
            foreach (var symbol in symbols)
            {
                line = line.Replace(symbol, "--");
            }

            if (line.Split("--").Length <= 4)
            {
                int index = line.IndexOf(' ');
                line = line.Remove(index, 1).Insert(index, "--");
            }

            return line.Split("--");
        }

        public static void SetColor(string color)
        {
            switch (color)
            {
                case "Red":
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case "Green":
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case "Blue":
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }
        }

        public static void SetCursor(string side, string text)
        {
            int x = 0;
            int y = 0;

            switch (side)
            {
                case "Top":
                    x = (width / 2) - (text.Length / 2);
                    y = 1;
                    break;
                case "Right":
                    x = width - text.Length - 1;
                    y = height / 2;
                    break;
                case "Left":
                    x = 1;
                    y = height / 2;
                    break;
                case "Bottom":
                    x = (width / 2) - (text.Length / 2);
                    y = height - 2;
                    break;
            }

            Console.SetCursorPosition(x, y);
        }

        public static void CheckUpdateText(string[][] parameters, int seconds)
        {
            int secAppend, secDelete;
            string side, text, color;

            foreach (var line in parameters)
            {
                secAppend = Int32.Parse(line[1]);
                secDelete = Int32.Parse(line[3]);

                if (line.Length <= 5)
                {
                    side = "Bottom";
                    color = "White";
                    text = line[4];
                }
                else
                {
                    side = line[4];
                    color = line[5];
                    text = line[6];
                }

                if (secAppend == seconds)
                    EditWindowText(side, color, text, "Append");
                if (secDelete == seconds)
                    EditWindowText(side, color, text, "Delete");
            }
        }

        public static void EditWindowText(string side, string color, string text, string param)
        {
            SetCursor(side, text);
            SetColor(color);

            if (param == "Append") Console.Write(text);
            else if (param == "Delete") Console.Write(CreateEmptyString(text.Length));

            Console.ResetColor();
            Console.SetCursorPosition(0, height);
        }

        public static string CreateEmptyString(int length)
        {
            StringBuilder result = new();

            for (int i = 0; i < length; i++)
                result.Append(' ');

            return result.ToString();
        }

        public static void DrawWindow()
        {
            string[] window = File.ReadAllLines("sp.txt");

            foreach (string line in window)
            {
                Console.WriteLine(line);
            }

            width = window[0].Length;
            height = window.Length;
        }
    }
}