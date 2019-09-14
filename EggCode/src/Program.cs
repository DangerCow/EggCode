using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DCS;

namespace demo
{
    class Program
    {
        public static void Main(string[] args)
        {
            EggCode.Run("sampleCode.egg", EggCode.CodeType.File);
            Console.ReadLine();
        }
    }
}
