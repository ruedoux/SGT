using System;
namespace SGT;

public class Logger
{
  public ObserverManager<string> allLogObservers = new();
  public ObserverManager<Message> messageLogObservers = new();

  public uint indentationTabs = 0;

  public void Log(Message message)
  {
    message.SetIndentation(indentationTabs);

    allLogObservers.NotifyObservers(message.GetWhole());
    messageLogObservers.NotifyObservers(message);
  }

  public void LogException(Exception ex)
  {
    string[] lines = ex.InnerException.ToString()
      .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

    Log(Message.GetError("", ex.InnerException.Message));
    foreach (string line in lines)
    {
      Log(Message.GetError("", line));
    }
  }

  public void StartBlock(Message message)
  {
    Log(message);
    indentationTabs += 1;
  }

  public void EndBlock(Message message)
  {
    indentationTabs -= 1;
    Log(message);
  }
}
