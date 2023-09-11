using Godot;
using SGT;

public class SceneTest : SimpleTestClass
{
  [SimpleTestMethod]
  public void Test()
  {
    // Given
    var loadedNode = LoadNode<TestNode>("res://Examples/Scenes/TestNode.tscn");

    // When
    loadedNode.shouldBe10 = 123;
    CallTestRootDeffered(Node.MethodName.AddChild, loadedNode);

    // Then
    Assertions.AssertAwaitAtMost(1000, () =>
    {
      Assertions.AssertEqual(10, loadedNode.shouldBe10);
      Assertions.AssertEqual(5, loadedNode.Returns5());
    });
  }
}