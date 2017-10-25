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

		public Token CurrentToken2 {
		        get { return tokenStream.Current; }
		}

		public Token Expect(TokenCategory category) {;
			if (CurrentToken == category) {
				Token current = tokenStream.Current;
				tokenStream.MoveNext();
				return current;
			} else {
				throw new SyntaxError(category, tokenStream.Current);
			}
		}

		// new start point
		public Node CProgram() {
			Program nProgram = (Program)ProgramRecursive();
			Expect(TokenCategory.EOF);
			return nProgram;
		}

		// old start point (return "root" node for tree building)
		public Node ProgramRecursive() {
			Program nProgram = new Program();
			VarList nVarDefList = new VarList();
			FunList nFunDefList = new FunList();
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

		public void Vareamela(VarList nVarDefList) {
			VariableDeclaration nVarDef = new VariableDeclaration();
			Expect(TokenCategory.VAR);
			nVarDef.AnchorToken = Expect(TokenCategory.IDENTIFIER);
			nVarDefList.Add(nVarDef);
			while (CurrentToken==TokenCategory.COMMA) {
				Expect(TokenCategory.COMMA);
				VariableDeclaration otherNVarDef = new VariableDeclaration();
				otherNVarDef.AnchorToken = Expect(TokenCategory.IDENTIFIER);
				nVarDefList.Add(otherNVarDef);
			}
			Expect(TokenCategory.SEMICOLON);
		}

		public void Funcionamela(FunList nFunDefList) {
			FunctionDefinition nFunDef = new FunctionDefinition(){
				AnchorToken = Expect(TokenCategory.IDENTIFIER)
			};
			ParamList nParameterList = new ParamList();
			Expect(TokenCategory.PARENTHESIS_LEFT);
			if (CurrentToken==TokenCategory.IDENTIFIER) {
				nParameterList.Add(new Param(){
					AnchorToken = Expect(TokenCategory.IDENTIFIER)
				});
				while (CurrentToken==TokenCategory.COMMA) {
					Expect(TokenCategory.COMMA);
					nParameterList.Add(new Param(){
						AnchorToken = Expect(TokenCategory.IDENTIFIER)
					});
				}
			}
			Expect(TokenCategory.PARENTHESIS_RIGHT);
			Expect(TokenCategory.CURLY_BRACE_LEFT);
			VarList nVarDefList = (VarList)DefinitionContinuer();
			StatementList nStmtList = (StatementList)StatmentList();
			Expect(TokenCategory.CURLY_BRACE_RIGHT);
			nFunDef.Add(nParameterList);
			nFunDef.Add(nVarDefList);
			nFunDef.Add(nStmtList);
			nFunDefList.Add(nFunDef);
		}

		public Node DefinitionContinuer() {
			VarList nVarDefList = new VarList();
			while (CurrentToken == TokenCategory.VAR) {
				Vareamela(nVarDefList);
			}
			return nVarDefList;
		}

		public Node StatmentList() {
			StatementList nStmtList = new StatementList();
			while (firstOfStatement.Contains(CurrentToken)) {
				Statment(nStmtList);
			}
			return nStmtList;
		}

		public void Statment(StatementList nStmtList) {
			switch(CurrentToken) {
				case TokenCategory.IDENTIFIER: {
					Token tokenForAnchor = Expect(TokenCategory.IDENTIFIER);
					if (CurrentToken==TokenCategory.ASSIGN) {
						Assign nAssign = new Assign(){
							AnchorToken = tokenForAnchor
						};
						nAssign.Add(Assign());
						nStmtList.Add(nAssign);
					}
					else if (CurrentToken==TokenCategory.PARENTHESIS_LEFT) {
						FCall nFunCall = new FCall(){
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
					nStmtList.Add(new BreakNode() {
						AnchorToken = Expect(TokenCategory.BREAK)
					});
					Expect(TokenCategory.SEMICOLON);
					break;
				}
				case TokenCategory.CONTINUE: {
					nStmtList.Add(new ContinueNode() {
						AnchorToken = Expect(TokenCategory.CONTINUE)
					});
					Expect(TokenCategory.SEMICOLON);
					break;
				}
				case TokenCategory.RETURN: {
					Expect(TokenCategory.RETURN);
					nStmtList.Add(new ReturnNode(){
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

		public Node Assign() {
			Expect(TokenCategory.ASSIGN);
			ExprNode nExpr = (ExprNode)Expr();
			Expect(TokenCategory.SEMICOLON);
			return nExpr;
		}

		public Node FunCall() {
			ExprList nExprList = new ExprList();
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

		public Node IfStmt() {
			IfNode nIfStmt = new IfNode();
			Expect(TokenCategory.PARENTHESIS_LEFT);
			nIfStmt.Add(Expr());
			Expect(TokenCategory.PARENTHESIS_RIGHT);
			Expect(TokenCategory.CURLY_BRACE_LEFT);
			StatementList nStmtList = new StatementList();
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
					StatementList otherNStmtList = new StatementList();
					while (firstOfStatement.Contains(CurrentToken)) {
						Statment(otherNStmtList);
					}
					nIfStmt.Add(otherNStmtList);
					Expect(TokenCategory.CURLY_BRACE_RIGHT);
				}
				else if (CurrentToken==TokenCategory.CURLY_BRACE_LEFT) {
					Expect(TokenCategory.CURLY_BRACE_LEFT);
					StatementList otherNStmtList = new StatementList();
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

		public Node SwitchStmt() {
			SwitchNode nSwitchStmt = new SwitchNode();
			Expect(TokenCategory.SWITCH);
			Expect(TokenCategory.PARENTHESIS_LEFT);
			nSwitchStmt.Add(Expr());
			Expect(TokenCategory.PARENTHESIS_RIGHT);
			Expect(TokenCategory.CURLY_BRACE_LEFT);
			CaseList nCaseList = new CaseList();
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

		public Node Case() {
			Case nCase = new Case();
			Expect(TokenCategory.CASE);
			nCase.Add(Lit());
			while (CurrentToken==TokenCategory.COMMA) {
				Expect(TokenCategory.COMMA);
				nCase.Add(Lit());
			}
			Expect(TokenCategory.COLON);
			StatementList nStmtList = new StatementList();
			while (firstOfStatement.Contains(CurrentToken)) {
				Statment(nStmtList);
			}
			nCase.Add(nStmtList);
			return nCase;
		}

		public Node Lit() {
			switch(CurrentToken) {
				case TokenCategory.TRUE: {
					return new BoolNode(){
						AnchorToken = Expect(TokenCategory.TRUE)
					};
				}
				case TokenCategory.FALSE: {
					return new BoolNode(){
						AnchorToken = Expect(TokenCategory.FALSE)
					};
				}
				case TokenCategory.BASE_2: {
					return new IntNode(){
						AnchorToken = Expect(TokenCategory.BASE_2)
					};
				}
				case TokenCategory.BASE_8: {
					return new IntNode(){
						AnchorToken = Expect(TokenCategory.BASE_8)
					};
				}
				case TokenCategory.BASE_10: {
					return new IntNode(){
						AnchorToken = Expect(TokenCategory.BASE_10)
					};
				}
				case TokenCategory.BASE_16: {
					return new IntNode(){
						AnchorToken = Expect(TokenCategory.BASE_16)
					};
				}
				case TokenCategory.CHARACTER: {
					return new CharNode(){
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

		public Node Default() {
			StatementList nStmtList = new StatementList();
			Expect(TokenCategory.DEFAULT);
			Expect(TokenCategory.COLON);
			while (firstOfStatement.Contains(CurrentToken)) {
				Statment(nStmtList);
			}
			return nStmtList;
		}

		public Node WhileStmt() {
			WhileNode nWhileStmt = new WhileNode();
			Expect(TokenCategory.WHILE);
			Expect(TokenCategory.PARENTHESIS_LEFT);
			nWhileStmt.Add(Expr());
			Expect(TokenCategory.PARENTHESIS_RIGHT);
			Expect(TokenCategory.CURLY_BRACE_LEFT);
			StatementList nStmtList = new StatementList();
			while (firstOfStatement.Contains(CurrentToken)) {
				Statment(nStmtList);
			}
			nWhileStmt.Add(nStmtList);
			Expect(TokenCategory.CURLY_BRACE_RIGHT);
			return nWhileStmt;
		}

		public Node DoWhileStmt() {
			DoWhileNode nDoWhileStmt = new DoWhileNode();
			Expect(TokenCategory.DO);
			Expect(TokenCategory.CURLY_BRACE_LEFT);
			StatementList nStmtList = new StatementList();
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

		public Node ForStmt() {
			Expect(TokenCategory.FOR);
			Expect(TokenCategory.PARENTHESIS_LEFT);
			ForNode nForStmt = new ForNode() {
				AnchorToken = Expect(TokenCategory.IDENTIFIER)
			};
			Expect(TokenCategory.IN);
			nForStmt.Add(Expr());
			Expect(TokenCategory.PARENTHESIS_RIGHT);
			Expect(TokenCategory.CURLY_BRACE_LEFT);
			StatementList nStmtList = new StatementList();
			while (firstOfStatement.Contains(CurrentToken)) {
				Statment(nStmtList);
			}
			nForStmt.Add(nStmtList);
			Expect(TokenCategory.CURLY_BRACE_RIGHT);
			return nForStmt;
		}

		public Node Expr() {
			ExprNode nExpr = new ExprNode();
			nExpr.Add(ExprOr());
			if (CurrentToken==TokenCategory.QUESTION_MARK) {
				Expect(TokenCategory.QUESTION_MARK);
				nExpr.Add(Expr());
				Expect(TokenCategory.COLON);
				nExpr.Add(Expr());
			}
			return nExpr;
		}

		public Node ExprOr() {
			OrNode nExprOr = new OrNode();
			nExprOr.Add(ExprAnd());
			while (CurrentToken==TokenCategory.OR) {
				Expect(TokenCategory.OR);
				nExprOr.Add(ExprAnd());
			}
			return nExprOr;
		}

		public Node ExprAnd() {
			AndNode nExprAnd = new AndNode();
			nExprAnd.Add(ExprComp());
			while (CurrentToken==TokenCategory.AND) {
				Expect(TokenCategory.AND);
				nExprAnd.Add(ExprComp());
			}
			return nExprAnd;
		}

		public Node ExprComp() {
			CompNode nExprComp = new CompNode();
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
				}
			}
			return nExprComp;
		}

		public Node ExprRel() {
			RelNode nExprRel = new RelNode();
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
				}
			}
			return nExprRel;
		}

		public Node ExprBitOr() {
			BinOrNode nExprBitOr = new BinOrNode();
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
				}
			}
			return nExprBitOr;
		}

		public Node ExprBitAnd() {
			BinAndNode nExprBitAnd = new BinAndNode();
			nExprBitAnd.Add(ExprBitShift());
			while (CurrentToken==TokenCategory.BIT_AND) {
				switch(CurrentToken) {
					case TokenCategory.BIT_AND: {
						Expect(TokenCategory.BIT_AND);
						nExprBitAnd.Add(ExprBitAnd());
						break;
					}
				}
			}
			return nExprBitAnd;
		}

		public Node ExprBitShift() {
			BinShiftNode nExprBitShift = new BinShiftNode();
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
				}
			}
			return nExprBitShift;
		}

		public Node ExprAdd() {
			PlusNode nExprAdd = new PlusNode();
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
				}
			}
			return nExprAdd;
		}

		public Node ExprMul() {
			MulNode nExprMul = new MulNode();
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
				}
			}
			return nExprMul;
		}

		public Node ExprPow() {
			PowerNode nExprPow = new PowerNode();
			nExprPow.Add(ExprUnary());
			if (CurrentToken == TokenCategory.POWER) {
				Expect(TokenCategory.POWER);
				nExprPow.Add(ExprPow());
			}
			return nExprPow;
		}

		public Node ExprUnary() {
			UnaryNode nExprUnary = new UnaryNode();
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
				}
			}
			else if (firstOfExprPrimary.Contains(CurrentToken)) {
				nExprUnary.Add(ExprPrimary());
			}
			return nExprUnary;
		}

		public Node ExprPrimary() {
			PrimaryExpNode nExprPrimary = new PrimaryExpNode();
			if (CurrentToken == TokenCategory.IDENTIFIER) {
				Token identifier = Expect(TokenCategory.IDENTIFIER);
				if (CurrentToken==TokenCategory.PARENTHESIS_LEFT) {
					FCall nFunCall = new FCall(){
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

		public Node LitAlt() {
			if (firstOfLit.Contains(CurrentToken)) {
				return Lit();
			}
			else if (CurrentToken==TokenCategory.STRING) {
				StringNode nLitString = new StringNode(){
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

		public Node ArrayList() {
			ArrayList nArrayList = new ArrayList();
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
