using Godot;
using SGT;

namespace DefaultTests;

public class SceneTest : SimpleTestClass
{
  [SimpleTestMethod]
  public void TestAttributeAccess()
  {
    // Given
    var loadedNode = LoadSceneInstance<TestNode>(
      "res://Examples/Default/Scenes/TestNode.tscn");

    // When
    loadedNode.shouldBe10 = 123;
    CallTestRootDeffered(Node.MethodName.AddChild, loadedNode);

    // Then
    Assertions.AssertAwaitAtMost(5000, () =>
    {
      Assertions.AssertEqual(10, loadedNode.shouldBe10);
    });
  }

  [SimpleTestMethod]
  public void TestFunctionAccess()
  {
    // Given
    var loadedNode = LoadSceneInstance<TestNode>(
      "res://Examples/Default/Scenes/TestNode.tscn");

    // When
    loadedNode.shouldBe10 = 123;
    CallTestRootDeffered(Node.MethodName.AddChild, loadedNode);

    // Then
    Assertions.AssertAwaitAtMost(5000, () =>
    {
      Assertions.AssertEqual(5, loadedNode.Returns5());
    });
  }
}