using System;

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

        //called when varible is being declared

        public static void declareVarible(string line)
        {
            string[] args = line.Split(new[] { " = " }, StringSplitOptions.None);

            args[1] = EggCodeParser.ParseInput(args[1]);

            //try to create a varible or overwrite a varible

            try { EggCodeMain.stack.Add(args[0], args[1]); } catch { EggCodeMain.stack[args[0]] = args[1]; }
        }
    }
}
