namespace SGT;
using System;

public class AssertionException : Exception
{
  public AssertionException(string message)
       : base(message)
  {
  }
}
