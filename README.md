Simple Godot Tests
=========

About
-----

Addon for [Godot](https://github.com/godotengine/godot) that allows you to easily create tests for your C# scripts. Its designed to be as simple to use as possible and have as little dependency issues with different Godot versions.

I would recommend to use this testing addon along with [GUT](https://github.com/bitwes/Gut), Godot nodes have a lot of issues with mocking in C# so if you want to mock something its best to do it in GUT for the time being.

How to use
-----

Add `[SimpleTestClass]` attribute to your testing class and `[SimpleTestMethod]` attribute to test methods.

Run `EdditorRunner.tscn` provided in `addons/SGT`. Thats it, no additional setup is needed for it to work.

Example test class would look something like this:

```cs
[SimpleTestClass]
public class ExampleAssertionTest
{
  [SimpleBeforeEach]
  public void BeforeEach()
  {
    // Setup
  }

  [SimpleAfterEach]
  public void AfterEach()
  {
    // Clean
  }

  [SimpleTestMethod]
  public void SomeTestCase()
  {
    // Given
    int a = 1;
    int b = 2;
    int expectedResult = 3;

    // When
    int result = a + b;

    // Then
    Assertions.AssertEqual(expectedResult, result);
  }
}
```

Testing class can be in any `.cs` file in your project, I would recommend putting all tests in a test folder so its easier to exclude the testing files in your `.csproj` file from the compiled game like so:

```xml
<PropertyGroup>
  <DefaultItemExcludes Condition="'$(Configuration)' == 'ExportRelease'">
    $(DefaultItemExcludes);YourTestFoldername/**
  </DefaultItemExcludes>
</PropertyGroup>
```

Also the tests are ran by namespace so you can manually run only one namespace at a time if you want.

For the time being test output is being shown in the console, I'm planning to integrate it in some prettier way, probably something resembling GUT.

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

TODO:
- Add some visually pleasing output of the tests to the runner node
- Add plugin integration so the tests can be ran from a Godot IDE tab

