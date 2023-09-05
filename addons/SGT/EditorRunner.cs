using Godot;
using SGT;

public partial class EditorRunner : Control
{
  public override void _Ready()
  {
    GetNode<GodotTestRoot>("/root/SGT").RunAllTests();
  }
}