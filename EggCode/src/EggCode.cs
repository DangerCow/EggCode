using System;
using System.Collections.Generic;
using System.IO;
using DCS.Util;

namespace DCS
{
    class EggCode
    {
        public enum CodeType { Project, File };
        private static List<EggCodeTypeVoid> eggCodeVoids = new List<EggCodeTypeVoid>();
        private static List<EggCodeTypeVarible> eggCodeGlobalVaribles = new List<EggCodeTypeVarible>();
        private static EggCodeTypeVoid startVoid = null;

        private static EggCodeTypeVoid CurrentlyRunning = null;

        public static void Run(string file, CodeType codeType)
        {
            string filePath = AppDomain.CurrentDomain.BaseDirectory + file;

            if(codeType == CodeType.Project) { RunProject(file, filePath); }
            else if (codeType == CodeType.File) { RunFile(file, filePath); }
        }

        private static void RunFile(string file, string fullPath)
        {
            string[] lines = File.ReadAllLines(file);

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

            i = 0;

            //find voids

            foreach (string line in lines)
            {
                FindVoids(line, i, lines, true);
                i++;
            }

            //run

            if (startVoid == null) { throw new Exception("The defualt file dose not contain a start functtion"); }

            startVoid.Run();
        }
        
        private static void RunProject(string file, string fullPath)
        {
            DirectoryInfo d = new DirectoryInfo(fullPath);
            List<string> files = new List<string>();

            string defualtFile = null;

            //get project settings
            foreach (var f in d.GetFiles("*"))
            {
                if (f.Name == ".eggpro")
                {
                    string[] lines = File.ReadAllLines(f.FullName);
                    foreach(string line in lines)
                    {
                        if (line.StartsWith("addFile"))
                        {
                            files.Add(line.AdvancedBetween('(', ')'));
                        }
                        else if (line.StartsWith("defualt = "))
                        {
                            defualtFile = line.StringSplit(" = ")[1];
                        }
                    }
                }
            }

            if (defualtFile == null) { throw new Exception("Defualt file not found"); }

            //find voids in everyfile
            foreach(string f in files)
            {
                string[] lines = File.ReadAllLines(fullPath + "\\" + f);

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

                i = 0;

                foreach(string line in lines)
                {
                    FindVoids(line, i, lines, f == defualtFile);
                    i++;
                }
            }

            if (startVoid == null) { throw new Exception("The defualt file dose not contain a start functtion"); }

            startVoid.Run();
        }

        private static void FindVoids(string line, int i, string[] lines, bool DefaultFile)
        {
            if (line.StartsWith("func") && !line.EndsWith(".start") && !line.EndsWith(".end"))
            {
                EggCodeTypeVoid TempVoid = new EggCodeTypeVoid
                {
                    name = line.StringSplit("func ")[1]
                };
                TempVoid.Start(i);

                if (DefaultFile && TempVoid.name == "start")
                {
                    startVoid = TempVoid;
                }

                eggCodeVoids.Add(TempVoid);
            }
            else if (line.StartsWith("void") && line.EndsWith(")"))
            {
                EggCodeTypeVoid TempVoid = new EggCodeTypeVoid
                {
                    name = line.StringSplit("void ")[1].Split('(')[0]
                };
                TempVoid.StartWithPrams(i, line.AdvancedBetween('(', ')'));

                eggCodeVoids.Add(TempVoid);
            }
            else if (line.EndsWith(".end"))
            {
                foreach(EggCodeTypeVoid ecv in eggCodeVoids)
                {
                    if (line.StringSplit(".end")[0] == ecv.name)
                    {
                        ecv.End(i);

                        ecv.Create(lines);
                    }
                }
            }
        }

        private class EggCodeParser
        {
            public static int skip = 0;

            public static void ParseLine(string line)
            {
                if (line.StartsWith("print")) { EggCodeCommands.Print(line); }
                else if (line.EndsWith(".start")) { EggCodeCommands.StartFunc(line); }
                else if (line.Contains(".start") && line.EndsWith(")")) { EggCodeCommands.StartFuncWithPrams(line); }
                else if (line.StartsWith("skip")) { EggCodeCommands.Skip(line); }
                else if (line.StartsWith("input")) { EggCodeCommands.Input(line); }

                else if (line.StartsWith("if"))
                {
                    bool flag = Eval(line.StringSplit(" else ")[0].AdvancedBetween('(', ')'));
                    string elseCommand = line.StringSplit(" else ")[1].AdvancedBetween('(', ')');

                    if (!flag)
                    {
                        ParseLine(elseCommand);
                    }
                }

                //expression to find a varible being declared

                else if (line.Contains(" = ") && !line.Split(new[] { " = " }, StringSplitOptions.None)[0].Contains(" ") && !line.StartsWith("(private)"))
                {
                    EggCodeCommands.DeclareVarible(line);
                }

                else if (line.Contains(" = ") && !line.Split(new[] { " = " }, StringSplitOptions.None)[0].Contains(" ") && line.StartsWith("(private)"))
                {
                    EggCodeCommands.DeclarePrivateVarible(line);
                }
            }

            public static object ParseInput(string input)
            {
                if (input.StartsWith("\"")) { return input.Between("\"", "\""); }

                else if (input.StartsWith("input"))
                {
                    Console.Write(ParseInput(input.AdvancedBetween('(', ')')));
                    return Console.ReadLine();
                }

                else if (input.StartsWith("math"))
                {
                    return EggCodeCommands.Math(input);
                }

                //if no other input method is found try to return a private varible but if there is no private varible try to return a global one
                else
                {
                    try
                    {
                        try
                        {
                            return CurrentlyRunning.FindVarible(input).value;
                        }
                        catch
                        {
                            return EggCodeTypeVarible.FindVarible(input).value;
                        }
                    }
                    catch
                    {
                        return input;
                    }
                }
            }

            public static bool Eval(string flag)
            {
                try
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

                catch
                {
                    throw new Exception("If statment not correct format");
                }
            }
        }

        private class EggCodeCommands
        {
            public static void Print(string line)
            {
                Console.WriteLine(EggCodeParser.ParseInput(line.AdvancedBetween('(', ')')));
            }
            public static void StartFunc(string line)
            {
                foreach(EggCodeTypeVoid ecv in eggCodeVoids)
                {
                    if (ecv.name == line.StringSplit(".start")[0])
                    {
                        ecv.Run();
                    }
                }
            }
            public static void StartFuncWithPrams(string line)
            {
                foreach (EggCodeTypeVoid ecv in eggCodeVoids)
                {
                    if (ecv.name == line.StringSplit(".start")[0])
                    {
                        ecv.RunWithPrams(line.AdvancedBetween('(', ')'));
                    }
                }
            }
            public static void Skip(string line)
            {
                int skipAmount = int.Parse(line.StringSplit("skip ")[1]);
                EggCodeParser.skip = skipAmount;
            }

            public static void DeclareVarible(string line)
            {
                string[] args = line.StringSplit(" = ");
                args[1] = EggCodeParser.ParseInput(args[1]).ToString();

                EggCodeTypeVarible var = new EggCodeTypeVarible(args[0], args[1]);

                if (EggCodeTypeVarible.FindVaribleIndex(args[0]) == -1)
                {
                    eggCodeGlobalVaribles.Add(var);
                }
                else
                {
                    eggCodeGlobalVaribles[EggCodeTypeVarible.FindVaribleIndex(args[0])] = var;
                }
            }
            public static void DeclarePrivateVarible(string line)
            {
                string[] args = line.Replace("(private)", "").StringSplit(" = ");
                args[1] = EggCodeParser.ParseInput(args[1]).ToString();

                EggCodeTypeVarible var = new EggCodeTypeVarible(args[0], args[1]);

                if (CurrentlyRunning.FindVarible(args[0]) == null)
                {
                    CurrentlyRunning.privateVaribles.Add(var);
                }
                else
                {
                    CurrentlyRunning.privateVaribles[CurrentlyRunning.FindVaribleIndex(args[0])] = var;
                }
            }

            public static float Math(string input)
            {

                    string[] args = input.AdvancedBetween('(', ')').Split(' ');
                    float i_return = 0;

                    //reparse the arguments so varibles and eaven the math command in the future can be passed

                    args[0] = (string)EggCodeParser.ParseInput(args[0]);
                     args[2] = (string)EggCodeParser.ParseInput(args[2]);

                    if (args[1] == "+")
                    { i_return = float.Parse(args[0]) + float.Parse(args[2]); }
                    if (args[1] == "-")
                    { i_return = float.Parse(args[0]) - float.Parse(args[2]); }
                    if (args[1] == "*")
                    { i_return = float.Parse(args[0]) * float.Parse(args[2]); }
                    if (args[1] == "/")
                    { i_return = float.Parse(args[0]) / float.Parse(args[2]); }

                    return i_return;
            }

            internal static void Input(string line)
            {
                Console.Write(line.AdvancedBetween('(', ')'));
                Console.ReadLine();
            }
        }

        private class EggCodeTypeVarible
        {
            public string name;
            public object value;

            public EggCodeTypeVarible(string name, object value)
            {
                this.name = name;
                this.value = value;
            }

            public static int FindVaribleIndex(string name)
            {
                int i = 0;

                foreach (EggCodeTypeVarible v in eggCodeGlobalVaribles)
                {
                    if (v.name == name)
                    {
                        return i;
                    }
                    i++;
                }

                return -1;
            }

            public static EggCodeTypeVarible FindVarible(string name)
            {
                foreach (EggCodeTypeVarible v in eggCodeGlobalVaribles)
                {
                    if (v.name == name)
                    {
                        return v;
                    }
                }

                return null;
            }
        }

        private class EggCodeTypeVoid
        {
            public string name;
            private List<string> code = new List<string>();

            public List<EggCodeTypeVarible> privateVaribles = new List<EggCodeTypeVarible>();

            private int start;
            private int end;
            private string prams_string;

            public void Start(int i) { start = i; }
            public void StartWithPrams(int i, string pram) { start = i; prams_string = pram; }
            public void End(int i) { end = i; }

            //get lines of code inbetween start and end then save them to code

            public void Create(string[] lines) { int i = start; while (i < end) { code.Add(lines[i]); i += 1; } }

            public EggCodeTypeVarible FindVarible(string name)
            {
                foreach (EggCodeTypeVarible v in privateVaribles)
                {
                    if (v.name == name)
                    {
                        return v;
                    }
                }

                return null;
            }
            public int FindVaribleIndex(string name)
            {
                int i = 0;

                foreach (EggCodeTypeVarible v in privateVaribles)
                {
                    if (v.name == name)
                    {
                        return i;
                    }

                    i++;
                }

                return -1;
            }

            public void Run()
            {
                CurrentlyRunning = this;

                foreach (string line in code)
                {
                    if (EggCodeParser.skip == 0) { EggCodeParser.ParseLine(line); }
                    else if (EggCodeParser.skip != 0) { EggCodeParser.skip -= 1; }
                }
            }
            public void RunWithPrams(string pramCalls)
            {
                CurrentlyRunning = this;
                int i = 0;

                foreach(string pram in prams_string.StringSplit(", "))
                {
                    string s_pram = (string)EggCodeParser.ParseInput(pram);
                    object s_pramValue = EggCodeParser.ParseInput(pramCalls.StringSplit(", ")[i]);

                    EggCodeTypeVarible temp = new EggCodeTypeVarible(s_pram, s_pramValue);
                    privateVaribles.Add(temp);

                    i++;
                }

                foreach (string line in code)
                {
                    if (EggCodeParser.skip == 0) { EggCodeParser.ParseLine(line); }
                    else if (EggCodeParser.skip != 0) { EggCodeParser.skip -= 1; }
                }
            }
        }
    }
}

namespace DCS.Util
{
    public static class Util
    {
        //beter split command
        public static string[] StringSplit(this string str, string spliter)
        {
            return str.Split(new string[] { spliter }, StringSplitOptions.None);
        }

        //get string inbetween 2 chactors
        public static string Between(this string str, string s_in, string s_out)
        {
            int pos1 = str.IndexOf(s_in) + s_in.Length;
            int pos2 = str.Substring(pos1).IndexOf(s_out);
            return str.Substring(pos1, pos2);
        }

        //get string invetween 2 chactors that can also contain the 2 chactors
        public static string AdvancedBetween(this string str, char s_in, char s_out)
        {
            string final = "";
            int skip = 0;
            bool output = false;

            foreach (char chactor in str)
            {
                if (chactor == s_in && output == false) { output = true; continue; } //start
                else if (chactor == s_in && output == true) { skip += 1; }

                if (chactor == s_out && skip == 0) { output = false; } //stop
                else if (chactor == s_out && skip > 0) { skip -= 1; }

                if (output) { final += chactor; }
            }

            return final;
        }
    }
}
