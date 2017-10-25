/*
  Jaime Margolin A01019332
  Juan carlos Leon A01020200
  Rodrigo Solana A01129839
*/

using System;
using System.Collections.Generic;

namespace Int64 {

	class Parser {

		static readonly ISet<TokenCategory> firstOfProgram =
		new HashSet<TokenCategory>() {
			TokenCategory.IDENTIFIER,
			TokenCategory.VAR,
			TokenCategory.EOF
		};

		static readonly ISet<TokenCategory> firstOfStatement =
		new HashSet<TokenCategory>() {
			TokenCategory.IDENTIFIER,
			TokenCategory.IF,
			TokenCategory.SWITCH,
			TokenCategory.WHILE,
			TokenCategory.DO,
			TokenCategory.FOR,
			TokenCategory.BREAK,
			TokenCategory.CONTINUE,
			TokenCategory.RETURN,
			TokenCategory.SEMICOLON
		};

		static readonly ISet<TokenCategory> firstOfUnary =
		new HashSet<TokenCategory>() {
			TokenCategory.ADDITION,
			TokenCategory.SUBTRACTION,
			TokenCategory.BIT_NOT,
			TokenCategory.NOT
		};

		static readonly ISet<TokenCategory> firstOfLit =
		new HashSet<TokenCategory>() {
			TokenCategory.TRUE,
			TokenCategory.FALSE,
			TokenCategory.BASE_2,
			TokenCategory.BASE_8,
			TokenCategory.BASE_10,
			TokenCategory.BASE_16,
			TokenCategory.CHARACTER
		};

		static readonly ISet<TokenCategory> firstOfLitAlt =
		new HashSet<TokenCategory>() {
			TokenCategory.TRUE,
			TokenCategory.FALSE,
			TokenCategory.BASE_2,
			TokenCategory.BASE_8,
			TokenCategory.BASE_10,
			TokenCategory.BASE_16,
			TokenCategory.CHARACTER,
			TokenCategory.STRING,
			TokenCategory.CURLY_BRACE_LEFT
		};

		static readonly ISet<TokenCategory> firstOfExprPrimary =
		new HashSet<TokenCategory>() {
			TokenCategory.IDENTIFIER,
			TokenCategory.TRUE,
			TokenCategory.FALSE,
			TokenCategory.BASE_2,
			TokenCategory.BASE_8,
			TokenCategory.BASE_10,
			TokenCategory.BASE_16,
			TokenCategory.CHARACTER,
			TokenCategory.STRING,
			TokenCategory.CURLY_BRACE_LEFT,
			TokenCategory.PARENTHESIS_LEFT
		};

		IEnumerator<Token> tokenStream;

		public Parser(IEnumerator<Token> tokenStream) {
			this.tokenStream = tokenStream;
			this.tokenStream.MoveNext();
		}

		public TokenCategory CurrentToken {
			get { return tokenStream.Current.Category; }
		}

		public Token Expect(TokenCategory category) {
			//Console.WriteLine("Expecting: " + tokenStream.Current.Lexeme);
			//Console.WriteLine(System.Environment.StackTrace);
			if (CurrentToken == category) {
				//Console.WriteLine("Consuming: " + tokenStream.Current.Lexeme);
				Token current = tokenStream.Current;
				tokenStream.MoveNext();
				return current;
			} else {
				throw new SyntaxError(category, tokenStream.Current);
			}
		}

		// Grammar entry point
		// Returns NProgram
		public Node CProgram() {
			NProgram nProgram = (NProgram)ProgramRecursive();
			Expect(TokenCategory.EOF);
			return nProgram;
		}

		// Returns NProgram
		public Node ProgramRecursive() {
			NProgram nProgram = new NProgram();
			NVarDefList nVarDefList = new NVarDefList();
			NFunDefList nFunDefList = new NFunDefList();
			while (firstOfProgram.Contains(CurrentToken)) {
				if (CurrentToken == TokenCategory.VAR) {
					Vareamela(nVarDefList);
				}
				else if (CurrentToken == TokenCategory.IDENTIFIER) {
					Funcionamela(nFunDefList);
				}
				else if (CurrentToken == TokenCategory.EOF) {
					break;
				}
				else {
					throw new SyntaxError(firstOfProgram, tokenStream.Current);
				}
			}
			if (!firstOfProgram.Contains(CurrentToken)) {
				throw new SyntaxError(new HashSet<TokenCategory>() {
					TokenCategory.IDENTIFIER,
					TokenCategory.VAR,
					TokenCategory.EOF
				}
				, tokenStream.Current);
			}
			nProgram.Add(nVarDefList);
			nProgram.Add(nFunDefList);
			return nProgram;
		}

		// Appends NVarDef to nVarDefList
		public void Vareamela(NVarDefList nVarDefList) {
			NVarDef nVarDef = new NVarDef();
			Expect(TokenCategory.VAR);
			nVarDef.AnchorToken = Expect(TokenCategory.IDENTIFIER);
			nVarDefList.Add(nVarDef);
			while (CurrentToken==TokenCategory.COMMA) {
				Expect(TokenCategory.COMMA);
				NVarDef otherNVarDef = new NVarDef();
				otherNVarDef.AnchorToken = Expect(TokenCategory.IDENTIFIER);
				nVarDefList.Add(otherNVarDef);
			}
			Expect(TokenCategory.SEMICOLON);
		}

		// Appends NFunDef to nFunDefList
		public void Funcionamela(NFunDefList nFunDefList) {
			NFunDef nFunDef = new NFunDef(){
				AnchorToken = Expect(TokenCategory.IDENTIFIER)
			};
			NParameterList nParameterList = new NParameterList();
			Expect(TokenCategory.PARENTHESIS_LEFT);
			if (CurrentToken==TokenCategory.IDENTIFIER) {
				nParameterList.Add(new NParameter(){
					AnchorToken = Expect(TokenCategory.IDENTIFIER)
				});
				while (CurrentToken==TokenCategory.COMMA) {
					Expect(TokenCategory.COMMA);
					nParameterList.Add(new NParameter(){
						AnchorToken = Expect(TokenCategory.IDENTIFIER)
					});
				}
			}
			Expect(TokenCategory.PARENTHESIS_RIGHT);
			Expect(TokenCategory.CURLY_BRACE_LEFT);
			NVarDefList nVarDefList = (NVarDefList)DefinitionContinuer();
			NStmtList nStmtList = (NStmtList)StatmentList();
			Expect(TokenCategory.CURLY_BRACE_RIGHT);
			nFunDef.Add(nParameterList);
			nFunDef.Add(nVarDefList);
			nFunDef.Add(nStmtList);
			nFunDefList.Add(nFunDef);
		}

		// Returns NVarDefList
		public Node DefinitionContinuer() {
			NVarDefList nVarDefList = new NVarDefList();
			while (CurrentToken == TokenCategory.VAR) {
				Vareamela(nVarDefList);
			}
			return nVarDefList;
		}

		// Returns NStmtList
		public Node StatmentList() {
			NStmtList nStmtList = new NStmtList();
			while (firstOfStatement.Contains(CurrentToken)) {
				Statment(nStmtList);
			}
			return nStmtList;
		}

		// Appends NStmt to nStmtList
		public void Statment(NStmtList nStmtList) {
			switch(CurrentToken) {
				case TokenCategory.IDENTIFIER: {
					Token tokenForAnchor = Expect(TokenCategory.IDENTIFIER);
					if (CurrentToken==TokenCategory.ASSIGN) {
						NAssign nAssign = new NAssign(){
							AnchorToken = tokenForAnchor
						};
						nAssign.Add(Assign());
						nStmtList.Add(nAssign);
					}
					else if (CurrentToken==TokenCategory.PARENTHESIS_LEFT) {
						NFunCall nFunCall = new NFunCall(){
							AnchorToken = tokenForAnchor
						};
						nFunCall.Add(FunCall());
						nStmtList.Add(nFunCall);
					}
					else {
						throw new SyntaxError(new HashSet<TokenCategory>() {
							TokenCategory.ASSIGN,
							TokenCategory.PARENTHESIS_LEFT
						}
						, tokenStream.Current);
					}
					break;
				}
				case TokenCategory.IF: {
					Expect(TokenCategory.IF);
					nStmtList.Add(IfStmt());
					break;
				}
				case TokenCategory.SWITCH: {
					nStmtList.Add(SwitchStmt());
					break;
				}
				case TokenCategory.WHILE: {
					nStmtList.Add(WhileStmt());
					break;
				}
				case TokenCategory.DO: {
					nStmtList.Add(DoWhileStmt());
					break;
				}
				case TokenCategory.FOR: {
					nStmtList.Add(ForStmt());
					break;
				}
				case TokenCategory.BREAK: {
					nStmtList.Add(new NBreak() {
						AnchorToken = Expect(TokenCategory.BREAK)
					});
					Expect(TokenCategory.SEMICOLON);
					break;
				}
				case TokenCategory.CONTINUE: {
					nStmtList.Add(new NContinue() {
						AnchorToken = Expect(TokenCategory.CONTINUE)
					});
					Expect(TokenCategory.SEMICOLON);
					break;
				}
				case TokenCategory.RETURN: {
					Expect(TokenCategory.RETURN);
					nStmtList.Add(new NReturn(){
						Expr()
					});
					Expect(TokenCategory.SEMICOLON);
					break;
				}
				case TokenCategory.SEMICOLON: {
					Expect(TokenCategory.SEMICOLON);
					break;
				}
				default: {
					throw new SyntaxError(firstOfStatement, tokenStream.Current);
				}
			}
		}

		// Returns NAssign
		public Node Assign() {
			Expect(TokenCategory.ASSIGN);
			NExpr nExpr = (NExpr)Expr();
			Expect(TokenCategory.SEMICOLON);
			return nExpr;
		}

		// Returns NExprList
		public Node FunCall() {
			NExprList nExprList = new NExprList();
			Expect(TokenCategory.PARENTHESIS_LEFT);
			if (firstOfExprPrimary.Contains(CurrentToken)) {
				nExprList.Add(Expr());
				while (CurrentToken==TokenCategory.COMMA) {
					Expect(TokenCategory.COMMA);
					nExprList.Add(Expr());
				}
			}
			Expect(TokenCategory.PARENTHESIS_RIGHT);
			return nExprList;
		}

		// Returns NIfStmt
		public Node IfStmt() {
			NIfStmt nIfStmt = new NIfStmt();
			Expect(TokenCategory.PARENTHESIS_LEFT);
			nIfStmt.Add(Expr());
			Expect(TokenCategory.PARENTHESIS_RIGHT);
			Expect(TokenCategory.CURLY_BRACE_LEFT);
			NStmtList nStmtList = new NStmtList();
			while (firstOfStatement.Contains(CurrentToken)) {
				Statment(nStmtList);
			}
			nIfStmt.Add(nStmtList);
			Expect(TokenCategory.CURLY_BRACE_RIGHT);
			while (CurrentToken==TokenCategory.ELSE) {
				Expect(TokenCategory.ELSE);
				if (CurrentToken==TokenCategory.IF) {
					Expect(TokenCategory.IF);
					Expect(TokenCategory.PARENTHESIS_LEFT);
					nIfStmt.Add(Expr());
					Expect(TokenCategory.PARENTHESIS_RIGHT);
					Expect(TokenCategory.CURLY_BRACE_LEFT);
					NStmtList otherNStmtList = new NStmtList();
					while (firstOfStatement.Contains(CurrentToken)) {
						Statment(otherNStmtList);
					}
					nIfStmt.Add(otherNStmtList);
					Expect(TokenCategory.CURLY_BRACE_RIGHT);
				}
				else if (CurrentToken==TokenCategory.CURLY_BRACE_LEFT) {
					Expect(TokenCategory.CURLY_BRACE_LEFT);
					NStmtList otherNStmtList = new NStmtList();
					while (firstOfStatement.Contains(CurrentToken)) {
						Statment(otherNStmtList);
					}
					nIfStmt.Add(otherNStmtList);
					Expect(TokenCategory.CURLY_BRACE_RIGHT);
					break;
				}
			}
			return nIfStmt;
		}

		// Return NSwitchStmt
		public Node SwitchStmt() {
			NSwitchStmt nSwitchStmt = new NSwitchStmt();
			Expect(TokenCategory.SWITCH);
			Expect(TokenCategory.PARENTHESIS_LEFT);
			nSwitchStmt.Add(Expr());
			Expect(TokenCategory.PARENTHESIS_RIGHT);
			Expect(TokenCategory.CURLY_BRACE_LEFT);
			NCaseList nCaseList = new NCaseList();
			while (CurrentToken==TokenCategory.CASE) {
				nCaseList.Add(Case());
			}
			nSwitchStmt.Add(nCaseList);
			if (CurrentToken==TokenCategory.DEFAULT) {
				nSwitchStmt.Add(Default());
			}
			Expect(TokenCategory.CURLY_BRACE_RIGHT);
			return nSwitchStmt;
		}

		//Returns NCase
		public Node Case() {
			NCase nCase = new NCase();
			Expect(TokenCategory.CASE);
			nCase.Add(Lit());
			while (CurrentToken==TokenCategory.COMMA) {
				Expect(TokenCategory.COMMA);
				nCase.Add(Lit());
			}
			Expect(TokenCategory.COLON);
			NStmtList nStmtList = new NStmtList();
			while (firstOfStatement.Contains(CurrentToken)) {
				Statment(nStmtList);
			}
			nCase.Add(nStmtList);
			return nCase;
		}

		// Returns NLitInt, NLitBool or NLitChar
		public Node Lit() {
			switch(CurrentToken) {
				case TokenCategory.TRUE: {
					return new NLitBool(){
						AnchorToken = Expect(TokenCategory.TRUE)
					};
				}
				case TokenCategory.FALSE: {
					return new NLitBool(){
						AnchorToken = Expect(TokenCategory.FALSE)
					};
				}
				case TokenCategory.BASE_2: {
					return new NLitInt(){
						AnchorToken = Expect(TokenCategory.BASE_2)
					};
				}
				case TokenCategory.BASE_8: {
					return new NLitInt(){
						AnchorToken = Expect(TokenCategory.BASE_8)
					};
				}
				case TokenCategory.BASE_10: {
					return new NLitInt(){
						AnchorToken = Expect(TokenCategory.BASE_10)
					};
				}
				case TokenCategory.BASE_16: {
					return new NLitInt(){
						AnchorToken = Expect(TokenCategory.BASE_16)
					};
				}
				case TokenCategory.CHARACTER: {
					return new NLitChar(){
						AnchorToken = Expect(TokenCategory.CHARACTER)
					};
				}
				default: {
					throw new SyntaxError(new HashSet<TokenCategory>() {
						TokenCategory.TRUE,
						TokenCategory.FALSE,
						TokenCategory.BASE_2,
						TokenCategory.BASE_8,
						TokenCategory.BASE_10,
						TokenCategory.BASE_16,
						TokenCategory.CHARACTER
					}
					, tokenStream.Current);
				}
			}
		}

		// Returns NStmtList
		public Node Default() {
			NStmtList nStmtList = new NStmtList();
			Expect(TokenCategory.DEFAULT);
			Expect(TokenCategory.COLON);
			while (firstOfStatement.Contains(CurrentToken)) {
				Statment(nStmtList);
			}
			return nStmtList;
		}

		// Returns NWhileStmt
		public Node WhileStmt() {
			NWhileStmt nWhileStmt = new NWhileStmt();
			Expect(TokenCategory.WHILE);
			Expect(TokenCategory.PARENTHESIS_LEFT);
			nWhileStmt.Add(Expr());
			Expect(TokenCategory.PARENTHESIS_RIGHT);
			Expect(TokenCategory.CURLY_BRACE_LEFT);
			NStmtList nStmtList = new NStmtList();
			while (firstOfStatement.Contains(CurrentToken)) {
				Statment(nStmtList);
			}
			nWhileStmt.Add(nStmtList);
			Expect(TokenCategory.CURLY_BRACE_RIGHT);
			return nWhileStmt;
		}

		// Returns NDoWhileStmt
		public Node DoWhileStmt() {
			NDoWhileStmt nDoWhileStmt = new NDoWhileStmt();
			Expect(TokenCategory.DO);
			Expect(TokenCategory.CURLY_BRACE_LEFT);
			NStmtList nStmtList = new NStmtList();
			while (firstOfStatement.Contains(CurrentToken)) {
				Statment(nStmtList);
			}
			nDoWhileStmt.Add(nStmtList);
			Expect(TokenCategory.CURLY_BRACE_RIGHT);
			Expect(TokenCategory.WHILE);
			Expect(TokenCategory.PARENTHESIS_LEFT);
			nDoWhileStmt.Add(Expr());
			Expect(TokenCategory.PARENTHESIS_RIGHT);
			Expect(TokenCategory.SEMICOLON);
			return nDoWhileStmt;
		}

		// Returns NForStmt
		public Node ForStmt() {
			Expect(TokenCategory.FOR);
			Expect(TokenCategory.PARENTHESIS_LEFT);
			NForStmt nForStmt = new NForStmt() {
				AnchorToken = Expect(TokenCategory.IDENTIFIER)
			};
			Expect(TokenCategory.IN);
			nForStmt.Add(Expr());
			Expect(TokenCategory.PARENTHESIS_RIGHT);
			Expect(TokenCategory.CURLY_BRACE_LEFT);
			NStmtList nStmtList = new NStmtList();
			while (firstOfStatement.Contains(CurrentToken)) {
				Statment(nStmtList);
			}
			nForStmt.Add(nStmtList);
			Expect(TokenCategory.CURLY_BRACE_RIGHT);
			return nForStmt;
		}

		// Returns NExpr
		public Node Expr() {
			NExpr nExpr = new NExpr();
			nExpr.Add(ExprOr());
			if (CurrentToken==TokenCategory.QUESTION_MARK) {
				Expect(TokenCategory.QUESTION_MARK);
				nExpr.Add(Expr());
				Expect(TokenCategory.COLON);
				nExpr.Add(Expr());
			}
			return nExpr;
		}

		// Returns NExprOr
		public Node ExprOr() {
			NExprOr nExprOr = new NExprOr();
			nExprOr.Add(ExprAnd());
			while (CurrentToken==TokenCategory.OR) {
				Expect(TokenCategory.OR);
				nExprOr.Add(ExprAnd());
			}
			return nExprOr;
		}

		// Returns NExprAnd
		public Node ExprAnd() {
			NExprAnd nExprAnd = new NExprAnd();
			nExprAnd.Add(ExprComp());
			while (CurrentToken==TokenCategory.AND) {
				Expect(TokenCategory.AND);
				nExprAnd.Add(ExprComp());
			}
			return nExprAnd;
		}

		// Returns NExprComp
		public Node ExprComp() {
			NExprComp nExprComp = new NExprComp();
			nExprComp.Add(ExprRel());
			if (CurrentToken == TokenCategory.NOT_EQUAL || CurrentToken == TokenCategory.EQUAL) {
				switch(CurrentToken) {
					case TokenCategory.NOT_EQUAL: {
						nExprComp.AnchorToken = Expect(TokenCategory.NOT_EQUAL);
						nExprComp.Add(ExprComp());
						break;
					}
					case TokenCategory.EQUAL: {
						nExprComp.AnchorToken = Expect(TokenCategory.EQUAL);
						nExprComp.Add(ExprComp());
						break;
					}
					// There is no default as it would not be reachable
				}
			}
			return nExprComp;
		}

		// Returns NExprRel
		public Node ExprRel() {
			NExprRel nExprRel = new NExprRel();
			nExprRel.Add(ExprBitOr());
			if (CurrentToken == TokenCategory.LESS_THAN || CurrentToken == TokenCategory.LESS_OR_EQUAL_THAN || CurrentToken == TokenCategory.GREATER_THAN || CurrentToken == TokenCategory.GREATER_OR_EQUAL_THAN) {
				switch(CurrentToken) {
					case TokenCategory.LESS_THAN: {
						nExprRel.AnchorToken = Expect(TokenCategory.LESS_THAN);
						nExprRel.Add(ExprRel());
						break;
					}
					case TokenCategory.LESS_OR_EQUAL_THAN: {
						nExprRel.AnchorToken = Expect(TokenCategory.LESS_OR_EQUAL_THAN);
						nExprRel.Add(ExprRel());
						break;
					}
					case TokenCategory.GREATER_THAN: {
						nExprRel.AnchorToken = Expect(TokenCategory.GREATER_THAN);
						nExprRel.Add(ExprRel());
						break;
					}
					case TokenCategory.GREATER_OR_EQUAL_THAN: {
						nExprRel.AnchorToken = Expect(TokenCategory.GREATER_OR_EQUAL_THAN);
						nExprRel.Add(ExprRel());
						break;
					}
					// Default case would be unreachable
				}
			}
			return nExprRel;
		}

		// Returns NExprBitOr
		public Node ExprBitOr() {
			NExprBitOr nExprBitOr = new NExprBitOr();
			nExprBitOr.Add(ExprBitAnd());
			if (CurrentToken == TokenCategory.BIT_OR || CurrentToken == TokenCategory.XOR) {
				switch(CurrentToken) {
					case TokenCategory.BIT_OR: {
						nExprBitOr.AnchorToken = Expect(TokenCategory.BIT_OR);
						nExprBitOr.Add(ExprBitOr());
						break;
					}
					case TokenCategory.XOR: {
						nExprBitOr.AnchorToken = Expect(TokenCategory.XOR);
						nExprBitOr.Add(ExprBitOr());
						break;
					}
					// Default case would be unreachable
				}
			}
			return nExprBitOr;
		}

		// Returns NExprBitAnd
		public Node ExprBitAnd() {
			NExprBitAnd nExprBitAnd = new NExprBitAnd();
			nExprBitAnd.Add(ExprBitShift());
			while (CurrentToken==TokenCategory.BIT_AND) {
				switch(CurrentToken) {
					case TokenCategory.BIT_AND: {
						Expect(TokenCategory.BIT_AND);
						nExprBitAnd.Add(ExprBitAnd());
						break;
					}
					// Default case would be unreachable
				}
			}
			return nExprBitAnd;
		}

		// Returns NExprBitShift
		public Node ExprBitShift() {
			NExprBitShift nExprBitShift = new NExprBitShift();
			nExprBitShift.Add(ExprAdd());
			if (CurrentToken == TokenCategory.SHIFT_LEFT || CurrentToken == TokenCategory.SHIFT_RIGHT || CurrentToken == TokenCategory.SHIFT_RIGHT_ALT) {
				switch(CurrentToken) {
					case TokenCategory.SHIFT_LEFT: {
						nExprBitShift.AnchorToken = Expect(TokenCategory.SHIFT_LEFT);
						nExprBitShift.Add(ExprBitShift());
						break;
					}
					case TokenCategory.SHIFT_RIGHT: {
						nExprBitShift.AnchorToken = Expect(TokenCategory.SHIFT_RIGHT);
						nExprBitShift.Add(ExprBitShift());
						break;
					}
					case TokenCategory.SHIFT_RIGHT_ALT: {
						nExprBitShift.AnchorToken = Expect(TokenCategory.SHIFT_RIGHT_ALT);
						nExprBitShift.Add(ExprBitShift());
						break;
					}
					// Default case would be unreachable
				}
			}
			return nExprBitShift;
		}

		// Returns NExprAdd
		public Node ExprAdd() {
			NExprAdd nExprAdd = new NExprAdd();
			nExprAdd.Add(ExprMul());
			while (CurrentToken==TokenCategory.SUBTRACTION||CurrentToken==TokenCategory.ADDITION) {
				switch(CurrentToken) {
					case TokenCategory.SUBTRACTION: {
						nExprAdd.AnchorToken = Expect(TokenCategory.SUBTRACTION);
						nExprAdd.Add(ExprAdd());
						break;
					}
					case TokenCategory.ADDITION: {
						nExprAdd.AnchorToken = Expect(TokenCategory.ADDITION);
						nExprAdd.Add(ExprAdd());
						break;
					}
					// Default case would be unreachable
				}
			}
			return nExprAdd;
		}

		// Returns NExprMul
		public Node ExprMul() {
			NExprMul nExprMul = new NExprMul();
			nExprMul.Add(ExprPow());
			if (CurrentToken == TokenCategory.MULTIPLICATION || CurrentToken == TokenCategory.DIVISION || CurrentToken == TokenCategory.MODULUS) {
				switch(CurrentToken) {
					case TokenCategory.MULTIPLICATION: {
						nExprMul.AnchorToken = Expect(TokenCategory.MULTIPLICATION);
						nExprMul.Add(ExprMul());
						break;
					}
					case TokenCategory.DIVISION: {
						nExprMul.AnchorToken = Expect(TokenCategory.DIVISION);
						nExprMul.Add(ExprMul());
						break;
					}
					case TokenCategory.MODULUS: {
						nExprMul.AnchorToken = Expect(TokenCategory.MODULUS);
						nExprMul.Add(ExprMul());
						break;
					}
					// Default case would be unreachable
				}
			}
			return nExprMul;
		}

		// Returns NExprPow
		public Node ExprPow() {
			NExprPow nExprPow = new NExprPow();
			nExprPow.Add(ExprUnary());
			if (CurrentToken == TokenCategory.POWER) {
				Expect(TokenCategory.POWER);
				nExprPow.Add(ExprPow());
			}
			return nExprPow;
		}

		// Returns NExprUnary
		public Node ExprUnary() {
			NExprUnary nExprUnary = new NExprUnary();
			if (firstOfUnary.Contains(CurrentToken)) {
				switch(CurrentToken) {
					case TokenCategory.ADDITION: {
						nExprUnary.AnchorToken = Expect(TokenCategory.ADDITION);
						nExprUnary.Add(ExprUnary());
						break;
					}
					case TokenCategory.SUBTRACTION: {
						nExprUnary.AnchorToken = Expect(TokenCategory.SUBTRACTION);
						nExprUnary.Add(ExprUnary());
						break;
					}
					case TokenCategory.NOT: {
						nExprUnary.AnchorToken = Expect(TokenCategory.NOT);
						nExprUnary.Add(ExprUnary());
						break;
					}
					case TokenCategory.BIT_NOT: {
						nExprUnary.AnchorToken = Expect(TokenCategory.BIT_NOT);
						nExprUnary.Add(ExprUnary());
						break;
					}
					// Default case would be unreachable
				}
			}
			else if (firstOfExprPrimary.Contains(CurrentToken)) {
				nExprUnary.Add(ExprPrimary());
			}
			return nExprUnary;
		}

		// Returns NExprPrimary
		public Node ExprPrimary() {
			NExprPrimary nExprPrimary = new NExprPrimary();
			if (CurrentToken == TokenCategory.IDENTIFIER) {
				Token identifier = Expect(TokenCategory.IDENTIFIER);
				if (CurrentToken==TokenCategory.PARENTHESIS_LEFT) {
					NFunCall nFunCall = new NFunCall(){
						AnchorToken = identifier
					};
					nFunCall.Add(FunCall());
					nExprPrimary.Add(nFunCall);
				}
				else {
					nExprPrimary.AnchorToken = identifier;
				}
			}
			else if (firstOfLitAlt.Contains(CurrentToken)) {
				nExprPrimary.Add(LitAlt());
			}
			else if (CurrentToken==TokenCategory.PARENTHESIS_LEFT) {
				Expect(TokenCategory.PARENTHESIS_LEFT);
				nExprPrimary.Add(Expr());
				Expect(TokenCategory.PARENTHESIS_RIGHT);
			}
			return nExprPrimary;
		}

		// Returns NLitInt, NLitBool, NLitChar, NLitString or NArrayList
		public Node LitAlt() {
			if (firstOfLit.Contains(CurrentToken)) {
				return Lit();
			}
			else if (CurrentToken==TokenCategory.STRING) {
				NLitString nLitString = new NLitString(){
					AnchorToken = Expect(TokenCategory.STRING)
				};
				return nLitString;
			}
			else if (CurrentToken==TokenCategory.CURLY_BRACE_LEFT) {
				return ArrayList();
			}
			else {
				throw new SyntaxError(new HashSet<TokenCategory>() {
					TokenCategory.STRING,
					TokenCategory.CURLY_BRACE_LEFT,
					TokenCategory.TRUE,
					TokenCategory.FALSE,
					TokenCategory.BASE_2,
					TokenCategory.BASE_8,
					TokenCategory.BASE_10,
					TokenCategory.BASE_16,
					TokenCategory.CHARACTER
				}
				, tokenStream.Current);
			}
		}

		// Returns NArrayList
		public Node ArrayList() {
			NArrayList nArrayList = new NArrayList();
			Expect(TokenCategory.CURLY_BRACE_LEFT);
			if (firstOfLit.Contains(CurrentToken)) {
				nArrayList.Add(Lit());
				while (CurrentToken==TokenCategory.COMMA) {
					Expect(TokenCategory.COMMA);
					nArrayList.Add(Lit());
				}
			}
			Expect(TokenCategory.CURLY_BRACE_RIGHT);
			return nArrayList;
		}
	}
}
