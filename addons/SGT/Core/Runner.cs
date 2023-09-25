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
    try
    {
      return RunAllNamespaces();
    }
    catch (Exception ex)
    {
      logger.Log(new Message(
        Message.Severity.FAILED,
        Message.SuiteType.STAY,
        Message.SuiteKind.INFO,
        ex.Message,
        -1,
        ex.StackTrace));
    }
    return false;
  }

  public bool RunAllNamespaces()
  {
    var namespacesOk = AssemblyExtractor.ContainsExistingNamespaces(namespaces);
    if (!namespacesOk.Item1)
    {
      throw new TestSetupException(
        "Failed to start test becuase provided config contains invalid namespaces: " + string.Join(", ", namespacesOk.Item2));
    }
    if (!namespaces.Any())
    {
      throw new TestSetupException(
        "Failed to start test becuase provided no namespaces.");
    }

    bool isPassed = true;
    string allNamespaces = $"({string.Join(", ", namespaces)})";
    isPassed &= RunSuiteWithLog(allNamespaces, Message.SuiteKind.ALL, () =>
    {
      foreach (string namespaceName in namespaces)
      {
        isPassed &= RunNamespace(namespaceName);
      }
      return isPassed;
    });

    godotTestRoot.FinalizeTest();

    return isPassed;
  }

  private bool RunNamespace(string namespaceName)
  {
    var testObjects = AssemblyExtractor.GetTestObjectsInNamespace(namespaceName);

    bool isPassed = true;
    isPassed &= RunSuiteWithLog(namespaceName, Message.SuiteKind.NAMESPACE, () =>
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
    isPassed &= RunSuiteWithLog(testObject.GetType().Name, Message.SuiteKind.CLASS, () =>
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
        Message.SuiteType.STAY,
        Message.SuiteKind.METHOD,
        thisMethod.Name,
        methodStopwatch.ElapsedMilliseconds));
    }
    catch (TimeoutException)
    {
      isPassed &= false;
      logger.Log(new Message(
        Message.Severity.TIMEOUT,
        Message.SuiteType.STAY,
        Message.SuiteKind.METHOD,
        thisMethod.Name,
        methodStopwatch.ElapsedMilliseconds));
    }
    catch (Exception ex)
    {
      isPassed &= false;
      logger.Log(new Message(
        Message.Severity.FAILED,
        Message.SuiteType.STAY,
        Message.SuiteKind.METHOD,
        thisMethod.Name,
        methodStopwatch.ElapsedMilliseconds,
        ex.InnerException.ToString()));
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