using Godot;

namespace SGT;


public class Config // TODO make this being loaded from file
{
  public static string testResultsPath = ProjectSettings.GlobalizePath("res://addons/SGT/Temp/TestResults.xml");
  public static long testTimeoutTimeMs = 60_000;
}