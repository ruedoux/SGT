namespace SGT;
using System;
using Godot;


public partial class GodotRunner : Control
{
  private readonly GodotTestRoot godotTestRoot = new();
  private readonly MessagePrinter messagePrinter = new();
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
      godotTestRoot.RunTestsInNamespaces(runnerConfig.namespaces);
    }
    catch (Exception ex)
    {
      throw new TestSetupException("Failed to load config!", ex);
    }
  }

  public void UpdateLog(Message message)
  {
    string bbcode = "";

    if (message.severity == Message.Severity.PASSED)
    {
      bbcode = "Lawngreen";
    }
    else if (message.severity == Message.Severity.INFO)
    {
      bbcode = "Deepskyblue";
    }
    else if (message.severity == Message.Severity.FAILED)
    {
      bbcode = "Red";
    }
    else if (message.severity == Message.Severity.TIMEOUT)
    {
      bbcode = "Orange";
    }

    output.CallDeferred(
      RichTextLabel.MethodName.AppendText,
      messagePrinter.GetAsString(message, bbcode) + '\n');
  }


}
