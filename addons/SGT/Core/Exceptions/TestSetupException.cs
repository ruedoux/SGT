namespace SGT;
using System;

internal class TestSetupException : Exception
{
  public TestSetupException(string message)
       : base(message)
  {
  }

  public TestSetupException(string message, Exception ex)
       : base(message, ex)
  {
  }
}
