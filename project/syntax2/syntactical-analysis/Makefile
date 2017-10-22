MCS=mcs

all: source/Driver.cs source/Parser.cs source/Scanner.cs source/SyntaxError.cs source/Token.cs source/TokenCategory.cs
	$(MCS) -out:int64.exe source/*.cs

buttercup.exe: Driver.cs Scanner.cs Token.cs TokenCategory.cs Parser.cs \
	SyntaxError.cs
	mcs -out:int64.exe Driver.cs Scanner.cs Token.cs TokenCategory.cs \
	Parser.cs SyntaxError.cs

test:
	mono int64.exe sample-inputs/palindrome.int64

clean:
	rm int64.exe
