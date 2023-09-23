Simple Godot Tests
=========

About
-----

Addon for [Godot](https://github.com/godotengine/godot) that allows you to easily create tests for your C# scripts. Its designed to be as simple to use as possible and have as little dependency issues with different Godot versions.

How to use
-----


**Disclaimer**:
*Everything for this repo on visual side is done for 4.1.1 (latest) version. If you want to use this addon for older versions some minor setup could be needed (look end of readme).*

Your test class needs to derive from `SimpleTestClass` and test methods needs to have `[SimpleTestMethod]` attribute.

To run the tests you can use runner scene which displays test results in a convenient form, no additional setup is needed for it to work (in 4.1.1 version at least).

Example test class would look something like this:

```cs
public class SceneTest : SimpleTestClass
{
  [SimpleTestMethod]
  public void Test()
  {
    // Given
    var loadedNode = LoadNode<TestNode>("res://Examples/Scenes/TestNode.tscn");

    // When
    CallTestRootDeffered(Node.MethodName.AddChild, loadedNode);

    // Then
    Assertions.AssertAwaitAtMost(1000, () =>
    {
      Assertions.AssertEqual(10, loadedNode.shouldBe10);
      Assertions.AssertEqual(5, loadedNode.Returns5());
    });
  }
}
```

Tests are ran by namespace so you can manually run only one namespace at a time if you want.

All calls to godot scene root need to be done via deffered call: `CallTestRootDeffered()`, since tests are being ran async.

Mocking
----

The library works well with [Moq](https://github.com/moq/moq) as far as I have tested. You can use mocks in all test methods without issue.

You can add Moq to your project using:
```
dotnet add package Moq --version <LATEST VERSION>
```


Dev info
-----

Everything is as Godot/IDE agnostic as possible to minigate all of compatibility issues that other plugins tend to have. The only interface entry points with Godot are Logger and EditorRunner classes.

Testing class can be in any `.cs` file in your project, I would recommend putting all tests in a test folder so its easier to exclude the testing files in your `.csproj` file from the realase version of the game:

```xml
<PropertyGroup>
  <DefaultItemExcludes Condition="'$(Configuration)' == 'ExportRelease'">
    $(DefaultItemExcludes);YourTestFolderName/**;addons/SGT/**
  </DefaultItemExcludes>
</PropertyGroup>
```

**For older versions of godot** everything in `addons/SGT/Bridge/GUI` can be simply deleted because it only serves as GUI and all core parts are written in pure C#. All you need to do to run the tests is to modify `addons/SGT/Bridge/GodotTestRoot.cs` to be compatible with your version of godot, and simply call `RunTestsInNamespaces(string[] namespaces)`. If you want to run tests for all namespaces you can get them via `AssemblyExtractor.GetAllTestNamespaces().ToArray()`, this function gets all namespaces with tests in your entire project.

Frequent Q/A
-----
Q: Why no bottom panel? \
A: Bottom panel requires using `[Tool]` attribute for class, and that [doesn't work for mono in 4.x](https://github.com/godotengine/godot/issues/78513)

Q: Any documentation/wiki? \
A: Work in progress