.assembly 'output' { }

.assembly extern 'int64lib' {}

.assembly extern 'int64lib' { }
.class public 'Test' extends ['mscorlib']'System'.'Object' {
  .method public static void 'whatever'() {
  .entrypoint
.locals init (
          )
        .method public static
          default int64 'main'()
        {
        .locals init (
          )
           ldc.i4 "Hello World\n"
          conv.i8
          call int64 class ['int64lib']'Utils'.'Runtime'::'prints'(int64)
          }
          call void class ['mscorlib']'System'.'Console'::'WriteLine'(int32)
    ret
  }
}