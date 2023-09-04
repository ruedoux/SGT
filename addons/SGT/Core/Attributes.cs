namespace SGT;
using System;


[AttributeUsage(AttributeTargets.Class)]
internal class SimpleTestClass : Attribute { }

[AttributeUsage(AttributeTargets.Method)]
internal class SimpleTestMethod : Attribute
{
  public readonly uint repeatTest = 1;

  public SimpleTestMethod() { }

  public SimpleTestMethod(uint repeatTest)
  {
    this.repeatTest = repeatTest;
  }
}

[AttributeUsage(AttributeTargets.Method)]
internal class SimpleBeforeEach : Attribute { }

[AttributeUsage(AttributeTargets.Method)]
internal class SimpleAfterEach : Attribute { }

[AttributeUsage(AttributeTargets.Method)]
internal class SimpleBeforeAll : Attribute { }

[AttributeUsage(AttributeTargets.Method)]
internal class SimpleAfterAll : Attribute { }