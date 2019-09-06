using System;
using System.Collections.Generic;

namespace EggCode.Types
{
    class EggCodeStack
    {
        public Dictionary<string, string> stack = new Dictionary<string, string>();

        public void add(string s_name, string s_value)
        {
            stack.Add(s_name, s_value);
        }
        public string get(string s_name)
        {
            return stack[s_name];
        }
    }

    class EggCodeVoid
    {
        public string s_name;
        public int i_startLine;
        public int i_endLine;

        private List<string> code = new List<string>();

        public void Run()
        {
            foreach (string s_line in code)
            {
                EggCodeParser.ParseLine(s_line);
            }
        }

        public void Create(string[] s_lines)
        {
            int i = i_startLine;

            while (i < i_endLine)
            {
                code.Add(s_lines[i]);

                i += 1;
            }
        }
    }
}
