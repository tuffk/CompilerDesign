
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Exam1 {
class Question1 {

public static void solve(string file){
        Regex cc = new Regex(
                @"
              (?<b> 0[b|B] [0|1] [0|1|_]* [0|1]+ [l|L]? \b)
             |(?<b> 0[b|B] [0|1]+ [l|L]? \b)
          ",
                RegexOptions.IgnorePatternWhitespace
                | RegexOptions.Compiled
                | RegexOptions.Multiline
                );
        foreach (Match m in cc.Matches(file)) {

                Console.WriteLine($"{m.Captures[0]}\n");

        }
}

public static void Main(String[] args) {
        if (args.Length != 1) {
                Console.Error.WriteLine(
                        "Please specify the name of the input file.");
                Environment.Exit(1);
        }

        try {
                var inputPath = args[0];
                var input = File.ReadAllText(inputPath);
                solve(input);
        } catch (FileNotFoundException e) {
                Console.Error.WriteLine(e.Message);
                Environment.Exit(1);
        }
}
}
}
