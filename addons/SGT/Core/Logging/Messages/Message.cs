using System;
using System.Collections.Generic;

namespace SGT;

[Serializable]
public class Message
{
  public enum Severity { INFO, PASSED, FAILED, TIMEOUT }
  public enum SuiteType { ALL, METHOD, START_SUITE, END_SUITE }

  public readonly Severity severity;
  public readonly SuiteType suiteType;
  public readonly string text;
  public readonly long timeTook;
  public readonly string details;

  public Message(
    Severity severity,
    SuiteType suiteType,
    string text,
    long timeTook = -1,
    string details = "")
  {
    this.severity = severity;
    this.suiteType = suiteType;
    this.text = text;
    this.timeTook = timeTook;
    this.details = details;
  }

  public static Message GetInfo(string text, SuiteType suiteType)
    => new(Severity.INFO, suiteType, text);
  public static Message GetSuccess(string text, SuiteType suiteType)
    => new(Severity.PASSED, suiteType, text);
  public static Message GetFail(string text, SuiteType suiteType)
  => new(Severity.FAILED, suiteType, text);
  public static Message GetTimeout(string text, SuiteType suiteType)
    => new(Severity.TIMEOUT, suiteType, text);
}