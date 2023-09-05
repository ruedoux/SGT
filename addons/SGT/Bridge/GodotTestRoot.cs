namespace SGT;

using System.Threading.Tasks;
using Godot;

public partial class GodotTestRoot : Node
{
  public void RunAllTests()
  {
    Runner runner = new(this);
    Task.Run(() => runner.RunAllTests());
  }

  public void AddLog(
    string msgs, Logger.MESSAGE_TYPE messageType = Logger.MESSAGE_TYPE.NORMAL)
  {
    GD.Print(msgs);
    if (messageType == Logger.MESSAGE_TYPE.WARNING)
    {
      GD.PushWarning(msgs);
    }
    if (messageType == Logger.MESSAGE_TYPE.ERROR)
    {
      GD.PushWarning(msgs);
    }
  }

  public void DeleteAllChildren()
  {
    foreach (Node child in GetChildren())
    {
      child.QueueFree();
    }
  }
}
