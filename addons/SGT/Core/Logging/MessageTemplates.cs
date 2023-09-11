namespace SGT;


internal static class MessageTemplates
{
  private const string START_INDICATOR = "[START]";
  private const string FAILED_INDICATOR = "[FAILED]";
  private const string PASSED_INDICATOR = "[PASSED]";
  private const string TIMEOUT_INDICATOR = "[TIMEOUT]";
  private const string SKIPPED_INDICATOR = "[SKIPPED]";

  public static Message GetFailedTestMessage(string methodName, long timeTook)
    => Message.GetError(
      FAILED_INDICATOR,
      Message.SUIT.METHOD_RESULT,
      $" {methodName} | took: {timeTook}ms");

  public static Message GetPassedTestMessage(string methodName, long timeTook)
    => Message.GetSuccess(
      PASSED_INDICATOR,
      Message.SUIT.METHOD_RESULT,
      $" {methodName} | took: {timeTook}ms");

  public static Message GetTimeoutTestMessage(string methodName, long timeTook)
    => Message.GetError(
      TIMEOUT_INDICATOR,
      Message.SUIT.METHOD_RESULT,
      $" {methodName} | took: {timeTook}ms");

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

  public static Message GetEndSuccessClass(string className, long timeTook)
    => Message.GetSuccess(
      PASSED_INDICATOR,
      Message.SUIT.CLASS_RESULT,
      $" {className} | took: {timeTook}ms");

  public static Message GetEndFailedClass(string className, long timeTook)
    => Message.GetError(
      FAILED_INDICATOR,
      Message.SUIT.CLASS_RESULT,
      $" {className} | took: {timeTook}ms");

  public static Message GetRunNamespace(string namespaceName)
    => Message.GetInfo(
      START_INDICATOR,
      Message.SUIT.START_OF_SUIT,
      $" Running namespace: {namespaceName}");

  public static Message GetEndSuccessNamespace(string namespaceName, long timeTook)
    => Message.GetSuccess(
      PASSED_INDICATOR,
      Message.SUIT.NAMESPACE_RESULT,
      $" {namespaceName} | took: {timeTook}ms");

  public static Message GetEndFailedNamespace(string namespaceName, long timeTook)
    => Message.GetError(
      FAILED_INDICATOR,
      Message.SUIT.NAMESPACE_RESULT,
      $" {namespaceName} | took: {timeTook}ms");

  public static Message GetRunAll(string[] namespaces)
    => Message.GetInfo(START_INDICATOR,
    Message.SUIT.START_OF_SUIT,
    $" Starting tests: {ListToString(namespaces)}");

  public static Message GetEndSuccessAll(long timeTook)
    => Message.GetSuccess(
      PASSED_INDICATOR,
      Message.SUIT.ALL_TEST_RESULT,
      $" All tests | took: {timeTook}ms");

  public static Message GetEndFailedAll(long timeTook)
    => Message.GetError(
      FAILED_INDICATOR,
      Message.SUIT.ALL_TEST_RESULT,
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