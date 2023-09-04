using Godot;
using SGT;

public partial class EditorRunner : Control
{
  public override void _Ready()
  {
    GetNode<GodotInterface>("/root/SGT").GetRunner().RunAllTests();
  }
}