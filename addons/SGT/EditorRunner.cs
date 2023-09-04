using System.Threading.Tasks;
using Godot;
using SGT;

public partial class EditorRunner : Control
{
  public override void _Ready()
  {
    Runner runner = GetNode<GodotInterface>("/root/SGT").GetRunner();
    Task.Run(() => runner.RunAllTests());
  }
}