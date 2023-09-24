namespace SGT;
using System;
using System.Collections.Generic;
using Godot;

[AttributeUsage(AttributeTargets.Method)]
public class SimpleTestMethod : Attribute
{
  public uint repeatTest = 1;
}

[AttributeUsage(AttributeTargets.Method)]
public class SimpleBeforeEach : Attribute { }

[AttributeUsage(AttributeTargets.Method)]
public class SimpleAfterEach : Attribute { }

[AttributeUsage(AttributeTargets.Method)]
public class SimpleBeforeAll : Attribute { }

[AttributeUsage(AttributeTargets.Method)]
public class SimpleAfterAll : Attribute { }

public abstract class SimpleTestClass
{
  internal GodotTestRoot godotTestRoot;

  public void CallTestRootDeffered(string methodName, params Variant[] args)
  {
    godotTestRoot.CallDeferred(methodName, args);
  }

  public T LoadSceneInstance<T>(string path) where T : class
  {
    var scene = ResourceLoader.Load<PackedScene>(
      path, null, ResourceLoader.CacheMode.Replace);
    return scene.InstantiateOrNull<T>();
  }

  public void FailTest(string faileCause = "")
  {
    throw new AssertionException($"Failed test. {faileCause}");
  }

  public void TestLog(params object[] msgs)
  {
    string output = "";
    foreach (var msg in msgs)
      output += msg.ToString();

    godotTestRoot.logger.Log(new Message(
      Message.Severity.INFO, Message.SuiteType.NONE, Message.SuiteKind.INFO, output));
  }

  public static string EnumerableToString<T>(
    IEnumerable<T> enumerable, string delimiter = ", ", int maxLen = 100)
  {
    string joinedEnumerable = string.Join(delimiter, enumerable);
    if (joinedEnumerable.Length > maxLen)
      joinedEnumerable = joinedEnumerable[..maxLen] + " [...] ";
    return $"{typeof(T).Name} : [{joinedEnumerable}]";
  }

  /// <summary> 
  /// Called automatically after each test case (called after SimpleAfterEach)
  /// </summary>
  public void CleanUpTestRootChildNodes()
  {
    godotTestRoot.CallDeferred("DeleteAllChildren");
  }
}