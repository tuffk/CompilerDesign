using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Trillian {

    //---------------------------------------------------------------
    class SyntaxError: Exception {}

    //---------------------------------------------------------------
    enum Token {
        BANG, COMMA, STAR, SQ_OPEN, SQ_CLOSE, FLOAT,
        ILLEGAL_CHAR, EOF
    }

    //---------------------------------------------------------------
    class Scanner {
        readonly string input;
        public string val;
        static readonly Regex regex = new Regex(
            @"
                (?<Bang>       [!] )
              | (?<Comma>         [,] )
              | (?<Star>        [*] )
              | (?<SqOpen>    \[ )
              | (?<SqClose>   \] )
              | (?<Float>       [-]* \d+ [.]* \d* )
              | (?<WhiteSpace> \s  )
              | (?<Other>      .   )
            ",
            RegexOptions.IgnorePatternWhitespace
                | RegexOptions.Compiled
            );
        static readonly IDictionary<string, Token> regexLabels =
            new Dictionary<string, Token>() {
                {"Bang", Token.BANG},
                {"Comma", Token.COMMA},
                {"Star", Token.STAR},
                {"SqOpen", Token.SQ_OPEN},
                {"SqClose", Token.SQ_CLOSE},
                {"Float", Token.FLOAT}
            };
        public Scanner(string input) {
            this.input = input;
        }
        public IEnumerable<Token> Start() {
            foreach (Match m in regex.Matches(input)) {
                if (m.Groups["WhiteSpace"].Success) {
                    // Skip spaces.
                } else if (m.Groups["Other"].Success) {
                    yield return Token.ILLEGAL_CHAR;
                }else {
                    foreach (var name in regexLabels.Keys) {
                        if (m.Groups[name].Success) {
                          //Console.WriteLine($"NAME: {name}, val: {m}");
                            if(name == "Float")
                            {
                              val = $"{m}";
                            }
                            yield return regexLabels[name];
                            break;
                        }
                    }
                }
            }
            yield return Token.EOF;
        }
    }

    //---------------------------------------------------------------
    class Node: IEnumerable<Node> {
        IList<Node> children = new List<Node>();
        public Node this[int index] {
            get {
                return children[index];
            }
        }
        public string Lexeme;
        public void Add(Node node) {
            children.Add(node);
        }
        public IEnumerator<Node> GetEnumerator() {
            return children.GetEnumerator();
        }
        System.Collections.IEnumerator
                System.Collections.IEnumerable.GetEnumerator() {
            throw new NotImplementedException();
        }
        public override string ToString() {
            return GetType().Name;
        }
        public string ToStringTree() {
            var sb = new StringBuilder();
            TreeTraversal(this, "", sb);
            return sb.ToString();
        }
        static void TreeTraversal(
                Node node,
                string indent,
                StringBuilder sb) {
            sb.Append(indent);
            sb.Append(node);
            sb.Append('\n');
            foreach (var child in node.children) {
                TreeTraversal(child, indent + "  ", sb);
            }
        }
    }

    //---------------------------------------------------------------
    class Program:   Node {}
    class Bang:       Node {}
    class Star:        Node {}
    class Sum:       Node {}
    class Float: Node {

      public override string ToString() {
          return "Float";
      }

    }

    //---------------------------------------------------------------
    class Parser {
        IEnumerator<Token> tokenStream;
        public Parser(IEnumerator<Token> tokenStream) {
            this.tokenStream = tokenStream;
            this.tokenStream.MoveNext();
        }
        public Token CurrentToken {
            get { return tokenStream.Current; }
        }
        public Token Expect(Token category) {
            if (CurrentToken == category) {
                Token current = tokenStream.Current;
                tokenStream.MoveNext();
                return current;
            } else {
                throw new SyntaxError();
            }
        }
        public Node Start() {
            var e = new Program() { Max() };
            Expect(Token.EOF);
            return e;
        }

        public Node Max() {
          Console.WriteLine($"++ MAX ++");
            var exp1 = SimpleExp();
            while (CurrentToken == Token.BANG) {
                Expect(Token.BANG);
                var exp2 = new Bang() { exp1, SimpleExp() };
                exp1 = exp2;
            }
            return exp1;
        }
        public Node MaxList() {
          Console.WriteLine($"++ MaxList ++");
            var exp1 = Max();
            while (CurrentToken == Token.COMMA) {
                Expect(Token.COMMA);
                 //var exp2 = new Sum() { exp1, Max() };
                var exp2 = Max();
                exp1 = exp2;
            }
            return exp1;
        }
        public Node SimpleExp() {
          Console.WriteLine($"++ SimpleExp con {CurrentToken} ++");
            switch (CurrentToken) {
            case Token.SQ_OPEN:
                Console.WriteLine("-- SUM --");
                Expect(Token.SQ_OPEN);
                var exp = new Sum() { MaxList() };
                Expect(Token.SQ_CLOSE);
                return exp;
            case Token.STAR:
                Expect(Token.STAR);
                return new Star() { SimpleExp() };
            case Token.FLOAT:
                //Console.WriteLine("%%%%%%%%%%%%%%%");
                Expect(Token.FLOAT);
                return new Float();
            default:
                throw new SyntaxError();
            }
        }


    }
/*
    //---------------------------------------------------------------
    class CILGenerator {
        public string Visit(Program node) {
            return ".assembly 'Trillian' {}\n\n"
                + ".class public 'FinalExam' extends ['mscorlib']'System'.'Object' {\n"
                + "\t.method public static void 'start'() {\n"
                + "\t\t.entrypoint\n"
                + Visit((dynamic) node[0])
                + "\t\tcall void ['mscorlib']'System'.'Console'::'WriteLine'(int32)\n"
                + "\t\tret\n"
                + "\t}\n"
                + "}\n";
        }
        public string Visit(And node) {
            return Visit((dynamic) node[0])
                + Visit((dynamic) node[1])
                + "\t\tand\n";
        }
        public string Visit(Or node) {
            return Visit((dynamic) node[0])
                + Visit((dynamic) node[1])
                + "\t\tor\n";
        }
        public string Visit(Not node) {
            return Visit((dynamic) node[0])
                + "\t\tldc.i4.1\n"
                + "\t\txor\n";
        }
        public string Visit(Literal_0 node) {
            return "\t\tldc.i4.0\n";
        }
        public string Visit(Literal_1 node) {
            return "\t\tldc.i4.1\n";
        }
    }
*/
    //---------------------------------------------------------------
    class Driver {
        public static void Main(string[] args) {
            try {
                var p = new Parser(
                    new Scanner(args[0]).Start().GetEnumerator());
                var ast = p.Start();
                Console.Write(ast.ToStringTree());
                // File.WriteAllText(
                //     "output.il",
                //     new CILGenerator().Visit((dynamic) ast));
            } catch (SyntaxError) {
                Console.Error.WriteLine("parse error");
                Environment.Exit(1);
            }
        }
    }
}
