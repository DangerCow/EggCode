using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EggCode
{
    class EggCodeParser
    {
        public void ParseLine(string line)
        {
            if (line.StartsWith("print")) { EggCodeCommands.print(line); }
            else if (line.EndsWith(".start")) { EggCodeCommands.startFunction(line); }

            else if (line.Contains(" = ") && !line.Split(new[] { " = " }, StringSplitOptions.None)[0].Contains(" "))
            {
                EggCodeCommands.declairVarible(line);
            }
        }

        public static string ParseInput(string input)
        {
            if (input.StartsWith("\"")) { return EggCodeMain.Between("\"", input, "\""); }

            else if (input.StartsWith("math"))
            {
                string[] args = input.Replace("math(", "").Replace(")", "").Split(' ');
                int i_return = 0;

                if (args[1] == "+")
                { i_return = int.Parse(args[0]) + int.Parse(args[2]); }
                if (args[1] == "-")
                { i_return = int.Parse(args[0]) - int.Parse(args[2]); }
                if (args[1] == "*")
                { i_return = int.Parse(args[0]) * int.Parse(args[2]); }
                if (args[1] == "/")
                { i_return = int.Parse(args[0]) / int.Parse(args[2]); }

                return i_return.ToString();
            }

            else { try { return EggCodeMain.stack.get(input); } catch { return input; } }
        }
    }
}
