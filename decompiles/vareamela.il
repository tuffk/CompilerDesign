.assembly extern mscorlib
{
  .ver 4:0:0:0
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 ) // .z\V.4..
}
.assembly 'vareamela'
{
  .custom instance void class [mscorlib]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute::'.ctor'() =  (
		01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78   // ....T..WrapNonEx
		63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01       ) // ceptionThrows.

  .hash algorithm 0x00008004
  .ver  0:0:0:0
}
.module vareamela.exe // GUID = {DD8F27A5-EBDB-4238-B163-C4D6CC8A5B40}


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
           default void Main ()  cil managed 
    {
        // Method begins at RVA 0x2058
	.entrypoint
	// Code size 9 (0x9)
	.maxstack 2
	.locals init (
		int32	V_0,
		int32	V_1,
		int32	V_2)
	IL_0000:  ldc.i4.2 
	IL_0001:  stloc.0 
	IL_0002:  ldc.i4.2 
	IL_0003:  stloc.1 
	IL_0004:  ldloc.1 
	IL_0005:  ldloc.0 
	IL_0006:  add 
	IL_0007:  stloc.2 
	IL_0008:  ret 
    } // end of method Test::Main

  } // end of class Test

