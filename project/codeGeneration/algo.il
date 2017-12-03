.assembly 'output' { }
.assembly extern 'int64lib' { }
.class public 'Test' extends ['mscorlib']'System'.'Object' {
  .method public static void 'whatever'() {
  .entrypoint
.locals init (
           int64 'testeando',
          )
        .method public static
          default int64 'max_array'(a, z)
        {
        .locals init (
           int64 'max',
           int64 'x',
          )
        }
          .method public static
          default int64 'main'()
        {
        .locals init (
           int64 'array',
           int64 'sum',
           int64 'max',
          )
        }
          call void class ['mscorlib']'System'.'Console'::'WriteLine'(int32)
    ret
  }
}