using System;
using System.Collections.Generic;

namespace SGT;

public class Logger
{

  public ObserverManager<string> normalLogObservers = new();
  public ObserverManager<string> warningLogObservers = new();
  public ObserverManager<string> errorLogObservers = new();
  public ObserverManager<string> allLogObservers = new();
  public ObserverManager<Message> messageLogObservers = new();

  public bool supressError = false;
  public bool supressWarning = false;
  public uint indentationTabs = 0;

  public void Log(Message message)
  {
    message.SetIndentation(indentationTabs);

    allLogObservers.NotifyObservers(message.GetWhole());
    messageLogObservers.NotifyObservers(message);

    if (message.GetMessageType() == Message.TYPE.NORMAL)
    {
      normalLogObservers.NotifyObservers(message.GetWhole());
    }
    else if ((message.GetMessageType() == Message.TYPE.WARNING) && !supressWarning)
    {
      warningLogObservers.NotifyObservers(message.GetWhole());
    }
    else if ((message.GetMessageType() == Message.TYPE.ERROR) && !supressError)
    {
      errorLogObservers.NotifyObservers(message.GetWhole());
    }
  }

  public void LogException(Exception ex)
  {
    List<string> parsedExceptions = new()
    {
      ex.InnerException.Message
    };

    string[] lines = ex.InnerException.ToString()
      .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

    foreach (string line in lines)
    {
      parsedExceptions.Add(line);
    }

    Log(Message.GetError("", parsedExceptions.ToArray()));
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
