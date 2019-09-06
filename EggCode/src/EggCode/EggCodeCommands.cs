using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EggCode
{
    class EggCodeCommands
    {
        public static void print(string line)
        {
            Console.WriteLine(EggCodeParser.ParseInput(EggCodeMain.Between("(", line, ")")));
        }

        public static void startFunction(string line)
        {
            foreach (EggCodeVoid ecv in EggCodeMain.eggCodeVoids)
            {
                if (ecv.name == line.Split('.')[0]) { ecv.Run(); }
            }
        }

        public static void declairVarible(string line)
        {
            string[] args = line.Split(new[] { " = " }, StringSplitOptions.None);

            args[1] = EggCodeParser.ParseInput(args[1]);

            try { EggCodeMain.stack.add(args[0], args[1]); } catch { EggCodeMain.stack.stack[args[0]] = args[1]; }
        }
    }
}
