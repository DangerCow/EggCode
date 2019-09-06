using System;
using System.Collections.Generic;
using EggCode.Types;

namespace EggCode
{
    class EggCode
    {
        public static List<EggCodeVoid> eggCodeVoids = new List<EggCodeVoid>();
        public static EggCodeStack stack = new EggCodeStack();
        EggCodeVoid startVoid;

        public void Run(string s_file)
        {
            //read file
            string[] s_lines = System.IO.File.ReadAllLines(s_file);

            //remove tabs and spaces from file
            int i = 0;

            foreach (string s_forLine in s_lines)
            {
                string s_line = s_forLine.Replace("\t", "");

                while (s_line.IndexOf("  ") >= 0)
                {
                    s_line = s_line.Replace("  ", " ");
                }

                s_lines[i] = s_line;

                i += 1;
            }

            i = 0;

            //find voids
            while (i < s_lines.Length)
            {
                string s_line = s_lines[i];

                UpdateVoids(s_line, i, s_lines);

                i += 1;
            }

            //finaly run code!

            startVoid.Run();
        }

        private void UpdateVoids(string s_line, int i, string[] s_lines)
        {
            if (s_line.StartsWith("new void"))
            {
                EggCodeVoid tempVoid = new EggCodeVoid
                {
                    s_name = Between("[", s_line, "]"),
                    i_startLine = i + 1
                };

                if (tempVoid.s_name == "start") { startVoid = tempVoid; }

                eggCodeVoids.Add(tempVoid);
            }
            
            foreach(EggCodeVoid ecv in eggCodeVoids)
            {
                if (s_line.StartsWith(ecv.s_name) && s_line.EndsWith(".end"))
                {
                    ecv.i_endLine = i;
                    ecv.Create(s_lines);
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

    class EggCodeParser
    {
        public static void ParseLine(string s_line)
        {
            if (s_line.StartsWith("openVoid"))
            {
                string s_void = EggCode.Between("[", s_line, "]");

                foreach(EggCodeVoid ecv in EggCode.eggCodeVoids)
                {
                    if (ecv.s_name == s_void)
                    {
                        ecv.Run();
                        break;
                    }
                }
            }
            else if (s_line.StartsWith("print"))
            {
                Console.WriteLine(ParseString(EggCode.Between("[", s_line, "]"), true));
            }
            else if (s_line.StartsWith("new var"))
            {
                EggCode.stack.add(EggCode.Between("[", s_line, "]"), ParseString(s_line.Split('=')[1].Replace(" ", ""), true));
            }
        }

        static string ParseString(string s_string, bool s_stringOps)
        {
            if (s_string.StartsWith("\""))
            {
                return EggCode.Between("\"", s_string, "\"");
            }
            else
            {
                try { return EggCode.stack.get(s_string); } catch {
                    if (s_string.StartsWith("math") && s_stringOps)
                    {
                        string[] args = EggCode.Between("(", s_string, ")").Split(',');

                        if (args[0] == "+")
                        {
                            return (float.Parse(ParseString(args[1], false)) + float.Parse(ParseString(args[1], false))).ToString();
                        }
                        return s_string;
                    }
                    return s_string;
                }
            }
        }
    }
}
