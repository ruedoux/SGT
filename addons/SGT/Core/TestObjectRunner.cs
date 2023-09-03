using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace SGT;

public class TestObjectRunner
{
  private readonly object testedObject;
  private readonly long timeoutMs;
  private readonly MethodInfo[] methods;
  private bool testPassed = true;

  public TestObjectRunner(object testedObject, long timeoutMs)
  {
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
      Logger.Log($"> Skipping: {testedObject.GetType().Name}, no test methods found.");
      return true;
    }

    var stopwatch = Stopwatch.StartNew();

    Logger.AnnounceBlockStart($"> Running: {testedObject.GetType().Name}");
    UpdateTestStatus(RunHelperMethod<SimpleBeforeAll>());
    foreach (var method in simpleTestMethods)
    {
      UpdateTestStatus(RunHelperMethod<SimpleBeforeEach>());
      UpdateTestStatus(RunTestMethod(method));
      UpdateTestStatus(RunHelperMethod<SimpleAfterEach>());
    }
    UpdateTestStatus(RunHelperMethod<SimpleAfterAll>());
    Logger.AnnounceBlockEnd($"> {MessageTemplates.GetTestResultString(testPassed)} {testedObject.GetType().Name} | took: {stopwatch.ElapsedMilliseconds}ms");

    return testPassed;
  }

  public bool RunTestMethod(MethodInfo methodInfo) => Task.Run(() =>
    RunAsyncMethod(methodInfo, true)).Result;

  public bool RunHelperMethod<T>() where T : Attribute => Task.Run(() =>
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
        Logger.Log(MessageTemplates.GetMethodResultMessage(
          true, methodInfo.Name, stopwatch.ElapsedMilliseconds));
      }
      return true;
    }
    catch (TimeoutException)
    {
      Logger.Log(MessageTemplates.GetTimeoutMessage(
        methodInfo.Name, stopwatch.ElapsedMilliseconds));
    }
    catch (Exception ex)
    {
      Logger.Log(MessageTemplates.GetMethodResultMessage(
        false, methodInfo.Name, stopwatch.ElapsedMilliseconds));
      Logger.Log($"{ex.InnerException}\n");
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