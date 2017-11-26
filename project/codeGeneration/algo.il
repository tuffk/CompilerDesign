.assembly 'output' { }
.assembly extern 'int64lib' { }
.class public 'Test' extends ['mscorlib']'System'.'Object' {
  .method public static void 'whatever'() {
  .entrypoint
call void class ['mscorlib']'System'.'Console'::'WriteLine'(int32)
    ret
  }
}