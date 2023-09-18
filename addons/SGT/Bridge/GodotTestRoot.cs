namespace SGT;

using System.Threading.Tasks;
using Godot;

public partial class GodotTestRoot : Control
{
  public Logger logger = new();

  public GodotTestRoot()
  {
    logger.messageLogObservers.AddObservers(new MessagePrinter(GD.Print, false).Print);
  }

  public void RunTestsInNamespaces(string[] namespaces)
  {
    Runner runner = new(this, logger, namespaces);
    Task.Run(() => runner.Run());
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
      Message.SuiteType.NONE,
      Message.SuiteKind.INFO,
      $"Saved test results to file: {Config.testResultsPath}"));
  }
}
