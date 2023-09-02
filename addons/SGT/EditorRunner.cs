using Godot;
using SGT;
using System;

public partial class EditorRunner : Control
{
  public override void _Ready()
  {
    Runner.RunAllTests();
  }
}