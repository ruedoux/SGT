#if TOOLS
namespace SGT;
using Godot;

[Tool]
public partial class Plugin : EditorPlugin
{
  private const string SINGLETON_NAME = "SGT";
  private const string SINGLETON_PATH = "res://addons/SGT/Bridge/GodotInterface.cs";

  public override void _EnterTree()
  {
    AddAutoloadSingleton(SINGLETON_NAME, SINGLETON_PATH);
    GD.Print("Loaded SGT plugin.");
  }

  public override void _ExitTree()
  {
    RemoveAutoloadSingleton(SINGLETON_NAME);
    GD.Print("Unloaded SGT plugin.");
  }
}
#endif