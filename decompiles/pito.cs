using System;
public class Test
{
  public static int miglob;
  public static string miglob2;

  public static void pito(int x)
  {
    var kuz = 0;
    x++;
    miglob2 = $"algo {x}";
  }
    static public void Main()
    {
        var z = 0;
        pito(7);
        Console.WriteLine($@"hola soy: {z}");
    }
}
