Method List
---

`!Important!`\
**Test root** refers to a node which is anchor point of all tests and provides access to the scene tree``

### Assertions

*All assertions implementation can be found [here](https://github.com/RedouxG/SGT/blob/main/addons/SGT/Core/Assertions.cs)*

---

### Utility

[[CallTestRootDeffered](#CallTestRootDeffered), ]

*All utility methods implementation can be found [here](https://github.com/RedouxG/SGT/blob/main/addons/SGT/Core/Attributes.cs)*

---

<a name="CallTestRootDeffered">**CallTestRootDeffered**</a>\
Calls deffered a method of a given string name with provided parameters for test root node.

```c#
CallTestRootDeffered(Node.MethodName.AddChild, loadedNode);
// Adds child to root test node. All nodes are cleared after the test method is finished
```

<a name="LoadSceneInstance<T>(string path)">**LoadSceneInstance**</a>\
Loads an instance of a scene from a given path.

```c#
var loadedNode = LoadSceneInstance<TestNode>(
      "res://Examples/Default/Scenes/TestNode.tscn");
```