namespace SIM.Adapters
{
  using System;
  using System.Net.Http;
  using JetBrains.Annotations;

  public class WebServerAdapterException : Exception
  {
    [CanBeNull]
    public object Data { get; }

    protected WebServerAdapterException([NotNull] string message)
      : base($"Failed to perform an operation with IIS. {message}")
    {
    }

    public WebServerAdapterException([NotNull] Exception ex)
      : base("Failed to perform an operation with IIS", ex)
    {
    }

    public WebServerAdapterException([NotNull] object data)
    {
      Data = data;
    }
  }
}