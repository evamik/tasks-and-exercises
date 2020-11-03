using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2048
{
    class Program
    {
        static int Score = 0;
        static int Highscore = 0;
        const string HighscoreFile = @"2048";
        const string EncryptionKey = "1";

        static void Main(string[] args)
        {
            Program p = new Program();

            p.Load();
            p.PlayGame();
        }

        public void Load()
        {
            if (File.Exists(HighscoreFile))
            {
                string[] lines = File.ReadAllLines(HighscoreFile);
                lines[2] = EncryptText(lines[2], EncryptionKey);
                lines[0] = EncryptText(lines[0], lines[2]);
                File.AppendAllLines(HighscoreFile, lines);
                if (lines.Length >= 3)
                {
                    StringBuilder encryptedKey = new StringBuilder();
                    for (int i = 2; i < lines.Length; i++)
                        encryptedKey.Append(lines[i]);
                    string key = EncryptText(lines[2], EncryptionKey);

                    char[] keyChars = key.ToCharArray();
                    Array.Reverse(keyChars);
                    string key2 = new string(keyChars);

                    int highscore = 0;
                    int highscore2 = 1;

                    int.TryParse(EncryptText(lines[0], key), out highscore);
                    Console.WriteLine(key.PadLeft(60));
                    int.TryParse(EncryptText(lines[1], key2), out highscore2);

                    //if (highscore == highscore2)
                        Highscore = highscore;
                }
            }
        }

        public void PlayGame()
        {
            bool playing = true;

            while (playing)
            {
                Grid grid = new Grid(4, 4);

                Score = 0;

                bool lost = false;

                PopulateGrid(grid);
                PopulateGrid(grid);

                while (!lost)
                {
                    TakeInput(grid);
                    PopulateGrid(grid);
                    lost = !CanShove(grid);
                }

                SaveScore();
                Draw(grid);

                Console.WriteLine("No more moves left");
                Console.WriteLine("Press ENTER to play again..");

                while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }

                Console.Clear();
            }
        }

        public void SaveScore()
        {
            if (Score > Highscore)
            {
                Highscore = Score;
                File.Delete(HighscoreFile);

                string[] lines = new string[3];

                string key = EncryptText(DateTime.Now.Millisecond.ToString(), EncryptionKey);

                char[] keyChars = key.ToCharArray();
                //Array.Reverse(keyChars);
                string key2 = new string(keyChars);

                lines[0] = EncryptText(Highscore.ToString(), key);
                lines[1] = EncryptText(Highscore.ToString(), key2);
                lines[2] = key;

                using(var fr = File.AppendText(HighscoreFile))
                {
                    fr.WriteLine(lines[0]);
                    Console.WriteLine(lines[0]);
                    fr.WriteLine(lines[1]);
                    fr.WriteLine(lines[2]);
                }
            }
        }

        public string EncryptText(string text, string key)
        {
            StringBuilder keyLong = new StringBuilder();
            while (keyLong.Length < text.Length)
            {
                keyLong.Append(key);
            }
            string highscoreString = text.ToString();
            StringBuilder input = new StringBuilder(highscoreString);
            StringBuilder output = new StringBuilder(highscoreString.Length);
            char c;
            for (int i = 0; i < highscoreString.Length; i++)
            {
                c = highscoreString[i];
                c = (char)(c ^ key.GetHashCode());
                output.Append(c);
            }
            return output.ToString();
        }

        public static void AddScore(int amount)
        {
            Score += amount;
        }

        public bool CanShove(Grid grid)
        {
            if (grid.ShoveDown(true) || grid.ShoveLeft(true) || grid.ShoveRight(true) || grid.ShoveUp(true))
                return true;
            return false;
        }

        public void TakeInput(Grid grid)
        {
            ConsoleKey input = Console.ReadKey(true).Key;
            bool moved = false;
            while (!moved)
            {
                if (input == ConsoleKey.LeftArrow)
                {
                    if (grid.ShoveLeft())
                    {
                        moved = true;
                        grid.ShoveLeft();
                    }
                }
                else if (input == ConsoleKey.RightArrow)
                {
                    if (grid.ShoveRight())
                    {
                        moved = true;
                        grid.ShoveRight();
                    }
                }
                else if (input == ConsoleKey.UpArrow)
                {
                    if (grid.ShoveUp())
                    {
                        moved = true;
                        grid.ShoveUp();
                    }
                }
                else if (input == ConsoleKey.DownArrow)
                {
                    if (grid.ShoveDown())
                    {
                        moved = true;
                        grid.ShoveDown();
                    }
                }
                if (moved)
                    continue;
                input = Console.ReadKey(true).Key;
            }
            Draw(grid);
        }

        public void PopulateGrid(Grid grid)
        {
            Random random = new Random();
            int x = random.Next(grid.Width);
            int y = random.Next(grid.Height);
            int number = (random.Next(2) + 1) * 2;

            int zeros = Math.Min(grid.Zeros(), 1);
            if(zeros>0)
                while (!grid.SumNumbers(y, x, number))
                {
                    x = random.Next(grid.Width);
                    y = random.Next(grid.Height);
                }
            Draw(grid);
        }

        public void Draw(Grid grid)
        {
            string gridToText = grid.ToString();
            Console.SetCursorPosition(0, 0);
            Console.WriteLine(String.Format("Score: {0, -15} HighScore: {1, -15}", Score, Highscore));
            Console.WriteLine(gridToText);
        }
    }

    class GridVisuals
    {
        public string OutLine { get; private set; }
        public string[] Rows;
        private string[] Colors = {"    ", "....", ",,,,", ";;;;", "cccc", "!!!!", "iiii", "????", "░░░░", "####", "▓▓▓▓", "████" };

        public GridVisuals(Grid grid)
        {
            Rows = new string[grid.Height];
            OutLine = "".PadRight(grid.Width*7 + 1, '-') + "\r\n";
        }

        public void UpdateRow(Grid grid, int row)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 1; i <= 3; i++)
            {
                stringBuilder.Append("|");
                foreach (int number in grid.Numbers[row])
                {
                    stringBuilder.Append(" ");
                    if(i!=2)
                        stringBuilder.Append(Colors[Math.Max((int)Math.Log(number, 2), 0)]);
                    else stringBuilder.Append(String.Format("{0, 4}", number));
                    stringBuilder.Append(" |");
                }
                stringBuilder.Append("\r\n");
            }
            stringBuilder.Append(OutLine);

            Rows[row] = stringBuilder.ToString();
        }
    }

    class Grid
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int[][] Numbers { get; private set; }
        public GridVisuals GridVisuals { get; private set; }

        public Grid(int width, int height)
        {
            Width = width;
            Height = height;
            Numbers = new int[height][];

            GridVisuals = new GridVisuals(this);

            for (int i = 0; i < height; i++)
            {
                Numbers[i] = new int[width];
                for (int j = 0; j < width; j++)
                    Numbers[i][j] = 0;
                GridVisuals.UpdateRow(this, i);
            }
        }

        public bool ShoveLeft(bool onlyChecking = false)
        {
            bool shoved = false;
            for (int i = 0; i < Height; i++) // rows
            {
                for (int n = -1; n < Width - 1; n++) // columns
                {
                    if (n < 0) n = 0;
                    for (int j = n; j < Width - 1; j++)
                    {
                        bool summed = SumNumbers(i, j + 1, i, j, !onlyChecking);
                        if (summed)
                            shoved = true;
                    }
                }
            }
            return shoved;
        }

        public bool ShoveRight(bool onlyChecking = false)
        {
            bool shoved = false;
            for (int i = 0; i < Height; i++) // rows
            {
                for (int n = -1; n < Width - 1; n++) // columns
                {
                    if (n < 0) n = 0;
                    for (int j = Width - 1; j > n; j--)
                    {
                        bool summed = SumNumbers(i, j - 1, i, j, !onlyChecking);
                        if (summed)
                            shoved = true;
                    }
                }
            }
            return shoved;
        }

        public bool ShoveUp(bool onlyChecking = false)
        {
            bool shoved = false;
            for (int i = 0; i < Width; i++) // columns
            {
                for (int n = -1; n < Height - 1; n++) // rows
                {
                    if (n < 0) n = 0;
                    for (int j = n; j < Height - 1; j++)
                    {
                        bool summed = SumNumbers(j + 1, i, j, i, !onlyChecking);
                        if (summed)
                            shoved = true;
                    }
                }
            }
            return shoved;
        }

        public bool ShoveDown(bool onlyChecking = false)
        {
            bool shoved = false;
            for (int i = 0; i < Width; i++) // columns
            {
                for (int n = -1; n < Height - 1; n++) // rows
                {
                    if (n < 0) n = 0;
                    for (int j = Height - 1; j > n; j--)
                    {
                        bool summed = SumNumbers(j - 1, i, j, i, !onlyChecking);
                        if (summed)
                            shoved = true;
                    }
                }
            }
            return shoved;
        }

        public void SetNumber(int row, int column, int number)
        {
            Numbers[row][column] = number;
            GridVisuals.UpdateRow(this, row);
        }

        public bool SumNumbers(int row, int column, int row2, int column2, bool sum = true)
        {
            int num1 = Numbers[row][column];
            int num2 = Numbers[row2][column2];
            if (num1 == 0 && num2 == 0)
                return false;
            if (num1 == num2 || num2 == 0)
            {
                if (!sum)
                    return true;
                Numbers[row2][column2] = num1+num2;
                Numbers[row][column] = 0;
                GridVisuals.UpdateRow(this, row);
                GridVisuals.UpdateRow(this, row2);
                if(num1!=0 && num2!=0)
                    Program.AddScore(num1+num2);
                return true;
            }
            return false;
        }

        public bool SumNumbers(int row, int column, int number)
        {
            int num = Numbers[row][column];
            if (num == 0)
            {
                Numbers[row][column] = number;
                GridVisuals.UpdateRow(this, row);
                return true;
            }
            return false;
        }

        public int Zeros()
        {
            int zeros = 0;
            foreach (int[] rows in Numbers)
                foreach (int number in rows)
                    if (number == 0)
                        zeros++;
            return zeros;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(GridVisuals.OutLine);
            foreach (string row in GridVisuals.Rows)
            {
                sb.Append(row);
            }

            return sb.ToString();
        }
    }
}
