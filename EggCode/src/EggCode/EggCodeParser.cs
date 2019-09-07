using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
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
                if(EggCodeCShapTools.Eval(AdvancedBetween('(', line, ')'))) { parsing_if_true = true; } else { parsing_if_true = false; }
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

        public static string ParseInput(string input)
        {
            //get string

            if (input.StartsWith("\"")) { return EggCodeMain.Between("\"", input, "\""); }

            //get math command

            else if (input.StartsWith("math"))
            {
                string[] args = AdvancedBetween('(', input, ')').Split(' ');
                float i_return = 0;

                //reparse the arguments so varibles and eaven the math command in the future can be passed

                args[0] = ParseInput(args[0]);
                args[2] = ParseInput(args[2]);

                if (args[1] == "+")
                { i_return = float.Parse(args[0]) + float.Parse(args[2]); }
                if (args[1] == "-")
                { i_return = float.Parse(args[0]) - float.Parse(args[2]); }
                if (args[1] == "*")
                { i_return = float.Parse(args[0]) * float.Parse(args[2]); }
                if (args[1] == "/")
                { i_return = float.Parse(args[0]) / float.Parse(args[2]); }

                return i_return.ToString();
            }

            //if there isent a string or a math command try to find a verible with the name (input)
            //but if not found just return back input like a string

            else { try { return EggCodeMain.stack[input]; } catch { return input; } }
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
