using System;
using System.Collections.Generic;
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
    Stopwatch stopwatch = new();
    stopwatch.Start();

    Logger.Log("> Starting Tests");
    Logger.IncreaseIndentation();
    foreach (string namespaceName in AssemblyExtractor.GetAllTestNamespaces())
    {
      testsPassed = testsPassed && RunTestsInNamespace(namespaceName);
    }
    Logger.DecreaseIndentation();
    stopwatch.Stop();
    Logger.Log($"> Finishing Tests | took: {stopwatch.ElapsedMilliseconds}ms");

    return testsPassed;
  }

  public static bool RunTestsInNamespace(string namespaceName)
  {
    bool testsPassed = true;
    var testObjects = AssemblyExtractor.GetTestObjectsInNamespace(namespaceName);
    Stopwatch stopwatch = new();
    stopwatch.Start();

    Logger.Log($"> Begin tests for namespace: {namespaceName}");
    Logger.IncreaseIndentation();
    foreach (var instance in testObjects)
    {
      testsPassed = testsPassed && RunAllTestsInObject(instance);
    }
    Logger.DecreaseIndentation();
    stopwatch.Stop();
    Logger.Log($"> End tests for namespace: {namespaceName} | took: {stopwatch.ElapsedMilliseconds}ms");

    return testsPassed;
  }

  private static bool RunAllTestsInObject(object objectToRun)
  {
    bool testsPassed = true;
    var methods = GetMethodsFromObject(objectToRun);

    var simpleTestMethods =
      MethodClassifier.GetAllAttributeMethods<SimpleTestMethod>(methods);
    var beforeEachMethod =
      MethodClassifier.GetSingleAttributeMethod<SimpleBeforeEach>(methods, objectToRun);
    var afterEachMethod =
      MethodClassifier.GetSingleAttributeMethod<SimpleAfterEach>(methods, objectToRun);

    Stopwatch stopwatch = new();
    stopwatch.Start();

    Logger.Log($"> Running: {objectToRun.GetType().Name}");
    foreach (var method in simpleTestMethods)
    {
      testsPassed = testsPassed && Task.Run(() =>
        RunHelperMethod(beforeEachMethod, objectToRun, typeof(SimpleBeforeEach))).Result;
      testsPassed = testsPassed && Task.Run(() =>
        RunTestMethod(method, objectToRun)).Result;
      testsPassed = testsPassed && Task.Run(() =>
        RunHelperMethod(afterEachMethod, objectToRun, typeof(SimpleAfterEach))).Result;
    }
    stopwatch.Start();
    Logger.Log($"> {GetTestResultString(testsPassed)} {objectToRun.GetType().Name} | took: {stopwatch.ElapsedMilliseconds}ms");

    return testsPassed;
  }


  private static async Task<bool> RunTestMethod(
    MethodInfo methodInfo, object objectToRun)
  {
    Stopwatch stopwatch = new();
    stopwatch.Start();

    var testTask = Task.Run(() => methodInfo.Invoke(objectToRun, null));
    try
    {
      await testTask.WaitAsync(TimeSpan.FromMilliseconds(timeoutMs));
      Logger.Log($"[TEST PASS] {methodInfo.Name} | took: {stopwatch.ElapsedMilliseconds}ms");
      return true;
    }
    catch (TimeoutException)
    {
      Logger.Log($"[TEST TIMEOUT] {methodInfo.Name} | Timeout reached: {timeoutMs}ms");
    }
    catch (Exception ex)
    {
      Logger.Log($"[TEST FAIL] {methodInfo.Name} | took: {stopwatch.ElapsedMilliseconds}ms");
      Logger.Log($"{ex.InnerException}\n");
    }

    return false;
  }

  private static async Task<bool> RunHelperMethod(
    MethodInfo methodInfo, object objectToRun, Type methodType)
  {
    if (methodInfo == null) // Helper methods are not mandatory
    {
      return true;
    }

    var testTask = Task.Run(() => methodInfo.Invoke(objectToRun, null));
    try
    {
      await testTask.WaitAsync(TimeSpan.FromMilliseconds(timeoutMs));
      return true;
    }
    catch (TimeoutException)
    {
      Logger.Log($"[HELPER METHOD TIMEOUT] {methodInfo.Name} of type {methodType.FullName} | Timeout reached: {timeoutMs}ms");
    }
    catch (Exception ex)
    {
      Logger.Log($"[HELPER METHOD FAIL] {methodInfo.Name} of type {methodType.FullName}");
      Logger.Log($"{ex.InnerException}\n");

    }

    return false;
  }

  private static MethodInfo[] GetMethodsFromObject(object testObject)
  {
    return testObject.GetType().GetMethods(
      BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
  }

  private static string GetTestResultString(bool result)
    => result ? "{PASSED}" : "{FAILED}";

}
