using System;
using System.Collections.Generic;
using Godot;
using SGT;

public partial class GodotRunner : Control
{
  private readonly GodotTestRoot godotTestRoot = new();
  private RichTextLabel output;

  public override void _Ready()
  {
    output = GetNode<RichTextLabel>("Panel/Output");

    godotTestRoot.logger.messageLogObservers.AddObservers(UpdateLog);
    AddChild(godotTestRoot);

    RunnerConfig runnerConfig = new();
    try
    {
      runnerConfig = RunnerConfig.LoadFromFile();
    }
    catch (Exception ex)
    {
      godotTestRoot.logger.LogException(ex);
      GD.Print(ex);
    }

    string[] namespacesToRun = runnerConfig.namespacesToRun;
    if (runnerConfig.namespacesToRun.IsEmpty())
    {
      namespacesToRun = AssemblyExtractor.GetAllTestNamespaces().ToArray();
    }

    godotTestRoot.RunTestsInNamespaces(
        AssemblyExtractor.GetAllTestNamespaces().ToArray());
  }

  public void UpdateLog(Message message)
  {
    string prefix = message.GetPrefix();

    if (message.GetMessageType() == Message.TYPE.SUCCESS)
    {
      prefix = AddBBCodeColor(prefix, "Lawngreen");
    }
    else if (message.GetMessageType() == Message.TYPE.INFO)
    {
      prefix = AddBBCodeColor(prefix, "Deepskyblue");
    }
    else if (message.GetMessageType() == Message.TYPE.ERROR)
    {
      prefix = AddBBCodeColor(prefix, "Red");
    }
    else if (message.GetMessageType() == Message.TYPE.WARNING)
    {
      prefix = AddBBCodeColor(prefix, "Orange");
    }

    output.CallDeferred(
      RichTextLabel.MethodName.AppendText,
      message.GetIndentation() + prefix + message.GetText() + "\n");
  }

  private string AddBBCodeColor(string text, string color)
  {
    return $"[color={color}]{text}[/color]";
  }
}
