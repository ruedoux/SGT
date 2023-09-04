namespace SGT;
using System.Diagnostics;


public class Runner
{
  public long timeoutMs = 60 * 1000;
  private readonly GodotInterface godotInterface;
  private readonly Logger logger;


  internal Runner(GodotInterface godotInterface)
  {
    this.godotInterface = godotInterface;
    logger = new(godotInterface);
  }

  public bool RunAllTests()
  {
    bool testsPassed = true;
    var stopwatch = Stopwatch.StartNew();

    logger.AnnounceBlockStart("> Starting Tests");
    foreach (string namespaceName in AssemblyExtractor.GetAllTestNamespaces())
    {
      testsPassed = testsPassed && RunTestsInNamespace(namespaceName);
    }
    logger.AnnounceBlockEnd($"> {MessageTemplates.GetTestResultString(testsPassed)} Finishing Tests | took: {stopwatch.ElapsedMilliseconds}ms");

    return testsPassed;
  }

  public bool RunTestsInNamespace(string namespaceName)
  {
    bool testsPassed = true;
    var testObjects = AssemblyExtractor.GetTestObjectsInNamespace(namespaceName);
    var stopwatch = Stopwatch.StartNew();

    logger.AnnounceBlockStart($"> Begin tests for namespace: {namespaceName}");
    foreach (var instance in testObjects)
    {
      testsPassed = testsPassed &&
        new TestObjectRunner(
          godotInterface, logger, instance, timeoutMs).RunAllTestsInObject();
    }
    logger.AnnounceBlockEnd($"> {MessageTemplates.GetTestResultString(testsPassed)} End tests for namespace: {namespaceName} | took: {stopwatch.ElapsedMilliseconds}ms");

    return testsPassed;
  }
}
