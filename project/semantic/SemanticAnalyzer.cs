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

public List<string> globVars; // Global variabe "table"
public static int pasones;

//-----------------------------------------------------------
public SemanticAnalyzer() {
        pasones = 0;
        Table = new SymbolTable();
        Modishness mo = new Modishness("printi", 1, true);
        Table["printi"] = mo;
        mo = new Modishness("printc", 1, true);
        Table["printc"] = mo;
        mo = new Modishness("prints", 1, true);
        Table["prints"] = mo;
        mo = new Modishness("println", 0, true);
        Table["println"] = mo;
        mo = new Modishness("readi", 0, true);
        Table["readi"] = mo;
        mo = new Modishness("reads", 0, true);
        Table["reads"] = mo;
        mo = new Modishness("new", 1, true);
        Table["new"] = mo;
        mo = new Modishness("size", 1, true);
        Table["size"] = mo;
        mo = new Modishness("add", 2, true);
        Table["add"] = mo;
        mo = new Modishness("get", 2, true);
        Table["get"] = mo;
        mo = new Modishness("set", 3, true);
        Table["set"] = mo;
        globVars = new List<string>();
}

//-----------------------------------------------------------
public void Visit(NProgram node) {
        var cuentame = 0; var cuentaParam=0;
        Console.WriteLine($"+++++++++++++++ NPROGRAM ++++++++++++++++");
        Console.WriteLine($"n0: ${node[0].GetType()}\t n1: ${node[1].GetType()}");
        /*Primer Vuelta recursiva del árbol*/
        if(pasones == 0)
        {
        Visit((dynamic) node[0]);
        Visit((dynamic) node[1]);
        }
        /*Segunda Vuelta del árbol generando las tablas del body*/
        else if(pasones == 1){
          /*If SemanticError de main*/
          if (Table.Contains("main") == false)
            throw new SemanticError("No main function was found ");
          else if(Table["main"].args > 0)
            throw new SemanticError("main function should hace 0 parameters");

          foreach(Node j in node[1].children){
            Table[j.AnchorToken.Lexeme].locTable = new SortedDictionary<string , Sharmuta>();

                  foreach(Node i in j.children)
                  {

                  if(i.GetType().ToString() == "Int64.NParameterList"){
                    foreach(Node k in i.children){
                        Sharmuta sha = new Sharmuta(k.AnchorToken.Lexeme, true ,cuentame);
                        Table[j.AnchorToken.Lexeme].locTable[k.AnchorToken.Lexeme] = sha;
                        cuentame++;
                      }

                    }


                  if(i.GetType().ToString() == "Int64.NVarDefList")
                    foreach(Node k in i.children){
                      if(Table[j.AnchorToken.Lexeme].locTable.ContainsKey(k.AnchorToken.Lexeme))
                      throw new SemanticError($"parameter and local variable names have to be unique on function declaration: [{j.AnchorToken.Lexeme}]");
                      Sharmuta sha = new Sharmuta(k.AnchorToken.Lexeme, false , null);
                      Table[j.AnchorToken.Lexeme].locTable[k.AnchorToken.Lexeme] = sha;
                      }

                  }

                  cuentame=0;

          }
          //Console.WriteLine($"nodo: ${node[1].chilren}");
      }
        else if(pasones == 2){
              Visit((dynamic) node[1]);
        }
}

//-----------------------------------------------------------
public void Visit(NVarDefList node) {
        Console.WriteLine($"+++++++++++++++ NVARDEFLSIT ++++++++++++++++");
        Console.WriteLine($"n0: ${node.GetType()}");
        foreach(Node i in node)
        {
                Console.WriteLine($"Global Variable: {i.AnchorToken.Lexeme }");
        }
        VisitChildren(node);
}

public void Visit(NFunDefList node) {
        Console.WriteLine($"+++++++++++++++ NFUNDEFLIST ++++++++++++++++");

        VisitChildren(node);
}

public void Visit(NParameterList node) {
        Console.WriteLine($"+++++++++++++++ NPARAMETERLIST ++++++++++++++++");

        VisitChildren(node);
}

public void Visit(NParameter node) {
        Console.WriteLine($"+++++++++++++++ NParameter ++++++++++++++++");

        VisitChildren(node);
}

//-----------------------------------------------------------
public void Visit(NVarDef node) {
        Console.WriteLine($"+++++++++++++++ NVARDEF ++++++++++++++++");
//  Console.WriteLine(node);
        var variableName = node.AnchorToken.Lexeme;
        Console.WriteLine($"variable: {variableName}");
        if(pasones == 0)
        if (globVars.Contains(variableName))
                throw new SemanticError(
                              "Duplicated variable: " + variableName,
                              node.AnchorToken);
        else {
                Console.WriteLine($"Agregando Variable: {variableName} ");
                globVars.Add(variableName);
                //Table[variableName] =
                // typeMapper[node.AnchorToken.Category];
        }
}

//-----------------------------------------------------------
public void Visit(NFunDef node) {
        var cont = 0;
        Console.WriteLine($"+++++++++++++++ NFUNDEF ++++++++++++++++");
        Console.WriteLine(node);
        var funName = node.AnchorToken.Lexeme;
        Console.WriteLine($"Funcion: {funName}");
        if(pasones == 0)
        {
        if (Table.Contains(funName) && pasones == 0)
          throw new SemanticError(
                        "Repeated Function Declaration: " + funName,
                        node.AnchorToken);

        foreach(Node i in node.children)
        {
                Console.WriteLine($"Funcion: {i}");

                if(i.GetType().ToString() == "Int64.NParameterList")
                        foreach(Node j in i.children)
                                cont++;

        }
        Console.WriteLine($"Cont =  {cont}");

        Modishness mo = new Modishness(funName, cont);
        Console.WriteLine($"modishnes: {mo.name}, {mo.args}, {mo.predef}");
        Table[funName] = mo;
      }

      else if (pasones == 2)
      {
      Console.WriteLine("\n\n\n\n\n\t\t\t\tVoy en el tercer pason");
      Console.WriteLine($"\n\n\n\n\n\t\t\t\tnode: {node}");


       VisitChildren(node);
      }


}

//-----------------------------------------------------------
public void Visit(NStmtList node) {

  Console.WriteLine($"+++++++++++++++ NStmtList ++++++++++++++++");
        VisitChildren(node);
}


//-----------------------------------------------------------
public void Visit(NFunCall node) {

  Console.WriteLine($"+++++++++++++++ NFunCall ++++++++++++++++");
        VisitChildren(node);
}

//-----------------------------------------------------------
public void Visit(NExprList node) {

  Console.WriteLine($"+++++++++++++++ NExprList ++++++++++++++++");
        VisitChildren(node);
}

//-----------------------------------------------------------
public void Visit(NAssign node) {
  Console.WriteLine($"+++++++++++++++ NAssign ++++++++++++++++");
        var variableName = node.AnchorToken.Lexeme;

      if(pasones == 0)
        if (Table.Contains(variableName)) {

                Visit((dynamic) node[0]);

        } else {
                throw new SemanticError(
                              "Undeclared variable: " + variableName,
                              node.AnchorToken);
        }

        else if(pasones == 2)
        {
          Console.WriteLine($"NAssign 3er pason  {node}");
          Visit((dynamic) node[0]);

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
  Console.WriteLine($"+++++++++++++++ NLitInt ++++++++++++++++");

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

    Console.WriteLine($"+++++++++++++++ NExprAdd ++++++++++++++++");
        VisitBinaryOperator('+', node /*, Type.INT*/);
}

//-----------------------------------------------------------
public void Visit(NExprMul node) {
        VisitBinaryOperator('*', node /*, Type.INT*/);
}

//-----------------------------------------------------------
public void Visit(NExprPrimary node) {

      Console.WriteLine($"+++++++++++++++ NExprPrimary ++++++++++++++++");
        //VisitBinaryOperator('*', node /*, Type.INT*/);
        VisitChildren(node);
}

//-----------------------------------------------------------
public void VisitChildren(Node node) {
        foreach (var n in node.children) {

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

        Visit((dynamic) node[0]);
        Visit((dynamic) node[1]);
}
}
}
