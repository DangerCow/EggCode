using System.Collections.Generic;
using System;

namespace EggCode
{
    class EggCodeMain
    {
        public static List<EggCodeVoid> eggCodeVoids = new List<EggCodeVoid>();

        public static string[] lines;

        public static EggCodeParser parser = new EggCodeParser();
        public static Dictionary<string, string> stack = new Dictionary<string, string>();

        public static EggCodeVoid start;

        public void Run(string file)
        {
            //read file

            lines = System.IO.File.ReadAllLines(file);

            //remove tabs, spaces and comments from file
            int i = 0;

            foreach (string forLine in lines)
            {
                string line = forLine.Replace("\t", "");

                while (line.IndexOf("  ") >= 0)
                {
                    line = line.Replace("  ", " ");
                }

                if (line.StartsWith("//"))
                {
                    line = "";
                }

                lines[i] = line;

                i += 1;
            }
            //create voids

            i = 0;

            foreach (string line in lines)
            {
                CreateVoids(line, lines, i);
                i += 1;
            }

            //start program

            start.Run();
        }

        private void CreateVoids(string line, string[] lines, int i)
        {
            if (line.StartsWith("func") && !line.EndsWith(".start") && !line.EndsWith(".end"))
            {
                //create new void with name

                EggCodeVoid tempVoid = new EggCodeVoid
                {
                    name = line.Split(' ')[1]
                };

                //start the void so it can find the code inside of the void

                tempVoid.Start(i);

                if (tempVoid.name == "start") { start = tempVoid; }

                eggCodeVoids.Add(tempVoid);
            }
            if (line.EndsWith(".end"))
            {
                //find void ended then end the void then save the code

                foreach(EggCodeVoid ecv in eggCodeVoids)
                {
                    if (line.StartsWith(ecv.name))
                    {
                        ecv.End(i);
                        ecv.Create(lines);
                    }
                }
            }
        }

        public static string Between(string s_in, string s_str, string s_out)
        {
            int pos1 = s_str.IndexOf(s_in) + s_in.Length;
            int pos2 = s_str.Substring(pos1).IndexOf(s_out);
            return s_str.Substring(pos1, pos2);
        }
    }
}
