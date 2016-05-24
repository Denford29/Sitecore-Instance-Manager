namespace SIM.Core.Common
{
  using Sitecore.Diagnostics.Base.Annotations;

  public interface ICommand
  {
    [CanBeNull]
    CommandResultBase Execute();
  }
}