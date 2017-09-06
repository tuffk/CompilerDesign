
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Exam1 {
class Question1 {

public static void solve(string file){
        Regex wc = new Regex(
                @"
                  (?<word> [a-zA-Z]+ [a-zA-Z0-9_]*)
                  |(?<nl> \n)
                ",
                RegexOptions.IgnorePatternWhitespace
                | RegexOptions.Compiled
                | RegexOptions.Multiline
                );
        Regex cc = new Regex(
                @"
                  (?<ch> .)
                ",
                RegexOptions.IgnorePatternWhitespace
                | RegexOptions.Compiled
                | RegexOptions.Singleline
                );
        var words = 0;
        var chars = 0;
        var news = 0;
        foreach (Match m in wc.Matches(file)) {
          if(m.Groups["word"].Success)
          {
            words++;
          }else if(m.Groups["nl"].Success){
            news++;
          }
        }
        foreach (Match m in cc.Matches(file)) {
          if(m.Groups["ch"].Success)
          {
            chars++;
          }
        }

      Console.WriteLine($"lines: {news}\nwords: {words}\nchars: {chars}");
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
