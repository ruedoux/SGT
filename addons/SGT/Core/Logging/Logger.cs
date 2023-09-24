namespace SGT;

internal class Logger
{
  public ObserverManager<Message> messageLogObservers = new();
  public MessageAgregator messageAgregator = new();

  public void Log(Message message)
  {
    messageLogObservers.NotifyObservers(message);
    messageAgregator.AddMessage(message);
  }
}
