using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace SGT;

public static class Runner
{
  public static int timeoutMs = 60 * 1000;

  public static bool RunAllTests()
  {
    bool testsPassed = true;

    Logger.Log("> Starting Tests");
    Logger.IncreaseIndentation();
    foreach (string namespaceName in AssemblyExtractor.GetAllTestNamespaces())
    {
      testsPassed = testsPassed && RunTestsInNamespace(namespaceName);
    }
    Logger.DecreaseIndentation();
    Logger.Log("> Finishing Tests");

    return testsPassed;
  }

  public static bool RunTestsInNamespace(string namespaceName)
  {
    bool testsPassed = true;
    var testObjects = AssemblyExtractor.GetTestObjectsInNamespace(namespaceName);

    Logger.Log($"> Begin tests for namespace: " + namespaceName);
    Logger.IncreaseIndentation();
    foreach (var instance in testObjects)
    {
      testsPassed = testsPassed && RunAllTestsInObject(instance);
    }
    Logger.DecreaseIndentation();
    Logger.Log("> End tests for namespace: " + namespaceName);

    return testsPassed;
  }

  private static bool RunAllTestsInObject(object objectToRun)
  {
    var methods = objectToRun.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
    bool testsPassed = true;

    Logger.Log($"> Running: {objectToRun.GetType().Name}");
    foreach (var method in methods)
    {
      if (!Attribute.IsDefined(method, typeof(SimpleTestMethod)))
      {
        continue;
      }
      bool result = Task.Run(() => RunTest(method, objectToRun)).Result;
      testsPassed = testsPassed && result;
    }
    Logger.Log(GetTestResultString(testsPassed) + objectToRun.GetType().Name);
    return testsPassed;
  }

  private static async Task<bool> RunTest(MethodInfo methodInfo, object objectToRun)
  {
    Stopwatch stopwatch = new();
    stopwatch.Start();

    var testTask = Task.Run(() => methodInfo.Invoke(objectToRun, null));
    try
    {
      await testTask.WaitAsync(TimeSpan.FromMilliseconds(timeoutMs));
      LogSuccess(methodInfo.Name, stopwatch.ElapsedMilliseconds);
      return true;
    }
    catch (TimeoutException)
    {
      LogTimeout(methodInfo.Name);
    }
    catch (Exception ex)
    {
      LogFail(methodInfo.Name, ex, stopwatch.ElapsedMilliseconds);
    }

    return false;
  }

  private static void LogSuccess(string methodName, long timeMs)
  {
    Logger.Log($"[TEST PASS] {methodName} {timeMs}ms");
  }

  private static void LogFail(string methodName, Exception ex, long timeMs)
  {
    Logger.Log($"[TEST FAIL] {methodName} {timeMs}ms");
    Logger.Log($"{ex.InnerException}\n");
  }

  private static void LogTimeout(string methodName)
  {
    Logger.Log($"[TEST TIMEOUT] {methodName} | Timeout reached: {timeoutMs}ms");
  }

  private static string GetTestResultString(bool result)
  {
    if (result)
    {
      return "> PASSED ";
    }
    return "> FAILED ";
  }
}
