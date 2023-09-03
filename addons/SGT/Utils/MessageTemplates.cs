using System.Xml.XPath;

namespace SGT;


public static class MessageTemplates
{

  public static string GetMethodResultMessage(
    bool result, string methodName, long timeTook)
      => $"[TEST {GetTestResultString(result)}] {methodName} | took: {timeTook}ms";

  public static string GetTimeoutMessage(
    string methodName, long timeoutTime)
      => $"[TEST TIMEOUT] {methodName} | Timeout time: {timeoutTime}";

  public static string GetTestResultString(bool result)
    => result ? "PASS" : "FAIL";
}