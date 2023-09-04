namespace SGT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


internal static class AssemblyExtractor
{
  public static string defaultNamespaceName = "Default";

  public static List<SimpleTestClass> GetTestObjectsInNamespace(
    string namespaceName)
  {
    List<SimpleTestClass> testObjects = new();

    foreach (var type in GetTestClassTypesFromAssembly())
    {
      string typeNamespace = GetTypeNamespaceName(type);
      if (namespaceName != typeNamespace)
      {
        continue;
      }

      var instance = (SimpleTestClass)Activator.CreateInstance(type);
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

    foreach (var type in GetTestClassTypesFromAssembly())
    {
      var namespaceName = GetTypeNamespaceName(type);
      if (!allNamespaces.Contains(namespaceName))
      {
        allNamespaces.Add(namespaceName);
      }
    }

    return allNamespaces;
  }

  private static IEnumerable<Type> GetTestClassTypesFromAssembly()
  {
    return Assembly.GetExecutingAssembly()
      .GetTypes().Where(type =>
        type.IsClass &&
        !type.IsAbstract &&
        typeof(SimpleTestClass).IsAssignableFrom(type));
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