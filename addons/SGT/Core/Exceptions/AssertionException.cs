using System;

namespace SGT;

public class AssertionException : Exception
{
  public AssertionException(string message)
       : base(message)
  {
  }
}
