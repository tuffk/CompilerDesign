.assembly 'Trillian' {}

.class public 'final_exam' extends ['mscorlib']'System'.'Object' {
	.method public static void 'start'() {
		.entrypoint
		ldc.r8 -4.2
		dup
		add
		call void ['mscorlib']'System'.'Console'::'WriteLine'(float64)
		ret
	}
}
