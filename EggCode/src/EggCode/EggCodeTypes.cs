using System.Collections.Generic;

namespace EggCode
{

    class EggCodeVoid
    {
        public string name;
        private List<string> code = new List<string>();

        private int start;
        private int end;

        public void Start(int i){ start = i; }
        public void End(int i) { end = i; }

        //get lines of code inbetween start and end then save them to code

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
