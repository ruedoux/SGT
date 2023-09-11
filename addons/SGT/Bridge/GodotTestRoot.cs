namespace SGT;
using System.Threading.Tasks;
using Godot;

public partial class GodotTestRoot : Control
{

  private Task awaitedTests;
  private bool waitingForAwaitedTests = false;
  internal Runner runner;
  public Logger logger = new();


  public GodotTestRoot()
  {
    logger.allLogObservers.AddObservers(GD.Print);
  }

  public void RunTestsInNamespaces(string[] namespaces)
  {
    runner ??= new(this);
    awaitedTests = Task.Run(() => runner.RunTestsInNamespaces(namespaces));
    waitingForAwaitedTests = true;
  }

  public void DeleteAllChildren()
  {
    foreach (Node child in GetChildren())
    {
      child.QueueFree();
    }
  }
}
