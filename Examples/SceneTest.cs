using SGT;

internal class SceneTest : SimpleTestClass
{
  [SimpleTestMethod]
  public void Test()
  {
    // Given
    var pathToNode = "res://Examples/Scenes/TestNode.tscn";
    var loadedNode = LoadNode<TestNode>(pathToNode);

    // When
    AddNodeToTestTree(loadedNode);

    // Then
    Assertions.AssertAwaitAtMost(1000, () =>
    {
      Assertions.AssertEqual(loadedNode.shouldBe10, 10);
    });
  }
}