using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace SGT;


internal class TestMethod
{
  public uint repeat;
  public Stopwatch stopwatch;
  public readonly GodotTestRoot godotTestRoot;
  public readonly SimpleTestClass testedObject;
  public readonly MethodInfo[] allMethods;
  public readonly MethodInfo methodInfo;
  public readonly uint allRepeatCount;

  public TestMethod(
    MethodInfo methodInfo, SimpleTestClass testedObject, GodotTestRoot godotTestRoot)
  {
    this.methodInfo = methodInfo;
    this.testedObject = testedObject;
    this.godotTestRoot = godotTestRoot;

    allMethods = testedObject.GetType().GetMethods(
      BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
    allRepeatCount = methodInfo.GetCustomAttribute<SimpleTestMethod>().repeatTest;
  }

  public bool Run()
  {
    stopwatch = Stopwatch.StartNew();
    try
    {
      RunHelperMethod<SimpleBeforeAll>();
      for (; repeat < allRepeatCount; repeat++)
      {
        RunHelperMethod<SimpleBeforeEach>();
        RunAsyncTestMethod().GetAwaiter().GetResult();
        RunHelperMethod<SimpleAfterEach>();
        testedObject.CleanUpTestRootChildNodes();
      }
      RunHelperMethod<SimpleAfterAll>();
      godotTestRoot.logger.Log(MessageTemplates.GetTestResultMessage(this, true));
      return true;
    }
    catch (TimeoutException)
    {
      godotTestRoot.logger.Log(MessageTemplates.GetTestTimeoutMessage(this));
    }
    catch (Exception ex)
    {
      godotTestRoot.logger.Log(MessageTemplates.GetTestResultMessage(this, false));
      godotTestRoot.logger.LogException(ex);
    }

    return false;
  }

  private async Task RunAsyncTestMethod()
  {
    await Task.Run(() => methodInfo.Invoke(testedObject, null))
        .WaitAsync(TimeSpan.FromMilliseconds(SGTConfig.testTimeoutTimeMs));
  }

  private void RunHelperMethod<T>() where T : Attribute
  {
    var helperMethodInfo = MethodClassifier.GetSingleAttributeMethod<T>(
      allMethods, testedObject);

    if (helperMethodInfo != null)
    {
      Task.Run(() => helperMethodInfo.Invoke(testedObject, null))
          .Wait(TimeSpan.FromMilliseconds(SGTConfig.testTimeoutTimeMs));
    }
  }
}