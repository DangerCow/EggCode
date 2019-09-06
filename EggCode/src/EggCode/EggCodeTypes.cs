using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggCode
{
    class EggCodeStack
    {
        public Dictionary<string, string> stack = new Dictionary<string, string>();

        public void add(string name, string value)
        {
            stack.Add(name, value);
        }
        public string get(string name)
        {
            return stack[name];
        }
    }

    class EggCodeVoid
    {
        public string name;
        private List<string> code = new List<string>();

        private int start;
        private int end;

        public void Start(int i){ start = i; }
        public void End(int i) { end = i; }

        public void Create(string[]lines){int i=start;while(i<end){code.Add(lines[i]);i += 1;}}

        public void Run()
        {
            foreach(string line in code)
            {
                EggCodeMain.parser.ParseLine(line);
            }
        }
    }
}
