using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SGT;

internal class Runner : RunnerTemplate
{
  private readonly string[] namespaces;

  internal Runner(GodotTestRoot godotTestRoot, Logger logger, string[] namespaces)
    : base(godotTestRoot, logger)
  {
    this.namespaces = namespaces;
  }

  public override bool Run()
  {
    if (!AssemblyExtractor.ContainsExistingNamespaces(namespaces))
    {
      throw new TestSetupException(
        "Failed to start test becuase provided config contains invalid namespace.");
    }
    if (namespaces.Count() == 0)
    {
      throw new TestSetupException(
        "Failed to start test becuase provided no namespaces.");
    }

    bool isPassed = true;
    string allNamespaces = $"Namespaces to run: {string.Join(", ", namespaces)}";
    isPassed &= RunSuiteWithLog(allNamespaces, () =>
    {
      foreach (string namespaceName in namespaces)
      {
        isPassed &= RunNamespace(namespaceName);
      }
      return isPassed;
    });

    return isPassed;
  }

  private bool RunNamespace(string namespaceName)
  {
    var testObjects = AssemblyExtractor.GetTestObjectsInNamespace(namespaceName);

    bool isPassed = true;
    string namespaceToRun = $"Running namespace: {namespaceName}";
    isPassed &= RunSuiteWithLog(namespaceToRun, () =>
    {
      foreach (var testObject in testObjects)
      {
        testObject.godotTestRoot = godotTestRoot;
        isPassed &= RunClass(testObject);
      }
      return isPassed;
    });

    return isPassed;
  }

  private bool RunClass(SimpleTestClass testObject)
  {
    var allMethods = testObject.GetType().GetMethods(
      BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
    var simpleTestMethods = MethodClassifier
      .GetAllAttributeMethods<SimpleTestMethod>(allMethods);

    if (simpleTestMethods.Count == 0) { return true; }

    bool isPassed = true;
    string classToRun = $"Running class: {testObject.GetType().Name}";
    isPassed &= RunSuiteWithLog(classToRun, () =>
    {
      foreach (var method in simpleTestMethods)
      {
        isPassed &= RunMethod(method, allMethods, testObject);
      }
      return isPassed;
    });

    return isPassed;
  }

  private bool RunMethod(
    MethodInfo thisMethod, MethodInfo[] allMethods, SimpleTestClass testObject)
  {
    bool isPassed = true;
    var methodStopwatch = Stopwatch.StartNew();

    var testRepeats = thisMethod.GetCustomAttribute<SimpleTestMethod>().repeatTest;
    try
    {
      RunHelperMethod<SimpleBeforeAll>(allMethods, testObject);
      for (uint i = 0; i < testRepeats; i++)
      {
        RunHelperMethod<SimpleBeforeEach>(allMethods, testObject);
        RunAsyncTestMethod(thisMethod, testObject).GetAwaiter().GetResult();
        RunHelperMethod<SimpleAfterEach>(allMethods, testObject);
        testObject.CleanUpTestRootChildNodes();
      }
      RunHelperMethod<SimpleAfterAll>(allMethods, testObject);
      isPassed &= true;
      logger.Log(new Message(
        Message.Severity.PASSED,
        Message.SuiteType.METHOD,
        thisMethod.Name,
        methodStopwatch.ElapsedMilliseconds));
    }
    catch (TimeoutException)
    {
      isPassed &= false;
      logger.Log(new Message(
        Message.Severity.TIMEOUT,
        Message.SuiteType.METHOD,
        thisMethod.Name,
        methodStopwatch.ElapsedMilliseconds));
    }
    catch (Exception ex)
    {
      isPassed &= false;
      logger.Log(new Message(
        Message.Severity.FAILED,
        Message.SuiteType.METHOD,
        thisMethod.Name,
        methodStopwatch.ElapsedMilliseconds,
        ex.ToString()));
    }

    return isPassed;
  }

  private async Task RunAsyncTestMethod(
    MethodInfo thisMethod, SimpleTestClass testObject)
  {
    await Task.Run(() => thisMethod.Invoke(testObject, null))
        .WaitAsync(TimeSpan.FromMilliseconds(Config.testTimeoutTimeMs));
  }

  private void RunHelperMethod<T>(
    MethodInfo[] allMethods, SimpleTestClass testObject) where T : Attribute
  {
    var helperMethodInfo = MethodClassifier.GetSingleAttributeMethod<T>(
      allMethods, testObject);

    if (helperMethodInfo != null)
    {
      Task.Run(() => helperMethodInfo.Invoke(testObject, null))
          .Wait(TimeSpan.FromMilliseconds(Config.testTimeoutTimeMs));
    }
  }
}