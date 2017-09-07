//==========================================================
// Ta01019332
//==========================================================

using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Exam1 {
public class Problem1 {
public static void solve(string input){
        Regex wc = new Regex(
                @"
                        (?<good> ^[^c|C|\*] .*)
                      ",
                RegexOptions.IgnorePatternWhitespace
                | RegexOptions.Multiline
                );
                System.IO.StreamWriter file = new System.IO.StreamWriter("prog1.f");
        foreach (Match m in wc.Matches(input)) {

                Console.WriteLine($"{m.Captures[0]}\n");
                file.WriteLine($"{m.Captures[0]}");


        }
        file.Close();
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
