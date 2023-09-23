namespace SGT;
using Godot;

internal partial class GodotRunner : CanvasLayer
{
  private readonly GodotTestRoot godotTestRoot = new();

  private RichTextLabel output;
  private Button runAllButton;
  private Button runSelectedButton;
  private OptionButton selectNamespace;


  public GodotRunner()
  {
    godotTestRoot.logger.messageLogObservers.AddObservers(
      new MessagePrinter(UpdateLog, true).Print);
  }

  public override void _Ready()
  {
    output = GetNode<RichTextLabel>("Window/OutputPanel/Output");
    runAllButton = GetNode<Button>("Window/ButtonPanel/Margin/HBox/RunAll");
    runSelectedButton = GetNode<Button>("Window/ButtonPanel/Margin/HBox/RunSelected");
    selectNamespace = GetNode<OptionButton>("Window/ButtonPanel/Margin/HBox/SelectNamespace");

    runAllButton.Connect(Button.SignalName.Pressed, new Callable(this, nameof(RunAll)));
    runSelectedButton.Connect(Button.SignalName.Pressed, new Callable(this, nameof(RunSelected)));
    selectNamespace.Connect(Button.SignalName.Pressed, new Callable(this, nameof(UpdateSelectedNamespaces)));

    GetTree().Root.CallDeferred(Node.MethodName.AddChild, godotTestRoot);
    UpdateSelectedNamespaces();
  }

  public void RunAll()
  {
    output.Clear();
    godotTestRoot.RunTestsInNamespaces(AssemblyExtractor.GetAllTestNamespaces().ToArray());
  }

  public void RunSelected()
  {
    output.Clear();
    godotTestRoot.RunTestsInNamespaces(new string[] { selectNamespace.GetItemText(selectNamespace.GetSelectedId()) });
  }

  public void UpdateSelectedNamespaces()
  {
    selectNamespace.Clear();

    foreach (string ns in AssemblyExtractor.GetAllTestNamespaces())
    {
      selectNamespace.AddItem(ns);
    }
  }

  public void UpdateLog(string message)
  {
    output.CallDeferred(RichTextLabel.MethodName.AppendText, message += "\n");
  }
}
