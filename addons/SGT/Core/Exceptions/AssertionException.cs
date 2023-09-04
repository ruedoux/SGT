namespace SGT;
using System;

internal class AssertionException : Exception
{
  public AssertionException(string message)
       : base(message)
  {
  }

  public AssertionException(string message, Exception ex)
       : base(message, ex)
  {
  }
}
