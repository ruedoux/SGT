namespace SGT;
using System.Threading.Tasks;
using Godot;

public partial class GodotTestRoot : Control
{

  internal Runner runner;
  public Logger logger = new();


  public GodotTestRoot()
  {
    logger.allLogObservers.AddObservers(GD.Print);
  }

  public void RunTestsInNamespaces(string[] namespaces)
  {
    runner ??= new(this);
    Task.Run(() => runner.RunTestsInNamespaces(namespaces));
  }

  public void DeleteAllChildren()
  {
    foreach (Node child in GetChildren())
    {
      child.QueueFree();
    }
  }
}
