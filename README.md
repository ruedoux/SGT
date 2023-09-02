Simple Godot Tests
=========

About
-----

Addon / Library for [Godot](https://github.com/godotengine/godot) that lets you create tests for your C# scripts. Its designed to be as simple to use as possible and have as little dependency issues with different Godot versions.

The library is designed mainly thinking about integration tests. (Testing if function x for class y does what you want it to do after providing z)

For example usage look [here](https://github.com/RedouxG/SGT/tree/main/Examples).

You can also easily add your own assertions and test methods easily. Any library or method that throws an exception will be caught by [Runner](https://github.com/RedouxG/SGT/blob/main/addons/SGT/Core/Runner.cs).

How to use
-----

Add `[SimpleTestClass]` attribute to your testing class and `[SimpleTestMethod]` attribute to methods in the test class you want to be tested.

Run `EdditorRunner.tscn` provided in `addons/SGT`. Thats it, no additional setup is needed for it to work.

Example test class would look something like this:

```cs
[SimpleTestClass]
public class ExampleAssertionTest
{

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

For the time being test output is being shown in the console, I'm planning to integrate it in some prettier way, probably something resembling GUT.

Mocking
----

The library works well with [Moq](https://github.com/moq/moq) as far as I have tested. You can use mocks in all test methods without issue.

You can easily add Moq to your project using:
> `dotnet add package Moq --version <LATEST VERSION>`


Dev info
-----

Everything is as Godot/IDE agnostic as possible to minigate all of compatibility issues that other plugins tend to have. The only interface entry points with Godot are Logger and EditorRunner classes.

I would recommend to use this testing library along with [GUT](https://github.com/bitwes/Gut), Godot nodes have a lot of issues with mocking in C# so if you want to mock something its best to do it in GUT for the time being.

