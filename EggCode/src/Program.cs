using System;
using DCS;

namespace demo
{
    class Program
    {
        public static void Main(string[] args)
        {
            EggCode.Run("EggCodeSimpleSyntax", EggCode.RunAction.ConvertProjectToFile);
            Console.ReadLine();
        }
    }
}
