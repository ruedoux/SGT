using System;
using System.Collections.Generic;

namespace SGT;


internal static class ExceptionParser
{

  public static List<string> Parse(Exception ex, string className)
  {
    List<string> parsedExceptions = new()
    {
      ex.InnerException.Message
    };

    string[] lines = ex.InnerException.ToString()
      .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

    foreach (string line in lines)
    {

      parsedExceptions.Add(line);

    }

    return parsedExceptions;
  }
}