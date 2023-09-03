namespace SGT;
using System.Diagnostics;


public static class Runner
{
  public static long timeoutMs = 60 * 1000;

  public static bool RunAllTests()
  {
    bool testsPassed = true;
    var stopwatch = Stopwatch.StartNew();

    Logger.AnnounceBlockStart("> Starting Tests");
    foreach (string namespaceName in AssemblyExtractor.GetAllTestNamespaces())
    {
      testsPassed = testsPassed && RunTestsInNamespace(namespaceName);
    }
    Logger.AnnounceBlockEnd($"> {MessageTemplates.GetTestResultString(testsPassed)} Finishing Tests | took: {stopwatch.ElapsedMilliseconds}ms");

    return testsPassed;
  }

  public static bool RunTestsInNamespace(string namespaceName)
  {
    bool testsPassed = true;
    var testObjects = AssemblyExtractor.GetTestObjectsInNamespace(namespaceName);
    var stopwatch = Stopwatch.StartNew();

    Logger.AnnounceBlockStart($"> Begin tests for namespace: {namespaceName}");
    foreach (var instance in testObjects)
    {
      testsPassed = testsPassed &&
        new TestObjectRunner(instance, timeoutMs).RunAllTestsInObject();
    }
    Logger.AnnounceBlockEnd($"> {MessageTemplates.GetTestResultString(testsPassed)} End tests for namespace: {namespaceName} | took: {stopwatch.ElapsedMilliseconds}ms");

    return testsPassed;
  }
}
