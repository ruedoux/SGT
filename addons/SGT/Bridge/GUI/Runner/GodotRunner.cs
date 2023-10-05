namespace SGT;
using Godot;

internal partial class GodotRunner : CanvasLayer
{
  private GodotTestRoot godotTestRoot = new();

  private RichTextLabel output;
  private Button runAllButton;
  private Button runSelectedButton;
  private OptionButton selectNamespace;

  public override void _Ready()
  {
    output = GetNode<RichTextLabel>("Window/OutputPanel/Output");
    runAllButton = GetNode<Button>("Window/ButtonPanel/Margin/HBox/RunAll");
    runSelectedButton = GetNode<Button>("Window/ButtonPanel/Margin/HBox/RunSelected");
    selectNamespace = GetNode<OptionButton>("Window/ButtonPanel/Margin/HBox/SelectNamespace");

    runAllButton.Connect(Button.SignalName.Pressed, new Callable(this, nameof(RunAll)));
    runSelectedButton.Connect(Button.SignalName.Pressed, new Callable(this, nameof(RunSelected)));
    selectNamespace.Connect(Button.SignalName.Pressed, new Callable(this, nameof(UpdateSelectedNamespaces)));

    UpdateSelectedNamespaces();
  }

  private void RunAll()
    => RunTests(AssemblyExtractor.GetAllTestNamespaces().ToArray());

  private void RunSelected()
    => RunTests(new string[] { selectNamespace.GetItemText(selectNamespace.GetSelectedId()) });

  private void UpdateSelectedNamespaces()
  {
    selectNamespace.Clear();

    foreach (string ns in AssemblyExtractor.GetAllTestNamespaces())
    {
      selectNamespace.AddItem(ns);
    }
  }

  private void UpdateLog(string message)
    => output.CallDeferred(RichTextLabel.MethodName.AppendText, message += "\n");

  private void RunTests(string[] namespaces)
  {
    if (!godotTestRoot.testsFinished)
      return;

    godotTestRoot.QueueFree();
    output.Clear();
    godotTestRoot = new();
    GetTree().Root.AddChild(godotTestRoot);

    godotTestRoot.logger.messageLogObservers.AddObservers(
      new MessagePrinter(UpdateLog, true).Print);

    godotTestRoot.RunTestsInNamespaces(namespaces);
  }
}
