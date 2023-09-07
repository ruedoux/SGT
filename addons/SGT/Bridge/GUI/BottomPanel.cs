#if TOOLS
namespace SGT;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Godot;

[Tool]
internal partial class BottomPanel : Control
{
  private EditorInterface editorInterface;
  private RichTextLabel logBoard;
  private Button runAllbutton;
  private Button runSelected;
  private OptionButton namespaceSelectButton;

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
      "Console/LogContainer/LogBoard");
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
  }

  public void RunAllTests()
  {
    logBoard.Clear();
    PlayTestScene(AssemblyExtractor.GetAllTestNamespaces().ToArray());
  }

  public void RunSelectedTests()
  {
    logBoard.Clear();
    string selectedNamespace = GetSelectedNamespace();
    var allNamespaces = AssemblyExtractor.GetAllTestNamespaces();
    if (allNamespaces.Contains(selectedNamespace))
    {
      PlayTestScene(new string[] { selectedNamespace });
    }
  }

  public void UpdateLog(string log)
  {
    logBoard.AppendText(log);
  }

  private void PlayTestScene(string[] namespaces)
  {
    UpdateLog("Running...");

    RunnerConfig runnerConfig = new(namespaces);
    runnerConfig.SaveToFile();

    editorInterface.PlayCustomScene("res://addons/SGT/Bridge/GUI/GodotRunner.tscn");
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
}
#endif