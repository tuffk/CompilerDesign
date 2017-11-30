.assembly 'boolang' {}

.class public 'FinalExam' extends ['mscorlib']'System'.'Object' {
	.method public static void 'start'() {
		.entrypoint
		ldc.i4.1
		ldc.i4.1
		xor
		ldc.i4.0
		ldc.i4.0
		ldc.i4.0
		ldc.i4.1
		xor
		or
		and
		or
		call void ['mscorlib']'System'.'Console'::'WriteLine'(int32)
		ret
	}
}
