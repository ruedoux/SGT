// Will make this class as json serializable the moment this is resolved:
// https://github.com/godotengine/godot/issues/78513
using System;
using System.IO;
using Godot;

public class RunnerConfig
{
  private const string ARR_DELIMETER = ", ";
  public static string savePath = ProjectSettings.GlobalizePath(
    "res://addons/SGT/Data/RunnerConfig.json");

  public string[] namespaces;

  public RunnerConfig() { }

  public void SaveToFile()
  {
    if (File.Exists(savePath))
    {
      File.Delete(savePath);
    }

    File.WriteAllText(savePath, string.Join(ARR_DELIMETER, namespaces));
  }

  public static RunnerConfig LoadFromFile()
  {
    if (!File.Exists(savePath))
    {
      throw new FileNotFoundException("Could not find file: " + savePath);
    }
    string text = File.ReadAllText(savePath);
    string[] namespaces = text.Split(
      new[] { ARR_DELIMETER }, StringSplitOptions.None);

    RunnerConfig runnerConfig = new()
    {
      namespaces = namespaces
    };

    return runnerConfig;
  }
}