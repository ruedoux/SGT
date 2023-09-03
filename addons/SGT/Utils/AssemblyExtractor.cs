namespace SGT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


public static class AssemblyExtractor
{
  public static string defaultNamespaceName = "Default";

  public static List<object> GetTestObjectsInNamespace(
    string namespaceName)
  {
    var types = Assembly.GetExecutingAssembly()
      .GetTypes().Where(t => t.IsDefined(typeof(SimpleTestClass), false));
    List<object> testObjects = new();

    foreach (var type in types)
    {
      string typeNamespace = GetTypeNamespaceName(type);
      if (namespaceName != typeNamespace)
      {
        continue;
      }

      var instance = Activator.CreateInstance(type);
      if (ObjectHasTestMethod(instance))
      {
        testObjects.Add(instance);
      }
    }

    return testObjects;
  }

  public static List<string> GetAllTestNamespaces()
  {
    List<string> allNamespaces = new();
    var types = Assembly.GetExecutingAssembly()
      .GetTypes().Where(t => t.IsDefined(typeof(SimpleTestClass), false));

    foreach (var type in types)
    {
      var namespaceName = GetTypeNamespaceName(type);
      if (!allNamespaces.Contains(namespaceName))
      {
        allNamespaces.Add(namespaceName);
      }
    }

    return allNamespaces;
  }

  private static bool ObjectHasTestMethod(object instance)
  {
    var methods = instance.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
    foreach (var method in methods)
    {
      if (Attribute.IsDefined(method, typeof(SimpleTestMethod)))
      {
        return true;
      }
    }
    return false;
  }

  private static string GetTypeNamespaceName(Type type)
  {
    string namespaceName = defaultNamespaceName;
    if (type.FullName.Contains('.'))
    {
      var dotIndex = type.FullName.IndexOf('.');
      namespaceName = type.FullName[..dotIndex];
    }
    return namespaceName;
  }
}