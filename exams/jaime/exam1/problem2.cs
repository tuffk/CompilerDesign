//==========================================================
// a01019332
//==========================================================

using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Exam1 {
public class Problem2 {
public static void solve(string input){
        Regex wc = new Regex(
                @"
                  (?<hex> x [a-fA-F0-9]+ ;)
                            ",
                RegexOptions.IgnorePatternWhitespace
                | RegexOptions.Multiline
                );
        var splited = input.Split('\n');
        MatchCollection m;
        string zain;
        foreach( string s in splited) {
                m = wc.Matches(s);
                zain = s;
                foreach(var kuz in m) {
                        // Console.WriteLine(kuz);
                        string temp = kuz.ToString();
                        temp = temp.Replace('x', ' ');
                        temp = temp.Replace(';', ' ');
                        temp = temp.Trim();
                        int conv = Convert.ToInt32(temp,16);
                        zain = zain.Replace(kuz.ToString(),conv.ToString()+";");
                }
                Console.WriteLine(zain);
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
