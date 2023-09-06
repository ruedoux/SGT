using System.Collections.Generic;

public class ObserverManager<T>
{
  public delegate void functionDelegate(T arg);
  private readonly List<functionDelegate> _observers = new();

  public void AddObservers(params functionDelegate[] observers)
  {
    _observers.AddRange(observers);
  }

  public void RemoveObserver(functionDelegate observer)
  {
    _observers.Remove(observer);
  }

  public void NotifyObservers(T arg)
  {
    foreach (var observer in _observers)
    {
      observer(arg);
    }
  }
}