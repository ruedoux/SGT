using Godot;

namespace SGT;


public class Config // TODO make this being loaded from file
{
  public static string configPath = ProjectSettings.GlobalizePath("res://addons/SGT/Temp/Config.xml");
  public static string runnerConfigPath = ProjectSettings.GlobalizePath("res://addons/SGT/Temp/RunnerConfig.xml");
  public static string testResultsPath = ProjectSettings.GlobalizePath("res://addons/SGT/Temp/TestResults.xml");
  public static long testTimeoutTimeMs = 60_000;
}