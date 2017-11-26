.assembly extern mscorlib
{
  .ver 4:0:0:0
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 ) // .z\V.4..
}
.assembly 'funcionamela'
{
  .custom instance void class [mscorlib]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute::'.ctor'() =  (
		01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78   // ....T..WrapNonEx
		63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01       ) // ceptionThrows.

  .hash algorithm 0x00008004
  .ver  0:0:0:0
}
.module funcionamela.exe // GUID = {B895BB3E-8316-40A1-B804-1C9595DBE3B7}


  .class public auto ansi beforefieldinit Test
  	extends [mscorlib]System.Object
  {
    .field  public static  int32 miglob
    .field  public static  string miglob2

    // method line 1
    .method public hidebysig specialname rtspecialname 
           instance default void '.ctor' ()  cil managed 
    {
        // Method begins at RVA 0x2050
	// Code size 7 (0x7)
	.maxstack 8
	IL_0000:  ldarg.0 
	IL_0001:  call instance void object::'.ctor'()
	IL_0006:  ret 
    } // end of method Test::.ctor

    // method line 2
    .method public static hidebysig 
           default void pito (int32 x)  cil managed 
    {
        // Method begins at RVA 0x2058
	// Code size 29 (0x1d)
	.maxstack 2
	.locals init (
		int32	V_0)
	IL_0000:  ldc.i4.0 
	IL_0001:  stloc.0 
	IL_0002:  ldarg.0 
	IL_0003:  ldc.i4.1 
	IL_0004:  add 
	IL_0005:  starg.s 0
	IL_0007:  ldstr "algo {0}"
	IL_000c:  ldarg.0 
	IL_000d:  box [mscorlib]System.Int32
	IL_0012:  call string string::Format(string, object)
	IL_0017:  stsfld string Test::miglob2
	IL_001c:  ret 
    } // end of method Test::pito

    // method line 3
    .method public static hidebysig 
           default void Main ()  cil managed 
    {
        // Method begins at RVA 0x2084
	.entrypoint
	// Code size 9 (0x9)
	.maxstack 1
	.locals init (
		int32	V_0)
	IL_0000:  ldc.i4.0 
	IL_0001:  stloc.0 
	IL_0002:  ldc.i4.7 
	IL_0003:  call void class Test::pito(int32)
	IL_0008:  ret 
    } // end of method Test::Main

  } // end of class Test

