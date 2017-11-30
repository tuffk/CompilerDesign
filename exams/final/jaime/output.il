.assembly 'Trillian' {}

.class public 'final_exam' extends ['mscorlib']'System'.'Object' {
	.method public static void 'start'() {
		.entrypoint
		ldc.r8 4
		ldc.r8 -7.8
		call float64 ['mscorlib']'System'.'Math'::'Max'(float64, float64)
		call void ['mscorlib']'System'.'Console'::'WriteLine'(float64)
		ret
	}
}
