.assembly 'Trillian' {}

.class public 'FinalExam' extends ['mscorlib']'System'.'Object' {
	.method public static void 'start'() {
		.entrypoint
		ldc.r8 4
		dup
		add
		call void ['mscorlib']'System'.'Console'::'WriteLine'(int32)
		ret
	}
}
