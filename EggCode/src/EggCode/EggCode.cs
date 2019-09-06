using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EggCode
{
    class EggCodeMain
    {
        public static List<EggCodeVoid> eggCodeVoids = new List<EggCodeVoid>();
        public static EggCodeParser parser = new EggCodeParser();
        public static EggCodeStack stack = new EggCodeStack();

        public static EggCodeVoid start;

        public void Run(string file)
        {
            string[] lines = System.IO.File.ReadAllLines(file);

            //remove tabs and spaces from file
            int i = 0;

            foreach (string forLine in lines)
            {
                string line = forLine.Replace("\t", "");

                while (line.IndexOf("  ") >= 0)
                {
                    line = line.Replace("  ", " ");
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
            if (line.StartsWith("func"))
            {
                EggCodeVoid tempVoid = new EggCodeVoid
                {
                    name = line.Split(' ')[1]
                };
                tempVoid.Start(i);

                if (tempVoid.name == "start") { start = tempVoid; }

                eggCodeVoids.Add(tempVoid);
            }
            if (line.EndsWith(".end"))
            {
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
