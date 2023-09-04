#if TOOLS
namespace SGT;
using Godot;

[Tool]
public partial class Plugin : EditorPlugin
{
  public override void _EnterTree()
  {
    AddAutoloadSingleton("SGT", "res://addons/SGT/Godot/GodotInterface.cs");
    GD.Print("Loaded SGT plugin.");
  }
}
#endif