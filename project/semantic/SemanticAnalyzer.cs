/*
   Jaime Margolin A01019332
   Juan carlos Leon A01020200
   Rodrigo Solana A01129839
 */

using System;
using System.Collections.Generic;

namespace Int64 {

class SemanticAnalyzer {

//-----------------------------------------------------------
static readonly IDictionary<TokenCategory, Type> typeMapper =
        new Dictionary<TokenCategory, Type>() {
        // { TokenCategory.BREAK, Type.BREAK },
        // { TokenCategory.ELSE, Type.ELSE },
        // { TokenCategory.RETURN, Type.RETURN },
        // { TokenCategory.CASE, Type.CASE },
        // { TokenCategory.FALSE, Type.FALSE },
        // { TokenCategory.SWITCH, Type.SWITCH },
        // { TokenCategory.CONTINUE, Type.CONTINUE },
        // { TokenCategory.FOR, Type.FOR },
        // { TokenCategory.TRUE, Type.TRUE },
        // { TokenCategory.DEFAULT, Type.DEFAULT },
        // { TokenCategory.IF, Type.IF },
        // { TokenCategory.DO, Type.DO },
        // { TokenCategory.IN, Type.IN },
        // { TokenCategory.VAR, Type.VAR },
        // { TokenCategory.WHILE, Type.WHILE },
        // { TokenCategory.ASSIGN, Type.ASSIGN },
        // { TokenCategory.QUESTION_MARK, Type.QUESTION_MARK },
        // { TokenCategory.EQUAL, Type.EQUAL },
        // { TokenCategory.NOT_EQUAL, Type.NOT_EQUAL },
        // { TokenCategory.GREATER_THAN, Type.GREATER_THAN },
        // { TokenCategory.GREATER_OR_EQUAL_THAN, Type.GREATER_OR_EQUAL_THAN },
        // { TokenCategory.LESS_THAN, Type.LESS_THAN },
        // { TokenCategory.LESS_OR_EQUAL_THAN, Type.LESS_OR_EQUAL_THAN },
        // { TokenCategory.OR, Type.OR },
        // { TokenCategory.BIT_OR, Type.BIT_OR },
        // { TokenCategory.XOR, Type.XOR },
        // { TokenCategory.AND, Type.AND },
        // { TokenCategory.BIT_AND, Type.BIT_AND },
        // { TokenCategory.SHIFT_LEFT, Type.SHIFT_LEFT },
        // { TokenCategory.SHIFT_RIGHT, Type.SHIFT_RIGHT },
        // { TokenCategory.SHIFT_RIGHT_ALT, Type.SHIFT_RIGHT_ALT },
        // { TokenCategory.POWER, Type.POWER },
        // { TokenCategory.MULTIPLICATION, Type.MULTIPLICATION },
        // { TokenCategory.SUBTRACTION, Type.SUBTRACTION },
        // { TokenCategory.ADDITION, Type.ADDITION },
        // { TokenCategory.DIVISION, Type.DIVISION },
        // { TokenCategory.MODULUS, Type.MODULUS },
        // { TokenCategory.NOT, Type.NOT },
        // { TokenCategory.BIT_NOT, Type.BIT_NOT },
        // { TokenCategory.BASE_2, Type.BASE_2 },
        // { TokenCategory.BASE_8, Type.BASE_8 },
        // { TokenCategory.BASE_16, Type.BASE_16 },
        // { TokenCategory.BASE_10, Type.BASE_10 },
        // { TokenCategory.PARENTHESIS_LEFT, Type.PARENTHESIS_LEFT },
        // { TokenCategory.PARENTHESIS_RIGHT, Type.PARENTHESIS_RIGHT },
        // { TokenCategory.CURLY_BRACE_LEFT, Type.CURLY_BRACE_LEFT },
        // { TokenCategory.CURLY_BRACE_RIGHT, Type.CURLY_BRACE_RIGHT },
        // { TokenCategory.COLON, Type.COLON },
        // { TokenCategory.SEMICOLON, Type.SEMICOLON },
        // { TokenCategory.COMMA, Type.COMMA },
        // { TokenCategory.STRING, Type.STRING },
        // { TokenCategory.CHARACTER, Type.CHARACTER },
        // { TokenCategory.WHITE_SPACE, Type.WHITE_SPACE },
        // { TokenCategory.UNKNOWN, Type.UNKNOWN },
        // { TokenCategory.IDENTIFIER, Type.IDENTIFIER },
        // { TokenCategory.EOF, Type.EOF }

};

//-----------------------------------------------------------
public SymbolTable Table {
        get;
        private set;
}

public List<string> globVars;

//-----------------------------------------------------------
public SemanticAnalyzer() {
        Table = new SymbolTable();
        globVars = new List<string>();
}

//-----------------------------------------------------------
public void Visit(NProgram node) {
  Console.WriteLine($"+++++++++++++++ NPROGRAM ++++++++++++++++");
  Console.WriteLine($"n0: ${node[0].GetType()}\t n1: ${node[1].GetType()}");
        Visit((dynamic) node[0]);
        Visit((dynamic) node[1]);
}

//-----------------------------------------------------------
public void Visit(NVarDefList node) {
  Console.WriteLine($"+++++++++++++++ NVARDEFLSIT ++++++++++++++++");
  Console.WriteLine($"n0: ${node.GetType()}");
  foreach(Node i in node)
  {
    Console.WriteLine($"var: {i.AnchorToken.Lexeme }");
  }
        VisitChildren(node);
}

public void Visit(NFunDefList node) {
  Console.WriteLine($"+++++++++++++++ NFUNDEFLIST ++++++++++++++++");
  foreach(Node i in node)
  {
    Console.WriteLine($"func: {i.AnchorToken.Lexeme }");
  }
        VisitChildren(node);
}

//-----------------------------------------------------------
public void Visit(NVarDef node) {

        var variableName = node[0].AnchorToken.Lexeme;

        if (globVars.Contains(variableName)) {
                throw new SemanticError(
                              "Duplicated variable: " + variableName,
                              node[0].AnchorToken);

        } else {
                globVars.Add(variableName);
                //Table[variableName] =
                        // typeMapper[node.AnchorToken.Category];
        }
}

//-----------------------------------------------------------
public void Visit(NFunDef node) {

        var funName = node[0].AnchorToken.Lexeme;


}

//-----------------------------------------------------------
public void Visit(NStmtList node) {
        VisitChildren(node);
}

//-----------------------------------------------------------
public void Visit(NAssign node) {

        var variableName = node.AnchorToken.Lexeme;

        if (Table.Contains(variableName)) {

                // if (expectedType != Visit((dynamic) node[0])) {
                //         throw new SemanticError(
                //                       "Expecting type " + expectedType
                //                       + " in assignment statement",
                //                       node.AnchorToken);
                // }
                Visit((dynamic) node[0]);

        } else {
                throw new SemanticError(
                              "Undeclared variable: " + variableName,
                              node.AnchorToken);
        }
}

//-----------------------------------------------------------
// public void Visit(Print node) {
//         node.ExpressionType = Visit((dynamic) node[0]);
// }

//-----------------------------------------------------------
public void Visit(NIfStmt node) {
        // if (Visit((dynamic) node[0]) != Type.BOOL) {
        //         throw new SemanticError(
        //                       "Expecting type " + Type.BOOL
        //                       + " in conditional statement",
        //                       node.AnchorToken);
        // }
        VisitChildren(node[1]);
}

//-----------------------------------------------------------
public void Visit(NIdentifier node) {

        var variableName = node.AnchorToken.Lexeme;

        if (Table.Contains(variableName)) {
                return;
        }

        throw new SemanticError(
                      "Undeclared variable: " + variableName,
                      node.AnchorToken);
}

//-----------------------------------------------------------
public void Visit(NLitInt node) {

        var intStr = node.AnchorToken.Lexeme;

        try {
                Convert.ToInt32(intStr);

        } catch (OverflowException) {
                throw new SemanticError(
                              "Integer literal too large: " + intStr,
                              node.AnchorToken);
        }
}

//-----------------------------------------------------------
// public void Visit(True node) {
// }
//
// //-----------------------------------------------------------
// public void Visit(False node) {
// }
//
// //-----------------------------------------------------------
// public void Visit(Neg node) {
//         // if (Visit((dynamic) node[0]) != Type.INT) {
//         //         throw new SemanticError(
//         //                       "Operator - requires an operand of type " + Type.INT,
//         //                       node.AnchorToken);
//         // }
//         Visit((dynamic) node[0]);
// }

//-----------------------------------------------------------
public void Visit(NExprAnd node) {
        VisitBinaryOperator('&', node /*, Type.BOOL*/);
}

//-----------------------------------------------------------
public void Visit(NExprComp node) {
        VisitBinaryOperator('<', node /*, Type.INT*/);
}

//-----------------------------------------------------------
public void Visit(NExprAdd node) {
        VisitBinaryOperator('+', node /*, Type.INT*/);
}

//-----------------------------------------------------------
public void Visit(NExprMul node) {
        VisitBinaryOperator('*', node /*, Type.INT*/);
}

//-----------------------------------------------------------
void VisitChildren(Node node) {
  IList<Node> tizdaien;
  tizdaien = node.GetChildren();
        foreach (var n in tizdaien) {
                Visit((dynamic) n);
        }
}

//-----------------------------------------------------------
void VisitBinaryOperator(char op, Node node /*, Type type*/) {
        // if (Visit((dynamic) node[0]) != type ||
        //     Visit((dynamic) node[1]) != type) {
        //         throw new SemanticError(
        //                       String.Format(
        //                               "Operator {0} requires two operands of type {1}",
        //                               op,
        //                               type),
        //                       node.AnchorToken);
        // }

        Visit((dynamic) node[0]) ;
            Visit((dynamic) node[1]);
}
}
}
