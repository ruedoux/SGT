namespace SGT;


internal static class MessageTemplates
{
  private const string START_INDICATOR = "[START] ";
  private const string FAILED_INDICATOR = "[FAILED] ";
  private const string PASSED_INDICATOR = "[PASSED] ";
  private const string TIMEOUT_INDICATOR = "[TIMEOUT] ";

  public static Message GetStartMessage(string name)
  {
    string message = $"{name}";
    return Message.GetInfo(START_INDICATOR, message);
  }

  public static Message GetResultMessage(
      string name, long tookMs, bool isPassed)
  {
    string message = $"{name} | took: {tookMs}ms";
    return isPassed
      ? Message.GetSuccess(PASSED_INDICATOR, message)
      : Message.GetError(FAILED_INDICATOR, message);
  }

  public static Message GetTimeoutMessage(string name)
    => Message.GetError(
      TIMEOUT_INDICATOR, $" {name} | timeout: {Config.testTimeoutTimeMs}ms");

  public static string ListToString(string[] strings)
  {
    string result = "";

    foreach (string str in strings)
    {
      result += str + ", ";
    }

    return result[..^2];
  }
}