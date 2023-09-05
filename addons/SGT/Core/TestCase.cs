namespace SGT;
using System;
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

  public T LoadNode<T>(string pathToNode) where T : Node
  {
    return GD.Load<PackedScene>(pathToNode).InstantiateOrNull<T>();
  }

  /// <summary> 
  /// Called automatically after each test case (called after SimpleAfterEach)
  /// </summary>
  public void CleanUpChildNodes()
  {
    godotTestRoot.CallDeferred("DeleteAllChildren");
  }
}