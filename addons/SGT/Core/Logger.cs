namespace SGT;


public class Logger
{
  public enum MESSAGE_TYPE
  {
    NORMAL,
    WARNING,
    ERROR
  }

  public ObserverManager<string> normalLogObservers = new();
  public ObserverManager<string> warningLogObservers = new();
  public ObserverManager<string> errorLogObservers = new();

  private readonly GodotTestRoot godotTestRoot;

  private const string ERROR_MARKER = "[ERROR] ";
  private const string WARNING_MARKER = "[WARN] ";
  private const string INFO_MARKER = "[INFO] ";

  public bool supressError = false;
  public bool supressWarning = false;
  public uint indentationTabs = 0;


  internal Logger(GodotTestRoot godotTestRoot)
  {
    this.godotTestRoot = godotTestRoot;
  }

  public void Log(params object[] msgs)
  {
    ForwardLog("", MESSAGE_TYPE.NORMAL, msgs);
  }

  public void LogArray(string[] lines)
  {
    foreach (string line in lines)
    {
      Log(line);
    }
  }

  public void LogError(params object[] msgs)
  {
    ForwardLog(ERROR_MARKER, MESSAGE_TYPE.ERROR, msgs);
  }

  public void LogInfo(params object[] msgs)
  {
    ForwardLog(INFO_MARKER, MESSAGE_TYPE.NORMAL, msgs);
  }

  public void LogWarning(params object[] msgs)
  {
    ForwardLog(WARNING_MARKER, MESSAGE_TYPE.WARNING, msgs);
  }

  public void AnnounceBlockStart(string announcment)
  {
    Log(announcment);
    indentationTabs += 1;
  }

  public void AnnounceBlockEnd(string announcment)
  {
    indentationTabs -= 1;
    Log(announcment);
  }

  private void ForwardLog(
    string marker, MESSAGE_TYPE messageType, params object[] msgs)
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

    if (supressError && messageType == MESSAGE_TYPE.ERROR)
    {
      messageType = MESSAGE_TYPE.NORMAL;
    }
    if (supressWarning && messageType == MESSAGE_TYPE.WARNING)
    {
      messageType = MESSAGE_TYPE.NORMAL;
    }

    if (messageType == Logger.MESSAGE_TYPE.NORMAL)
    {
      normalLogObservers.NotifyObservers(output);
    }
    if (messageType == Logger.MESSAGE_TYPE.WARNING)
    {
      warningLogObservers.NotifyObservers(output);
    }
    if (messageType == Logger.MESSAGE_TYPE.ERROR)
    {
      errorLogObservers.NotifyObservers(output);
    }
  }
}
