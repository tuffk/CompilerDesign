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
public static int pasones;  //variable para contar el número de recorrido del árbol
public static string nombreFuncion; //variable para guardar el nombre de la funcion que se está recorriendo
public static string llamadaFuncion; //variable para guardar el nombre a una llamada de función
public static Token errorFunctionCall; //auxiliar para marcar row y column en NExprList
public static int variablePosition; //contador para segundo recorrido poner en que posición está tal argumento
public static int contadorArgumento; //contador de argumentos en una llamada de funcion
public static int inloop;

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
        inloop = 0;
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
        else if(pasones == 1) {
                /*If SemanticError de main*/
                if (Table.Contains("main") == false)
                        throw new SemanticError("No main function was found ");
                else if(Table["main"].args > 0)
                        throw new SemanticError("main function must have 0 parameters");
                Visit((dynamic) node[1]);

        }
        else if(pasones == 2) {
                Visit((dynamic) node[1]);
        }
}

//-----------------------------------------------------------
public void Visit(NVarDefList node) {
        Console.WriteLine($"+++++++++++++++ NVARDEFLSIT ++++++++++++++++");
        //Console.WriteLine($"n0: ${node.GetType()}");

        variablePosition=0;

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
        if(pasones == 1)
        {
                Sharmuta sha = new Sharmuta(node.AnchorToken.Lexeme, true,variablePosition);
                Table[nombreFuncion].locTable[node.AnchorToken.Lexeme] = sha;
                variablePosition++;
        }
        VisitChildren(node);
}

//-----------------------------------------------------------
public void Visit(NVarDef node) {
        Console.WriteLine($"+++++++++++++++ NVARDEF ++++++++++++++++");

        var variableName = node.AnchorToken.Lexeme;
        //Console.WriteLine($"variable: {variableName}");
        if(pasones == 0)
                if (globVars.Contains(variableName))
                        throw new SemanticError(
                                      "Duplicated variable: " + variableName,
                                      node.AnchorToken);
                else {
                        //Console.WriteLine($"Agregando Variable: {variableName} ");
                        globVars.Add(variableName);
                }
        else if(pasones == 1)
        {
                if(Table[nombreFuncion].locTable.ContainsKey(node.AnchorToken.Lexeme))
                        throw new SemanticError("parameter and local variable names have to be unique on function: " + nombreFuncion,
                                                node.AnchorToken);

                Sharmuta sha = new Sharmuta(node.AnchorToken.Lexeme, false, null);
                Table[nombreFuncion].locTable[node.AnchorToken.Lexeme] = sha;
        }
}

//-----------------------------------------------------------
public void Visit(NFunDef node) {
        var cont = 0;
        Console.WriteLine($"+++++++++++++++ NFUNDEF ++++++++++++++++");
        //Console.WriteLine(node);
        var funName = node.AnchorToken.Lexeme;
        Console.WriteLine($"\t\t\t\t\t\t\tEvaluando la funcion: {funName}");
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

        else if(pasones ==1 )
        {
                nombreFuncion = funName;
                Table[nombreFuncion].locTable = new SortedDictionary<string, Sharmuta>();
                VisitChildren(node);

        }

        else if (pasones == 2)
        {
                nombreFuncion = funName;
                Console.WriteLine("\n\n\n\n\n\t\t\t\tVoy en el tercer pason");
                Console.WriteLine($"\t\t\t\tnode: {node}");

                VisitChildren(node);
        }


}

//-----------------------------------------------------------
public void Visit(NStmtList node) {

        Console.WriteLine($"+++++++++++++++ NStmtList ++++++++++++++++");
        VisitChildren(node);
}

//-----------------------------------------------------------
public void Visit(NBreak node) {

        Console.WriteLine($"+++++++++++++++ NBreak ++++++++++++++++");
        if (pasones == 2)
        {
                if (inloop > 0) {
                        VisitChildren(node);
                }else{
                  throw new SemanticError("unexpected 'break'", node.AnchorToken);
                }
        }else {
                VisitChildren(node);
        }
}


//-----------------------------------------------------------
public void Visit(NContinue node) {

        Console.WriteLine($"+++++++++++++++ NContinue ++++++++++++++++");
        if (pasones == 2)
        {
                if (inloop > 0) {
                        VisitChildren(node);
                }else{
                  throw new SemanticError("unexpected 'continue'", node.AnchorToken);
                }
        }else {
                VisitChildren(node);
        }
}



//-----------------------------------------------------------
public void Visit(NReturn node) {

        Console.WriteLine($"+++++++++++++++ NReturn ++++++++++++++++");
        VisitChildren(node);
}


//-----------------------------------------------------------
public void Visit(NFunCall node) {

        Console.WriteLine($"+++++++++++++++ NFunCall ++++++++++++++++");
        if(pasones == 2)
        {
                errorFunctionCall = node.AnchorToken;
                llamadaFuncion = node.AnchorToken.Lexeme;
                if(!Table.Contains(node.AnchorToken.Lexeme))
                        throw new SemanticError("function [" + node.AnchorToken.Lexeme +"] has not been declared",
                                                node.AnchorToken);
        }

        VisitChildren(node);
}

//-----------------------------------------------------------
public void Visit(NExprList node) {

        Console.WriteLine($"+++++++++++++++ NExprList ++++++++++++++++");
        if(pasones == 2) {
                foreach(var i in node) {
                        contadorArgumento++;
                }
                if(Table[llamadaFuncion].args != contadorArgumento)
                        throw new SemanticError("expected " + Table[llamadaFuncion].args + $" arguments in function call  [{llamadaFuncion}]",
                                                errorFunctionCall);


        }
        contadorArgumento = 0;



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
                //Console.WriteLine($"\n\n\n\n\t\t\t\t\t\tNAssign 3er pason Funcion  {nombreFuncion}  {node.AnchorToken.Lexeme}");
                if(!Table[nombreFuncion].locTable.ContainsKey(node.AnchorToken.Lexeme))
                        if(!globVars.Contains(node.AnchorToken.Lexeme))
                                throw new SemanticError("variable ["+node.AnchorToken.Lexeme+"] has not been declared ",
                                                        node.AnchorToken);


                Visit((dynamic) node[0]);

        }
}

// -----------------------------------------------------------
public void Visit(Print node) {
        Console.WriteLine($"+++++++++++++++ Print ++++++++++++++++");
        VisitChildren(node);
        //  node.ExpressionType = Visit((dynamic) node[0]);
}

//-----------------------------------------------------------
public void Visit(NIfStmt node) {
        // if (Visit((dynamic) node[0]) != Type.BOOL) {
        //         throw new SemanticError(
        //                       "Expecting type " + Type.BOOL
        //                       + " in conditional statement",
        //                       node.AnchorToken);
        // }
        VisitChildren(node);
}

//------------------------------------------------------------
public void Visit(NSwitchStmt node) {
        Console.WriteLine($"+++++++++++++++ NSwitchStmt ++++++++++++++++");

        VisitChildren(node);
}

//------------------------------------------------------------
public void Visit(NCaseList node) {
        Console.WriteLine($"+++++++++++++++ NCaseList ++++++++++++++++");

        VisitChildren(node);
}

//------------------------------------------------------------
public void Visit(NCase node) {
        Console.WriteLine($"+++++++++++++++ NCase ++++++++++++++++");

        VisitChildren(node);
}

//------------------------------------------------------------
public void Visit(NLitBool node) {
        Console.WriteLine($"+++++++++++++++ NLitBool ++++++++++++++++");

        //VisitChildren(node);
}

//------------------------------------------------------------
public void Visit(NWhileStmt node) {
        Console.WriteLine($"+++++++++++++++ NWhileStmt ++++++++++++++++");
        if (pasones == 2) {
          inloop++;
        }
        VisitChildren(node);
        if (pasones == 2) {
          inloop--;
        }
}

//------------------------------------------------------------
public void Visit(NLitChar node) {
        Console.WriteLine($"+++++++++++++++ NLitChar ++++++++++++++++");
        Console.WriteLine($"\n\n\n\n\n\t\t\tnode:    {node.GetType()}");
        //VisitChildren(node);
}
//------------------------------------------------------------
public void Visit(NDoWhileStmt node) {
        Console.WriteLine($"+++++++++++++++ NDoWhileStmt ++++++++++++++++");
        if (pasones == 2) {
          inloop++;
        }
        VisitChildren(node);
        if (pasones == 2) {
          inloop--;
        }
}
//------------------------------------------------------------
public void Visit(NForStmt node) {
        Console.WriteLine($"+++++++++++++++ NForStmt ++++++++++++++++");
        if (pasones == 2) {
          inloop++;
        }
        VisitChildren(node);
        if (pasones == 2) {
          inloop--;
        }
}
//------------------------------------------------------------
public void Visit(NExpr node) {
        Console.WriteLine($"+++++++++++++++ NExpr ++++++++++++++++");

        //VisitChildren(node);
}
//------------------------------------------------------------
public void Visit(NExprOr node) {
        Console.WriteLine($"+++++++++++++++ NExprOr ++++++++++++++++");

        //VisitChildren(node);
}
//------------------------------------------------------------
public void Visit(NExprRel node) {
        Console.WriteLine($"+++++++++++++++ NExprRel ++++++++++++++++");

        //VisitChildren(node);
}
//------------------------------------------------------------
public void Visit(NExprBitOr node) {
        Console.WriteLine($"+++++++++++++++ NExprBitOr ++++++++++++++++");

        //VisitChildren(node);
}
//------------------------------------------------------------
public void Visit(NExprBitAnd node) {
        Console.WriteLine($"+++++++++++++++ NExprBitAnd ++++++++++++++++");

        //VisitChildren(node);
}
//------------------------------------------------------------
public void Visit(NExprBitShift node) {
        Console.WriteLine($"+++++++++++++++ NExprBitShift ++++++++++++++++");

        //VisitChildren(node);
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
          if(intStr.StartsWith("0o") || intStr.StartsWith("0O")){
            intStr = intStr.Remove(0,2);
            Convert.ToInt64(intStr,8);
          }
          else if(intStr.StartsWith("0b") || intStr.StartsWith("0B")){
            intStr = intStr.Remove(0,2);
            Convert.ToInt64(intStr,2);
          }
          else if(intStr.StartsWith("0x") || intStr.StartsWith("0X")){
            intStr = intStr.Remove(0,2);
            Convert.ToInt64(intStr,16);
          }
              //  Convert.ToInt64(intStr);
                Console.WriteLine("EXITO!!");

        } catch (OverflowException) {
                throw new SemanticError(
                              "Integer literal too large: " + intStr,
                              node.AnchorToken);
        }
}

//-----------------------------------------------------------
public void Visit(NLitString node) {
        Console.WriteLine($"+++++++++++++++ NLitString ++++++++++++++++");

        /*var intStr = node.AnchorToken.Lexeme;

           try {
                Convert.ToInt32(intStr);

           } catch (OverflowException) {
                throw new SemanticError(
                              "Integer literal too large: " + intStr,
                              node.AnchorToken);
           }*/
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
public void Visit(NExprPow node) {
        VisitBinaryOperator('^', node /*, Type.INT*/);
}

//-----------------------------------------------------------
public void Visit(NExprPrimary node) {

        Console.WriteLine($"+++++++++++++++ NExprPrimary ++++++++++++++++");

        if(pasones == 2)
                if(!Table[nombreFuncion].locTable.ContainsKey(node.AnchorToken.Lexeme))
                        if(!globVars.Contains(node.AnchorToken.Lexeme))
                                throw new SemanticError("variable ["+node.AnchorToken.Lexeme+"] has not been declared ",
                                                        node.AnchorToken);

        //Console.WriteLine($"\t\t\t\t\t\t\t\t+++++++++++++++ 3er pason  {node.AnchorToken.Lexeme}  ++++++++++++++++");

        //VisitBinaryOperator('*', node /*, Type.INT*/);
        VisitChildren(node);
}
//-----------------------------------------------------------
public void Visit(NExprUnary node) {

        Console.WriteLine($"+++++++++++++++ NExprUnary ++++++++++++++++");
        //VisitBinaryOperator('*', node /*, Type.INT*/);
        VisitChildren(node);
}
//-----------------------------------------------------------
public void Visit(NArrayList node) {

        Console.WriteLine($"+++++++++++++++ NArrayList ++++++++++++++++");
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
