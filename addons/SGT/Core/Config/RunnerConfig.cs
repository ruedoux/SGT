using System;
namespace SGT;

[Serializable]
public class RunnerConfig
{
  public string[] namespaces;

  private RunnerConfig() { }

  public RunnerConfig(string[] namespaces)
  {
    this.namespaces = namespaces;
  }
}