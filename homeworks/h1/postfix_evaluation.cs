// Simple expression tokenizer.
// Shows how to scan an input string using
// the .NET regular expression API and generators.
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ExpressionTokenizer {

    enum TokenCategory {
        NUMBER, PLUS, TIMES, MINUS, EOF, BAD_TOKEN
    }

    class Token {
        public TokenCategory Category { get; set; }
        public String Lexeme { get; set; }
        public Token(TokenCategory category, String lexeme) {
            Category = category;
            Lexeme = lexeme;
        }
        public override String ToString() {
            return String.Format("Token({0}, {1})", Category, Lexeme);

        }
    }

    class Scanner {
        String input;
        public Scanner(String input) {
            this.input = input;
        }
        public IEnumerable<Token> Start() {
            var regex = new Regex(@"(\d+)|([+])|([*])|([-])|(\s)|(.)");

            foreach (Match m in regex.Matches(input)) {
                if (m.Groups[1].Success) {
                    yield return new Token(TokenCategory.NUMBER, m.Value);
                } else if (m.Groups[2].Success) {
                    yield return new Token(TokenCategory.PLUS, m.Value);
                } else if (m.Groups[3].Success) {
                    yield return new Token(TokenCategory.TIMES, m.Value);
                } else if (m.Groups[4].Success) {
                    yield return new Token(TokenCategory.MINUS, m.Value);
                } else if (m.Groups[5].Success) {
                    continue;
                } else {
                    yield return new Token(TokenCategory.BAD_TOKEN, m.Value);
                }
            }
            yield return new Token(TokenCategory.EOF, null);
        }
    }

    class Driver {
       public static void sharmuta(ref Stack<int> s, String op){
          try {
          int temp1 = s.Pop();
          int temp2 = s.Pop();

          switch(op){
              case "PLUS":
                  s.Push((temp1 + temp2));
                  break;

              case "TIMES":
                  s.Push((temp1 * temp2));
                  break;

              case "MINUS":
                  s.Push((temp1 - temp2));
                  break;
          }
        }catch(Exception e) {
          Console.WriteLine("Too many operators");
          System.Environment.Exit(666);
        }
      }

        public static void Main() {
            Console.Write("> ");
            var input = Console.ReadLine();
            var scanner = new Scanner(input);
            Stack<int> stack = new Stack<int>();

            foreach (Token t in scanner.Start()) {
              if (stack.Count >= 3){
              Console.WriteLine("Hay demasiados números");
              System.Environment.Exit(666);
            }
                 if((t.Category).ToString() == "NUMBER")
                     stack.Push(Int32.Parse(t.Lexeme));
                else if ((t.Category).ToString() == "PLUS" || (t.Category).ToString() == "TIMES" || (t.Category).ToString() == "MINUS")
                {
                  sharmuta(ref stack, (t.Category).ToString());
                  continue;
                }
                else
                {
                  Console.WriteLine("Se ingresaron caracteres no válidos");
                    System.Environment.Exit(666);
                }

            }

            foreach (int i in stack)
            Console.WriteLine("El resultado es = {0} ", i);

        }
    }
}
