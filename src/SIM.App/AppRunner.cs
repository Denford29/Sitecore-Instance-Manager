namespace SIM
{
  using System;
  using JetBrains.Annotations;

  public abstract class AppRunner
  {
    public int Start([NotNull] string commandLine, [NotNull] string executableFilePath)
    {
      var app = CreateApp();

      var args = ParseCommandLineArgs(commandLine, executableFilePath);
      var commandName = GetCommandName(args);
      var commandData = GetCommandData(args, commandName);
      
      return app.Start(commandName, commandData);
    }

    [NotNull]
    protected abstract App CreateApp();

    [NotNull]
    internal static string GetCommandData([NotNull] string args, [NotNull] string commandName)
    {
      return args.Substring(Math.Min(args.Length, commandName.Length + 1));
    }

    [NotNull]
    internal static string GetCommandName([NotNull] string args)
    {
      return args.Substring(0, Math.Max(0, args.IndexOf(' ')));
    }

    [NotNull]
    internal static string ParseCommandLineArgs([NotNull] string commandLine, [NotNull] string executableFilePath)
    {
      return commandLine.Substring(Math.Min(commandLine.Length, $"\"{executableFilePath}\" ".Length));
    }
  }
}