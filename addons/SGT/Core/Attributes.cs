namespace SGT;
using System;


[AttributeUsage(AttributeTargets.Class)]
public class SimpleTestClass : Attribute { }

[AttributeUsage(AttributeTargets.Method)]
public class SimpleTestMethod : Attribute { }

[AttributeUsage(AttributeTargets.Method)]
public class SimpleBeforeEach : Attribute { }

[AttributeUsage(AttributeTargets.Method)]
public class SimpleAfterEach : Attribute { }

[AttributeUsage(AttributeTargets.Method)]
public class SimpleBeforeAll : Attribute { }

[AttributeUsage(AttributeTargets.Method)]
public class SimpleAfterAll : Attribute { }