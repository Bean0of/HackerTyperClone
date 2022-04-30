using System;
using System.IO;
using System.Linq;

namespace HackerTyperClone
{
    public static class Program
    {
        private const string ConsoleTitle = "Hacker Typer (ESC to exit)";
        private const int LettersPerKey = 3;

        private static readonly string[] RandomCodeExts = new[] { "c", "h", "cpp", "hpp", "cs", "js", "java", "py", "lua", "ts" };
        private static readonly Random RNG = new Random((int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds);

        private static string[] RandomCodeFiles;

        public static void Main(string[] args)
        {
            Init(args);

            string source = GetRandomSource();
            while (TypeSource(source))
            {
                source = GetRandomSource();
                Console.Write("\n\n");
            }

            Uninit();
        }

        private static bool TypeSource(string source)
        {
            bool cont = true;

            for (int i = 0; i < source.Length; i += LettersPerKey)
            {
                if (Console.ReadKey(true).Key == ConsoleKey.Escape)
                {
                    cont = false;
                    break;
                }

                for (int j = 0; j < LettersPerKey; j++)
                {
                    if (i + j >= source.Length) break;
                    Console.Write(source[i + j]);
                }
            }

            return cont;
        }

        private static string GetRandomSource()
        {
            return File.ReadAllText(RandomCodeFiles[RNG.Next(0, RandomCodeFiles.Length - 1)]);
        }

        private static void Init(string[] args)
        {
            string dir = string.Empty;
            if (args.Length == 0)
            {
                Console.Write("Enter a directory to pull source code from (recursive): ");
                dir = Console.ReadLine();
            }
            else dir = args[1];

            CheckRandomCodeDirExists(dir); // If this fails the program will exit early.

            Console.Title = ConsoleTitle;
            Console.Clear();
            Console.TreatControlCAsInput = true;
            Console.ForegroundColor = ConsoleColor.Green;

            RandomCodeFiles = RandomCodeExts.SelectMany(f => Directory.GetFiles(dir, "*." + f, SearchOption.AllDirectories)).ToArray();
        }

        private static void Uninit()
        {
            Console.ResetColor();
        }

        private static void CheckRandomCodeDirExists(string dir)
        {
            if (!Directory.Exists(dir))
            {
                Console.Error.WriteLine($"\"{(Path.IsPathRooted(dir) ? dir : Path.Combine(Environment.CurrentDirectory, dir))}\" doesnt exist.");
                Environment.Exit(1); // 1 = EXIT_FAILURE
            }
        }
    }
}
