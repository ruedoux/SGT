Test Setup
---

Your test class needs to derive from `SimpleTestClass` and test methods needs to have `[SimpleTestMethod]` attribute.

To run the tests you can use [runner scene](https://github.com/RedouxG/SGT/blob/main/addons/SGT/Bridge/GUI/Runner) which displays test results in a convenient form, no additional setup is needed for it to work (in 4.1.2 version at least).

Example test class would look something like this:

```cs
public class SceneTest : SimpleTestClass
{
  [SimpleTestMethod]
  public void Test()
  {
    // Given
    var loadedNode = LoadSceneInstance<TestNode>(
      "res://Examples/Scenes/TestNode.tscn");

    // When
    loadedNode.shouldBe10 = 123;
    CallTestRootDeffered(Node.MethodName.AddChild, loadedNode);

    // Then
    Assertions.AssertAwaitAtMost(5000, () =>
    {
      Assertions.AssertEqual(10, loadedNode.shouldBe10);
      Assertions.AssertEqual(5, loadedNode.Returns5());
    });
  }
}
```

Tests are ran by namespace so you can manually run only one namespace at a time if you want.

All calls to godot scene root need to be done via deffered call: `CallTestRootDeffered()`, since tests are being ran async.