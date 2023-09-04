using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace SGT;

internal class TestObjectRunner
{
  private readonly GodotInterface godotInterface;
  private readonly Logger logger;
  private readonly object testedObject;
  private readonly long timeoutMs;
  private readonly MethodInfo[] methods;
  private bool testPassed = true;

  public TestObjectRunner(
    GodotInterface godotInterface,
    Logger logger,
    object testedObject,
    long timeoutMs)
  {
    this.godotInterface = godotInterface;
    this.logger = logger;
    this.testedObject = testedObject;
    this.timeoutMs = timeoutMs;
    methods = testedObject.GetType().GetMethods(
      BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic); ;
  }

  public bool RunAllTestsInObject()
  {
    var simpleTestMethods = MethodClassifier.GetAllAttributeMethods<SimpleTestMethod>(methods);
    if (simpleTestMethods.Count == 0)
    {
      logger.Log($"> Skipping: {testedObject.GetType().Name}, no test methods found.");
      return true;
    }

    var stopwatch = Stopwatch.StartNew();

    logger.AnnounceBlockStart($"> Running: {testedObject.GetType().Name}");
    UpdateTestStatus(RunHelperMethod<SimpleBeforeAll>());
    foreach (var method in simpleTestMethods)
    {
      RunSingleTestMethodCase(method);
    }
    UpdateTestStatus(RunHelperMethod<SimpleAfterAll>());
    logger.AnnounceBlockEnd($"> {MessageTemplates.GetTestResultString(testPassed)} {testedObject.GetType().Name} | took: {stopwatch.ElapsedMilliseconds}ms");

    return testPassed;
  }

  private void RunSingleTestMethodCase(MethodInfo method)
  {
    uint repeatTest = method.GetCustomAttribute<SimpleTestMethod>().repeatTest;
    for (uint i = 0; i < repeatTest; i++)
    {
      UpdateTestStatus(RunHelperMethod<SimpleBeforeEach>());
      UpdateTestStatus(RunTestMethod(method));
      UpdateTestStatus(RunHelperMethod<SimpleAfterEach>());
    }
  }

  private bool RunTestMethod(MethodInfo methodInfo) => Task.Run(() =>
    RunAsyncMethod(methodInfo, true)).Result;

  private bool RunHelperMethod<T>() where T : Attribute => Task.Run(() =>
    RunAsyncMethod(MethodClassifier.GetSingleAttributeMethod<T>(
      methods, testedObject), false)).Result;

  private async Task<bool> RunAsyncMethod(MethodInfo methodInfo, bool logSuccess)
  {
    if (methodInfo == null) { return true; }

    var stopwatch = Stopwatch.StartNew();
    var testTask = Task.Run(() => methodInfo.Invoke(testedObject, null));
    try
    {
      await testTask.WaitAsync(TimeSpan.FromMilliseconds(timeoutMs));
      if (logSuccess)
      {
        logger.Log(MessageTemplates.GetMethodResultMessage(
          true, methodInfo.Name, stopwatch.ElapsedMilliseconds));
      }
      return true;
    }
    catch (TimeoutException)
    {
      logger.Log(MessageTemplates.GetTimeoutMessage(
        methodInfo.Name, stopwatch.ElapsedMilliseconds));
    }
    catch (Exception ex)
    {
      logger.Log(MessageTemplates.GetMethodResultMessage(
        false, methodInfo.Name, stopwatch.ElapsedMilliseconds));
      logger.Log($"{ex.InnerException}\n");
    }

    return false;
  }

  private void UpdateTestStatus(bool result)
  {
    if (!result)
    {
      testPassed = false;
    }
  }
}