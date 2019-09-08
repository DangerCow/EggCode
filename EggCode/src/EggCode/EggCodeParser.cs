using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Reflection;

namespace EggCode
{
    class EggCodeParser
    {
        public bool parsing_if = false;
        public bool parsing_if_true = false;

        public void ParseLine(string line)
        {
            //all of the commands go here

            if (!parsing_if || (parsing_if && parsing_if_true))
            {
                if (line.StartsWith("print")) { EggCodeCommands.print(line); }
                else if (line.EndsWith(".start")) { EggCodeCommands.startFunction(line); }
            }

            if (line.StartsWith("if ("))
            {
                parsing_if = true;
                if(Eval(AdvancedBetween('(', line, ')'))) { parsing_if_true = true; } else { parsing_if_true = false; }
            }
            else if (line.StartsWith("endif"))
            {
                parsing_if = false;
            }

            //expression to find a varible being declared

            else if (line.Contains(" = ") && !line.Split(new[] { " = " }, StringSplitOptions.None)[0].Contains(" "))
            {
                EggCodeCommands.declareVarible(line);
            }
        }

        public static bool Eval(string flag)
        {
            string[] args = flag.Split(' ');

            if (args[1] == "==")
            {
                if ((string)ParseInput(args[0]) == (string)ParseInput(args[2])) { return true; }
            }
            else if (args[1] == ">")
            {
                if ((float)ParseInput(args[0]) > (float)ParseInput(args[2])) { return true; }
            }
            else if (args[1] == "<")
            {
                if ((float)ParseInput(args[0]) < (float)ParseInput(args[2])) { return true; }
            }
            else if (args[1] == ">=")
            {
                if ((float)ParseInput(args[0]) >= (float)ParseInput(args[2])) { return true; }
            }
            else if (args[1] == "<=")
            {
                if ((float)ParseInput(args[0]) <= (float)ParseInput(args[2])) { return true; }
            }

            return false;
        }

        public static object ParseInput(string input)
        {
            //get string

            if (input.StartsWith("\"")) { return EggCodeMain.Between("\"", input, "\""); }

            //get input commands

            if (input.StartsWith("math"))
            {
                return EggCodeCommands.math(input);
            }

            //if there isent a string or a math command try to find a verible with the name (input)
            //but if not found just return back input like a string

            else { try { return EggCodeVarible.FindVarible(input).value; } catch { return input; } }
        }

        public static string AdvancedBetween(char s_in, string s_str, char s_out)
        {
            string final = "";
            int skip = 0;
            bool output = false;

            foreach (char chactor in s_str)
            {
                if (chactor == s_in && output == false){ output = true; continue; } //start
                else if (chactor == s_in && output == true) { skip += 1; }
                
                if (chactor == s_out && skip == 0) { output = false;  } //stop
                else if (chactor == s_out && skip > 0) { skip -= 1; }

                if (output) { final += chactor; }
            }

            return final;
        }
    }
}
