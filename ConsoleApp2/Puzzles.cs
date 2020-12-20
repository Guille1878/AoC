using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConsoleApp2
{
    static class Puzzles
    {
        public static List<string> IntroList => ReadList("intro");

        public static List<string> PuzzleList => ReadList("puzzle");

        private static List<string> ReadList(string file)
        {
            var result = new List<string>();
            using var sr = new StreamReader($"Data/{file}.txt");

            while (!sr.EndOfStream)
                result.Add(sr.ReadLine());

            return result;
        }

        
        



    }
}
