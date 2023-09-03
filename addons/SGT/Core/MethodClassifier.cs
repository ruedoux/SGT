namespace SGT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public static class MethodClassifier
{
  public static MethodInfo GetSingleAttributeMethod<T>(
    MethodInfo[] methods, object testObject)
    where T : Attribute
  {
    var methodInfos = GetAllAttributeMethods<T>(methods);
    if (methodInfos.Count > 1)
    {
      throw new TestSetupException(
        $"Could not setup test! {testObject.GetType().Name} has more than one attribute method: {typeof(T).FullName}.");
    }

    return methodInfos.LastOrDefault();
  }


  public static List<MethodInfo> GetAllAttributeMethods<T>(MethodInfo[] methods)
    where T : Attribute
  {
    List<MethodInfo> methodInfos = new();

    foreach (var method in methods)
    {
      if (Attribute.IsDefined(method, typeof(T)))
      {
        methodInfos.Add(method);
      }
    }

    return methodInfos;
  }
}