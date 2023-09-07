#if TOOLS
namespace SGT;
using Godot;

[Tool]
public partial class SGTPlugin : EditorPlugin
{
  private const string BOTTOM_PANEL_PATH = "res://addons/SGT/Bridge/GUI/BottomPanel.tscn";
  private BottomPanel bottomPanel;


  public override void _EnterTree()
  {
    bottomPanel = GD.Load<PackedScene>(BOTTOM_PANEL_PATH)
      .InstantiateOrNull<BottomPanel>();
    if (bottomPanel != null)
    {
      AddControlToBottomPanel(bottomPanel, "SGT");
      bottomPanel.SetInterface(GetEditorInterface());
      GD.Print("Loaded SGT plugin.");
    }
    else
    {
      GD.PushError("Failed to load SGT plugin.");
    }
  }

  public override void _ExitTree()
  {
    RemoveControlFromBottomPanel(bottomPanel);
    bottomPanel.QueueFree();
    GD.Print("Unloaded SGT plugin.");
  }
}
#endif