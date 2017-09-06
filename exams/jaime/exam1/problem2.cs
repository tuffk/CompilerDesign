//==========================================================
// a01019332
//==========================================================

using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Exam1 {
    public class Problem2 {
        public static void Main(String[] args) {
          if (args.Length != 1) {
                  Console.Error.WriteLine(
                          "Please specify the name of the input file.");
                  Environment.Exit(1);
          }

          try {
                  var inputPath = args[0];
                  var input = File.ReadAllText(inputPath);
                  //TODO: solution goes here
          } catch (FileNotFoundException e) {
                  Console.Error.WriteLine(e.Message);
                  Environment.Exit(1);
          }
        }
    }
}
