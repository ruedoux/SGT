#if TOOLS
namespace SGT;

using System;
using System.Diagnostics;
using System.IO;
using Godot;


[Tool]
internal partial class BottomPanel : Control
{
  private EditorInterface editorInterface;
  private RichTextLabel logBoard;
  private Button runAllbutton;
  private Button runSelected;
  private OptionButton namespaceSelectButton;
  private bool tryLoadTestLogs = false;

  public void SetInterface(EditorInterface editorInterface)
  {
    this.editorInterface = editorInterface;
  }

  public override void _Ready()
  {
    runAllbutton = GetNode<Button>(
      "Console/RunContainer/RunAllMargin/RunAll");
    runSelected = GetNode<Button>(
      "Console/RunContainer/RunSelected");
    logBoard = GetNode<RichTextLabel>(
      "Console/Panel/Output");
    namespaceSelectButton = GetNode<OptionButton>(
      "Console/RunContainer/NamespaceSelect");

    runAllbutton.Connect(
      Button.SignalName.Pressed,
      new Callable(this, nameof(RunAllTests)));
    runSelected.Connect(
      Button.SignalName.Pressed,
      new Callable(this, nameof(RunSelectedTests)));
    namespaceSelectButton.Connect(
      OptionButton.SignalName.Pressed,
      new Callable(this, nameof(UpdateNamespaces)));
    namespaceSelectButton.Connect(
      Control.SignalName.VisibilityChanged,
      new Callable(this, nameof(LoadTestResults)));
    UpdateNamespaces();
  }

  public void RunAllTests()
  {
    PlayTestScene(AssemblyExtractor.GetAllTestNamespaces().ToArray());
  }

  public void RunSelectedTests()
  {
    PlayTestScene(new string[] { GetSelectedNamespace() });
  }

  private void PlayTestScene(string[] namespaces)
  {
    logBoard.Clear();
    UpdateLog("Running...");

    RunnerConfig runnerConfig = new(namespaces);
    try
    {
      File.Delete(Config.testResultsPath);
      ObjectSerializer.SaveToFile(Config.runnerConfigPath, runnerConfig);
      editorInterface.PlayCustomScene(
        "res://addons/SGT/Bridge/GUI/GodotRunner.tscn");

      tryLoadTestLogs = true;
    }
    catch (Exception ex)
    {
      throw new TestSetupException("Failed to save runner config file!", ex);
    }
  }

  private void LoadTestResults()
  {
    if (!File.Exists(Config.testResultsPath) || !tryLoadTestLogs)
      return;

    logBoard.Clear();
    var messageAggregator = ObjectSerializer.LoadFromFile<MessageAgregator>(
      Config.testResultsPath);

    MessagePrinter messagePrinter = new(UpdateLog, true);
    foreach (var message in messageAggregator.messages)
    {
      messagePrinter.Print(message);
    }

    tryLoadTestLogs = false;
  }

  private string GetSelectedNamespace()
  {
    return namespaceSelectButton.GetItemText(namespaceSelectButton.GetSelectedId());
  }

  private void UpdateNamespaces()
  {
    var namespaces = AssemblyExtractor.GetAllTestNamespaces();
    namespaceSelectButton.Clear();

    foreach (var namespaceName in namespaces)
    {
      namespaceSelectButton.AddItem(namespaceName);
    }
  }

  private void UpdateLog(string message)
  {
    logBoard.AppendText(message += "\n");
  }
}
#endif