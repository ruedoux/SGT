namespace SGT;
using System.Threading.Tasks;
using Godot;

public partial class GodotTestRoot : Control
{
  public Logger logger = new();

  public GodotTestRoot()
  {
    logger.allLogObservers.AddObservers(GD.Print);
  }

  public void RunTestsInNamespaces(string[] namespaces)
  {
    Runner runner = new(this, logger, namespaces);
    Task.Run(() => runner.Run());
  }

  public void DeleteAllChildren()
  {
    foreach (Node child in GetChildren())
    {
      child.QueueFree();
    }
  }
}
