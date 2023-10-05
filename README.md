Simple Godot Tests
=========

About
-----

Addon for [Godot](https://github.com/godotengine/godot) that allows you to easily create tests for your C# scripts. Its designed to be as simple to use as possible and have as little dependency issues with different Godot versions.

Wiki
---

All information needed to use this plugin is provided [here](https://github.com/RedouxG/SGT/tree/main/Wiki/Home.md).


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

Q/A
-----
Q: Why no bottom panel? \
A: Bottom panel requires using `[Tool]` attribute for class, and that [doesn't work for mono in 4.x](https://github.com/godotengine/godot/issues/78513)