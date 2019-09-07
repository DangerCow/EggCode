using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EggCode
{
    class EggCodeCShapTools
    {
        public static bool Eval(string flag)
        {
            string[] args = flag.Split(' ');

            if (args[1] == "==")
            {
                if(EggCodeParser.ParseInput(args[0]) == EggCodeParser.ParseInput(args[2])) { return true; }
            }
            else if (args[1] == ">")
            {
                if (float.Parse(EggCodeParser.ParseInput(args[0])) > float.Parse(EggCodeParser.ParseInput(args[2]))) { return true; }
            }
            else if (args[1] == "<")
            {
                if (float.Parse(EggCodeParser.ParseInput(args[0])) < float.Parse(EggCodeParser.ParseInput(args[2]))) { return true; }
            }
            else if (args[1] == ">=")
            {
                if (float.Parse(EggCodeParser.ParseInput(args[0])) >= float.Parse(EggCodeParser.ParseInput(args[2]))) { return true; }
            }
            else if (args[1] == "<=")
            {
                if (float.Parse(EggCodeParser.ParseInput(args[0])) <= float.Parse(EggCodeParser.ParseInput(args[2]))) { return true; }
            }

            return false;
        }
    }
}
