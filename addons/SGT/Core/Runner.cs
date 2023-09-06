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

  public bool RunAllTests()
  {
    bool testsPassed = true;
    var stopwatch = Stopwatch.StartNew();

    godotTestRoot.logger.AnnounceBlockStart("> Starting Tests");
    var allNamespaces = AssemblyExtractor.GetAllTestNamespaces();
    foreach (string namespaceName in allNamespaces)
    {
      testsPassed &= RunTestsInNamespace(namespaceName);
    }
    godotTestRoot.logger.AnnounceBlockEnd($"> {MessageTemplates.GetTestResultString(testsPassed)} Finishing Tests | took: {stopwatch.ElapsedMilliseconds}ms");

    return testsPassed;
  }

  public bool RunTestsInNamespace(string namespaceName)
  {
    bool testsPassed = true;
    var testObjects = AssemblyExtractor.GetTestObjectsInNamespace(namespaceName);
    var stopwatch = Stopwatch.StartNew();

    godotTestRoot.logger.AnnounceBlockStart($"> Begin tests for namespace: {namespaceName}");
    foreach (var instance in testObjects)
    {
      testsPassed &= new TestObjectRunner(
          godotTestRoot, instance, timeoutMs).RunAllTestsInObject();
    }
    godotTestRoot.logger.AnnounceBlockEnd($"> {MessageTemplates.GetTestResultString(testsPassed)} End tests for namespace: {namespaceName} | took: {stopwatch.ElapsedMilliseconds}ms");

    return testsPassed;
  }
}
