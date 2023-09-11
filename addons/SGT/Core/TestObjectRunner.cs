namespace SGT;
using System.Diagnostics;
using System.Reflection;

internal class TestObjectRunner
{
  private readonly GodotTestRoot godotTestRoot;
  private readonly SimpleTestClass testedObject;
  private readonly MethodInfo[] methods;

  public TestObjectRunner(GodotTestRoot godotTestRoot, SimpleTestClass testedObject)
  {
    this.godotTestRoot = godotTestRoot;
    this.testedObject = testedObject;

    testedObject.godotTestRoot = godotTestRoot;
    methods = testedObject.GetType().GetMethods(
      BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
  }

  public bool Run()
  {
    bool testsPassed = true;
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

    foreach (var method in simpleTestMethods)
    {
      TestMethod testMethod = new(method, testedObject, godotTestRoot);
      testsPassed &= testMethod.Run();
    }

    godotTestRoot.logger.EndBlock(
      MessageTemplates.GetSuitResultMessage(
        className, stopwatch.ElapsedMilliseconds, testsPassed));

    return testsPassed;
  }
}