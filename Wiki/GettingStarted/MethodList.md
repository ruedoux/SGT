Method List
---

`Page is work in progress`

`!Important!`\
**Test root** refers to a node which is anchor point of all tests and provides access to the scene tree``

### Assertions

*All assertions implementation can be found [here](https://github.com/RedouxG/SGT/blob/main/addons/SGT/Core/Assertions.cs)*

---

### Utility

[[CallTestRootDeffered](#CallTestRootDeffered), [LoadSceneInstance](#LoadSceneInstance)]

*All utility methods implementation can be found [here](https://github.com/RedouxG/SGT/blob/main/addons/SGT/Core/Attributes.cs)*

---

<a name="CallDeffered">**CallTestRootDeffered**</a>\
Calls a given method in main thread for test root node.

```c#
CallDeffered(Node.MethodName.AddChild, loadedNode);
// Adds child to root test node. All nodes are cleared after the test method is finished
```

<a name="LoadSceneInstance<T>(string path)">**LoadSceneInstance**</a>\
Loads an instance of a scene from a given path.

```c#
var loadedNode = LoadSceneInstance<TestNode>(
      "res://Examples/Default/Scenes/TestNode.tscn");
```