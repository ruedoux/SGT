namespace SGT;
using System.Collections.Generic;
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
  /// <summary> 
  /// SGT singleton, access to scene tree 
  /// </summary>
  public GodotInterface godotInterface;
  private readonly List<Node> addedNodes = new();

  /// <summary> 
  /// Loads and adds node to tree
  /// </summary>
  public void AddNodeToTestTree(Node node)
  {
    addedNodes.Add(node);
    godotInterface.CallDeferred("add_child", node);
  }

  public T LoadNode<T>(string pathToNode) where T : Node
  {
    return GD.Load<PackedScene>(pathToNode).InstantiateOrNull<T>();
  }

  /// <summary> 
  /// Called automatically after each test case (called after SimpleAfterEach)
  /// </summary>
  public void CleanUpNodesAfterTest()
  {
    foreach (var node in addedNodes)
    {
      godotInterface.CallDeferred("remove_child", node);
      node.QueueFree();
    }
  }
}