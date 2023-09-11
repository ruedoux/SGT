namespace SGT;


internal static class MessageTemplates
{
  private const string START_INDICATOR = "[START]";
  private const string FAILED_INDICATOR = "[FAILED]";
  private const string PASSED_INDICATOR = "[PASSED]";
  private const string TIMEOUT_INDICATOR = "[TIMEOUT]";
  private const string SKIPPED_INDICATOR = "[SKIPPED]";

  public static Message GetTestResultMessage(
    TestMethod methodResult, bool isPassed)
  {
    string message = $"{methodResult.methodInfo.Name} | took: {methodResult.stopwatch.ElapsedMilliseconds}ms";
    return isPassed
        ? Message.GetSuccess(
          PASSED_INDICATOR, Message.SUIT.SINGLE_TEST_RESULT, message)
        : Message.GetError(
          FAILED_INDICATOR, Message.SUIT.SINGLE_TEST_RESULT, message);
  }

  public static Message GetTestTimeoutMessage(TestMethod testMethod)
    => Message.GetError(
      TIMEOUT_INDICATOR,
      Message.SUIT.SINGLE_TEST_RESULT,
      $" {testMethod.methodInfo.Name} | timeout: {SGTConfig.testTimeoutTimeMs}ms");

  public static Message GetSkipEmptyClass(string className)
    => Message.GetWarning(
      SKIPPED_INDICATOR,
      Message.SUIT.INFORMATION,
      $" Skipped empty test class: {className}, no test methods found.");

  public static Message GetRunClass(string className)
    => Message.GetInfo(
      START_INDICATOR,
      Message.SUIT.START_OF_SUIT,
      $" Running class: {className}");

  public static Message GetSuitResultMessage(
    string className, long timeTook, bool isPassed)
  {
    string message = $" {className} | took: {timeTook}ms";
    return isPassed
        ? Message.GetSuccess(
          PASSED_INDICATOR, Message.SUIT.END_OF_SUIT, message)
        : Message.GetError(
          FAILED_INDICATOR, Message.SUIT.END_OF_SUIT, message);
  }

  public static Message GetRunNamespace(string namespaceName)
    => Message.GetInfo(
      START_INDICATOR,
      Message.SUIT.START_OF_SUIT,
      $" Running namespace: {namespaceName}");

  public static Message GetRunAll(string[] namespaces)
    => Message.GetInfo(START_INDICATOR,
    Message.SUIT.START_OF_SUIT,
    $" Starting tests: {ListToString(namespaces)}");

  public static Message GetEndSuccessAll(long timeTook)
    => Message.GetSuccess(
      PASSED_INDICATOR,
      Message.SUIT.END_OF_SUIT,
      $" All tests | took: {timeTook}ms");

  public static Message GetEndFailedAll(long timeTook)
    => Message.GetError(
      FAILED_INDICATOR,
      Message.SUIT.END_OF_SUIT,
      $" All tests | took: {timeTook}ms");

  private static string ListToString(string[] strings)
  {
    string result = "";

    foreach (string str in strings)
    {
      result += str + ", ";
    }

    return result[..^2];
  }
}