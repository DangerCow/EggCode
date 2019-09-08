using System;

namespace EggCode
{
    class EggCodeCommands
    {
        public static void print(string line)
        {
            Console.WriteLine(EggCodeParser.ParseInput(EggCodeParser.AdvancedBetween('(', line, ')')));
        }

        public static void startFunction(string line)
        {
            foreach (EggCodeVoid ecv in EggCodeMain.eggCodeVoids)
            {
                if (ecv.name == line.Split('.')[0]) { ecv.Run(); }
            }
        }

        public static object math(string input)
        {
            string[] args = EggCodeParser.AdvancedBetween('(', input, ')').Split(' ');
            float i_return = 0;

            //reparse the arguments so varibles and eaven the math command in the future can be passed

            args[0] = (string)EggCodeParser.ParseInput(args[0]);
            args[2] = (string)EggCodeParser.ParseInput(args[2]);

            if (args[1] == "+")
            { i_return = float.Parse(args[0]) + float.Parse(args[2]); }
            if (args[1] == "-")
            { i_return = float.Parse(args[0]) - float.Parse(args[2]); }
            if (args[1] == "*")
            { i_return = float.Parse(args[0]) * float.Parse(args[2]); }
            if (args[1] == "/")
            { i_return = float.Parse(args[0]) / float.Parse(args[2]); }

            return i_return;
        }

        //called when varible is being declared

        public static void declareVarible(string line)
        {
            string[] args = line.Split(new[] { " = " }, StringSplitOptions.None);

            args[1] = (string)EggCodeParser.ParseInput(args[1]);

            //try to create a varible or overwrite a varible

            EggCodeVarible var = new EggCodeVarible(args[0], args[1]);

            if (EggCodeVarible.FindVaribleIndex(args[0]) == -1)
            {
                EggCodeMain.eggCodeVaribles.Add(var);
            }
            else
            {
                EggCodeMain.eggCodeVaribles[EggCodeVarible.FindVaribleIndex(args[0])] = var;
            }
        }
    }
}
