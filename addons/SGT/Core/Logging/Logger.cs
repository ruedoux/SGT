namespace SGT;

public class Logger
{
  public ObserverManager<Message> messageLogObservers = new();

  public void Log(Message message)
  {
    messageLogObservers.NotifyObservers(message);
  }
}
