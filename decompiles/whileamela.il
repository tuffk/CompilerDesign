.assembly extern mscorlib
{
  .ver 4:0:0:0
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 ) // .z\V.4..
}
.assembly 'whileamela'
{
  .custom instance void class [mscorlib]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute::'.ctor'() =  (
		01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78   // ....T..WrapNonEx
		63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01       ) // ceptionThrows.

  .hash algorithm 0x00008004
  .ver  0:0:0:0
}
.module whileamela.exe // GUID = {BCABA565-EA2B-4C1B-A470-0C5189D754EE}


  .class public auto ansi beforefieldinit Test
  	extends [mscorlib]System.Object
  {

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
	// Code size 20 (0x14)
	.maxstack 2
	.locals init (
		int32	V_0)
	IL_0000:  ldc.i4.0 
	IL_0001:  stloc.0 
	IL_0002:  br IL_000b

	IL_0007:  ldloc.0 
	IL_0008:  ldc.i4.1 
	IL_0009:  add 
	IL_000a:  stloc.0 
	IL_000b:  ldloc.0 
	IL_000c:  ldc.i4.s 0x0a
	IL_000e:  blt IL_0007

	IL_0013:  ret 
    } // end of method Test::Main

  } // end of class Test

