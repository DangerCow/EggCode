using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggCode
{
    class TestProgram
    {
        static void Main(string[] args)
        {
            EggCode ec = new EggCode();
            try { ec.Run(args[0]); } catch
            {
                Console.Write("file: ");
                string file = Console.ReadLine();

                if (file == "s") { ec.Run("samplecode.egg"); }
                else { ec.Run(file); }
            }

            Console.ReadLine();
        }
    }
}
