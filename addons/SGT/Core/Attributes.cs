namespace SGT;
using System;
using Godot;

[AttributeUsage(AttributeTargets.Class)]
internal class SimpleTestClass : Attribute { }

[AttributeUsage(AttributeTargets.Method)]
internal class SimpleTestMethod : Attribute
{
  public uint repeatTest = 1;
  public Node godotNode;
}

[AttributeUsage(AttributeTargets.Method)]
internal class SimpleBeforeEach : Attribute { }

[AttributeUsage(AttributeTargets.Method)]
internal class SimpleAfterEach : Attribute { }

[AttributeUsage(AttributeTargets.Method)]
internal class SimpleBeforeAll : Attribute { }

[AttributeUsage(AttributeTargets.Method)]
internal class SimpleAfterAll : Attribute { }