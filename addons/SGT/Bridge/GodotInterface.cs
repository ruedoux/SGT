namespace SGT;
using Godot;

public partial class GodotInterface : Node
{
  public Runner GetRunner()
  {
    return new Runner(this);
  }

  public void AddLog(string msgs, Logger.MESSAGE_TYPE messageType)
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
}
