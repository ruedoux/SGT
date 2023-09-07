namespace SGT;
using Godot;

internal class RunnerConfig
{
  public static string savePath = "res://addons/SGT/Data/RunnerConfig.json";
  public string[] namespacesToRun;

  public RunnerConfig() { }

  public RunnerConfig(string[] namespacesToRun)
  {
    this.namespacesToRun = namespacesToRun;
  }

  public void SaveToFile()
  {
    if (FileAccess.FileExists(savePath))
    {
      DirAccess.RemoveAbsolute(savePath);
    }

    var file = FileAccess.Open(savePath, FileAccess.ModeFlags.Write);
  }

  public static RunnerConfig LoadFromFile()
  {
    if (!FileAccess.FileExists(savePath))
    {
      return new RunnerConfig();
    }

    var file = FileAccess.Open(savePath, FileAccess.ModeFlags.Write);
    RunnerConfig runnerConfig = new();

    return runnerConfig;
  }
}