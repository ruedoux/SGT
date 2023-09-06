namespace SGT;
using System.Threading.Tasks;
using Godot;

public partial class GodotTestRoot : Node
{

  internal Runner runner;
  public Logger logger;


  public GodotTestRoot()
  {
    logger = new(this);
    logger.warningLogObservers.AddObservers(GD.Print, GD.PushWarning);
    logger.normalLogObservers.AddObservers(GD.Print);
    logger.errorLogObservers.AddObservers(GD.Print, GD.PushError);
  }

  public override void _Ready()
  {
    GD.Print("yap");
  }

  public void RunAllTests()
  {
    runner ??= new(this);
    Task.Run(() => runner.RunAllTests());
  }

  public void DeleteAllChildren()
  {
    foreach (Node child in GetChildren())
    {
      child.QueueFree();
    }
  }
}