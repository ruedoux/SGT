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
    logger.warningLogObservers.AddObservers(GD.PushWarning);
    logger.errorLogObservers.AddObservers(GD.PushError);
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

  public override void _PhysicsProcess(double delta)
  {
    // Kinda clunky way to await for any throws that could happen
    // Probably there is a better way to do this?
    if (awaitedTests != null && waitingForAwaitedTests)
    {
      waitingForAwaitedTests = false;
      awaitedTests.Wait();
      awaitedTests = null;
    }
  }
}
