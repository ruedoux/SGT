using System.Threading;
using Godot;

public partial class TestNode : Control
{
  public int shouldBe10;

  public int Returns5()
  {
    Thread.Sleep(1000);
    return 5;
  }

  public override void _Ready()
  {
    shouldBe10 = 10;
  }
}
