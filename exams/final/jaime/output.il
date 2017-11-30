.assembly 'Trillian' {}

.class public 'FinalExam' extends ['mscorlib']'System'.'Object' {
	.method public static void 'start'() {
		.entrypoint
		ldc.r8 4
		ldc.r8 4
		call float64 ['mscorlib']'System'.'Math'::'Max'(float64, float64)
		call void ['mscorlib']'System'.'Console'::'WriteLine'(int32)
		ret
	}
}
