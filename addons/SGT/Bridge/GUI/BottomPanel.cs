#if TOOLS
namespace SGT;

using System.Threading.Tasks;

using Godot;

[Tool]
internal partial class BottomPanel : Control
{
  private EditorInterface editorInterface;
  private RichTextLabel logBoard;
  private Button startAllbutton;

  public void SetInterface(EditorInterface editorInterface)
  {
    this.editorInterface = editorInterface;
  }

  public override void _Ready()
  {
    startAllbutton = GetNode<Button>("ScrollContainer/HBoxContainer/VBoxContainer/StartAll");
    logBoard = GetNode<RichTextLabel>("ScrollContainer/HBoxContainer/LogBoard");
    startAllbutton.Connect(
      Button.SignalName.Pressed,
      new Callable(this, "RunAllTests"));
  }

  public void RunAllTests()
  {
    logBoard.Clear();
    editorInterface.PlayCustomScene("res://addons/SGT/Bridge/GodotTestRoot.tscn");
    GD.Print("Yap");

  }

  public void UpdateLog(string log)
  {
    logBoard.AppendText(log);
  }
}
#endif