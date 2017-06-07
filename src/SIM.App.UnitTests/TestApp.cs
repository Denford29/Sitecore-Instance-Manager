namespace SIM.UnitTests
{
  using System;
  using System.Collections.Generic;
  using SIM.Commands;

  public class TestApp : App
  {
    public TestApp()
    {
    }

    /// <inheritdoc />
    protected internal override IReadOnlyDictionary<Type, string[]> Verbs { get; } =
      new Dictionary<Type, string[]>
      {
        { typeof(HelpCommand), new[] { "help", "help_descr" } }
      };

    /// <inheritdoc />
    public override string Information { get; } = "info";

    /// <inheritdoc />
    public override string ExecutableName { get; } = "sim_exe";
  }
}