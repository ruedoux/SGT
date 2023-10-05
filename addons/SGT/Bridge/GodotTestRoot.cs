namespace SGT;

using Godot;

internal partial class GodotTestRoot : Control
{
  internal Logger logger = new();
  internal bool testsFinished = true;

  public GodotTestRoot()
  {
    logger.messageLogObservers.AddObservers(new MessagePrinter(GD.PrintRich, true).Print);
  }

  public Runner RunTestsInNamespaces(string[] namespaces)
  {
    if (!testsFinished)
      return null;

    Runner runner = new(this, logger, namespaces);
    runner.Run();
    testsFinished = false;
    return runner;
  }

  internal void FinalizeTest()
  {
    ObjectSerializer.SaveToFile(Config.testResultsPath, logger.messageAgregator);
    logger.Log(new Message(
      Message.Severity.INFO,
      Message.SuiteType.STAY,
      Message.SuiteKind.INFO,
      $"Saved test results to file: {Config.testResultsPath}"));
    testsFinished = true;
  }

  public void FreeAllChildren()
  {
    foreach (var child in GetChildren())
      child.QueueFree();
  }
}
