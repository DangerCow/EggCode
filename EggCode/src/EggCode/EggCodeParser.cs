using System;

namespace EggCode
{
    class EggCodeParser
    {
        public void ParseLine(string line)
        {
            //all of the commands go here

            if (line.StartsWith("print")) { EggCodeCommands.print(line); }
            else if (line.EndsWith(".start")) { EggCodeCommands.startFunction(line); }

            //expression to find a varible being declared

            else if (line.Contains(" = ") && !line.Split(new[] { " = " }, StringSplitOptions.None)[0].Contains(" "))
            {
                EggCodeCommands.declareVarible(line);
            }
        }

        public static string ParseInput(string input)
        {
            //get string

            if (input.StartsWith("\"")) { return EggCodeMain.Between("\"", input, "\""); }

            //get math command

            else if (input.StartsWith("math"))
            {
                string[] args = input.Replace("math(", "").Replace(")", "").Split(' ');
                int i_return = 0;

                //reparse the arguments so varibles and eaven the math command in the future can be passed

                args[0] = ParseInput(args[0]);
                args[2] = ParseInput(args[2]);

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

            //if there isent a string or a math command try to find a verible with the name (input)
            //but if not found just return back input like a string

            else { try { return EggCodeMain.stack[input]; } catch { return input; } }
        }
    }
}
