using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DCS;

namespace Testing
{
    class TestingProgram
    {
        static void Main(string[] args)
        {
            EggCode.Run("TestProject", EggCode.CodeType.Project);
            Console.ReadLine();
        }
    }
}
