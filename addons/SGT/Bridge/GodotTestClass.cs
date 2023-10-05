using System.Collections.Generic;
using Godot;

namespace SGT;

public abstract class SimpleTestClass
{
  internal GodotTestRoot godotTestRoot;

  protected void CallDeferred(StringName methodName, params Variant[] args)
    => godotTestRoot.CallDeferred(methodName, args);

  public T LoadSceneInstance<T>(string path) where T : Node
  {
    var scene = ResourceLoader.Load<PackedScene>(path, null, ResourceLoader.CacheMode.Replace);
    return scene.InstantiateOrNull<T>();
  }

  public void FailTest(string faileCause = "")
    => throw new AssertionException($"Failed test. {faileCause}");

  public void TestLog(params object[] msgs)
  {
    string output = "";
    foreach (var msg in msgs)
      output += msg.ToString();

    godotTestRoot.logger.Log(new Message(
      Message.Severity.INFO, Message.SuiteType.STAY, Message.SuiteKind.INFO, output));
  }

  public static string EnumerableToString<T>(
    IEnumerable<T> enumerable, string delimiter = ", ", int maxLen = 100)
  {
    string joinedEnumerable = string.Join(delimiter, enumerable);
    if (joinedEnumerable.Length > maxLen)
      joinedEnumerable = joinedEnumerable[..maxLen] + " [...] ";
    return $"{typeof(T).Name} : [{joinedEnumerable}]";
  }

  public void FreeChildNodes()
    => godotTestRoot.CallDeferred(nameof(godotTestRoot.FreeAllChildren));
}