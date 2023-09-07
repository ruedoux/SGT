namespace SGT;
using System.Diagnostics;


internal class Runner
{
  public long timeoutMs = 60 * 1000;
  private readonly GodotTestRoot godotTestRoot;

  internal Runner(GodotTestRoot godotTestRoot)
  {
    this.godotTestRoot = godotTestRoot;
  }

  public bool RunTestsInNamespaces(string[] namespaces)
  {
    bool testsPassed = true;

    godotTestRoot.logger.StartBlock(MessageTemplates.GetRunAll(namespaces));
    var stopwatch = Stopwatch.StartNew();
    foreach (string namespaceName in namespaces)
    {
      testsPassed &= RunTestsInNamespace(namespaceName);
    }
    Message endMessage = testsPassed
      ? MessageTemplates.GetEndSuccessAll(stopwatch.ElapsedMilliseconds)
      : MessageTemplates.GetEndFailedAll(stopwatch.ElapsedMilliseconds);
    godotTestRoot.logger.EndBlock(endMessage);

    return testsPassed;
  }

  private bool RunTestsInNamespace(string namespaceName)
  {
    bool testsPassed = true;
    var testObjects = AssemblyExtractor.GetTestObjectsInNamespace(namespaceName);
    var stopwatch = Stopwatch.StartNew();

    godotTestRoot.logger.StartBlock(MessageTemplates.GetRunNamespace(namespaceName));
    foreach (var instance in testObjects)
    {
      testsPassed &= new TestObjectRunner(
          godotTestRoot, instance, timeoutMs).RunAllTestsInObject();
    }
    Message endMessage = testsPassed
      ? MessageTemplates.GetEndSuccessNamespace(namespaceName, stopwatch.ElapsedMilliseconds)
      : MessageTemplates.GetEndFailedNamespace(namespaceName, stopwatch.ElapsedMilliseconds);
    godotTestRoot.logger.EndBlock(endMessage);

    return testsPassed;
  }
}
