using Godot;

public partial class TestNode : Node2D
{
  public int shouldBe10;

  public int Returns5()
  {
    return 5;
  }

  public override void _Ready()
  {
    shouldBe10 = 10;
  }
}
