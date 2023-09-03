namespace SGT;
using Godot;


public static class Logger
{
  private enum MESSAGE_TYPE
  {
    NORMAL,
    WARN,
    ERROR
  }

  private const string ERROR_MARKER = "[ERROR] ";
  private const string WARNING_MARKER = "[WARN] ";
  private const string INFO_MARKER = "[INFO] ";

  public static bool supressError = false;
  public static bool supressWarning = false;
  public static uint indentationTabs = 0;

  public static void Log(params object[] msgs)
  {
    ForwardLog("", MESSAGE_TYPE.NORMAL, msgs);
  }

  public static void LogError(params object[] msgs)
  {
    ForwardLog(ERROR_MARKER, MESSAGE_TYPE.ERROR, msgs);
  }

  public static void LogInfo(params object[] msgs)
  {
    ForwardLog(INFO_MARKER, MESSAGE_TYPE.NORMAL, msgs);
  }

  public static void LogWarning(params object[] msgs)
  {
    ForwardLog(WARNING_MARKER, MESSAGE_TYPE.WARN, msgs);
  }

  public static void AnnounceBlockStart(string announcment)
  {
    Log(announcment);
    IncreaseIndentation();
  }

  public static void AnnounceBlockEnd(string announcment)
  {
    DecreaseIndentation();
    Log(announcment);
  }

  public static void IncreaseIndentation()
  {
    indentationTabs += 1;
  }

  public static void DecreaseIndentation()
  {
    indentationTabs -= 1;
  }

  public static void ResetIndentation()
  {
    indentationTabs = 0;
  }

  private static void ForwardLog(
    string marker, MESSAGE_TYPE messageType, params object[] msgs)
  {
    LogToConsole(marker, msgs);
    if ((messageType == MESSAGE_TYPE.WARN) && !supressWarning)
    {
      GD.PushWarning(msgs);
    }
    if ((messageType == MESSAGE_TYPE.ERROR) && !supressError)
    {
      GD.PushWarning(msgs);
    }
  }

  private static void LogToConsole(string marker, params object[] msgs)
  {
    string output = marker;

    for (int i = 0; i < indentationTabs; i++)
    {
      output += "\t";
    }

    foreach (object msg in msgs)
    {
      output += msg.ToString();
    }

    GD.Print(output);
  }
}
