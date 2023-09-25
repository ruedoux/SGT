namespace SGT;

using System.Threading.Tasks;
using Godot;

internal partial class GodotTestRoot : Control
{
  internal Logger logger = new();
  internal bool testsFinished = false;
  internal bool testsStarted = false;

  public GodotTestRoot()
  {
    logger.messageLogObservers.AddObservers(new MessagePrinter(GD.PrintRich, true).Print);
  }

  public void RunTestsInNamespaces(string[] namespaces)
  {
    Runner runner = new(this, logger, namespaces);
    Task.Run(() => runner.Run());
    testsStarted = true;
  }

  public void DeleteAllChildren()
  {
    foreach (Node child in GetChildren())
    {
      child.QueueFree();
    }
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
}
