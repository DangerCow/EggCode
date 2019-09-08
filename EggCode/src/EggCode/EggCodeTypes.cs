﻿using System.Collections.Generic;
using System.Linq;
using System;

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

        public void Create(string[]lines){int i=start;while(i<end - 1){code.Add(lines[i]);i += 1;}}

        public void Run()
        {
            foreach(string line in code)
            {
                EggCodeMain.parser.ParseLine(line);
            }
        }
    }

    class EggCodeVarible
    {
        public string name;
        public object value;

        public EggCodeVarible(string name, object value)
        {
            this.name = name;
            this.value = value;
        }

        public static EggCodeVarible FindVarible(string name)
        {
            foreach(EggCodeVarible v in EggCodeMain.eggCodeVaribles)
            {
                if (v.name == name)
                {
                    return v;
                }
            }

            return null;
        }

        public static int FindVaribleIndex(string name)
        {
            int i = 0;

            foreach (EggCodeVarible v in EggCodeMain.eggCodeVaribles)
            {
                if (v.name == name)
                {
                    return i;
                }
                i++;
            }

            return -1;
        }
    }
}
