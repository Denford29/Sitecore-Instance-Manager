namespace SIM
{
  using System;
  using JetBrains.Annotations;

  public abstract class ConsoleApp : App
  {
    protected ConsoleApp()
    {
    }

    protected override void WriteOutput(string json)
    {
      Console.WriteLine(json);
    }
  }
}