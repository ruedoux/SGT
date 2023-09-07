namespace SGT;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

internal class TestObjectRunner
{
  private readonly GodotTestRoot godotTestRoot;
  private readonly SimpleTestClass testedObject;
  private readonly long timeoutMs;
  private readonly MethodInfo[] methods;
  private bool testPassed = true;

  public TestObjectRunner(
    GodotTestRoot godotTestRoot,
    SimpleTestClass testedObject,
    long timeoutMs)
  {
    this.godotTestRoot = godotTestRoot;
    this.testedObject = testedObject;
    this.timeoutMs = timeoutMs;

    testedObject.godotTestRoot = godotTestRoot;
    methods = testedObject.GetType().GetMethods(
      BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
  }

  public bool RunAllTestsInObject()
  {
    var simpleTestMethods = MethodClassifier
      .GetAllAttributeMethods<SimpleTestMethod>(methods);
    var className = testedObject.GetType().Name;

    if (simpleTestMethods.Count == 0)
    {
      godotTestRoot.logger.Log(MessageTemplates.GetSkipEmptyClass(className));
      return true;
    }

    var stopwatch = Stopwatch.StartNew();

    godotTestRoot.logger.StartBlock(MessageTemplates.GetRunClass(className));
    testPassed &= RunHelperMethod<SimpleBeforeAll>();
    foreach (var method in simpleTestMethods)
    {
      RunSingleTestMethodCase(method);
    }
    testPassed &= RunHelperMethod<SimpleAfterAll>();

    Message endMessage = testPassed
      ? MessageTemplates.GetEndSuccessClass(className, stopwatch.ElapsedMilliseconds)
      : MessageTemplates.GetEndFailedClass(className, stopwatch.ElapsedMilliseconds);
    godotTestRoot.logger.EndBlock(endMessage);

    return testPassed;
  }

  private void RunSingleTestMethodCase(MethodInfo method)
  {
    uint repeatTest = method.GetCustomAttribute<SimpleTestMethod>().repeatTest;
    for (uint i = 0; i < repeatTest; i++)
    {
      testPassed &= RunHelperMethod<SimpleBeforeEach>();
      testPassed &= RunTestMethod(method);
      testPassed &= RunHelperMethod<SimpleAfterEach>();
      testedObject.CleanUpTestRootChildNodes();
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
        godotTestRoot.logger.Log(MessageTemplates.GetPassedTestMessage(
          methodInfo.Name, stopwatch.ElapsedMilliseconds));
      }
      return true;
    }
    catch (TimeoutException)
    {
      godotTestRoot.logger.Log(MessageTemplates.GetTimeoutTestMessage(
        methodInfo.Name, stopwatch.ElapsedMilliseconds));
    }
    catch (Exception ex)
    {
      godotTestRoot.logger.Log(
        MessageTemplates.GetFailedTestMessage(
          methodInfo.Name, stopwatch.ElapsedMilliseconds));
      godotTestRoot.logger.LogException(ex);
    }

    return false;
  }
}