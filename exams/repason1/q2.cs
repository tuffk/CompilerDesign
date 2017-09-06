
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Exam1 {
class Question1 {

public static void solve(string file){
        
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
