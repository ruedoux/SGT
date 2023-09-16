namespace SGT;

using System;
using System.Diagnostics;

internal abstract class RunnerTemplate
{
  private Stopwatch stopwatch;
  public readonly GodotTestRoot godotTestRoot;
  protected Logger logger;

  public RunnerTemplate(GodotTestRoot godotTestRoot, Logger logger)
  {
    this.godotTestRoot = godotTestRoot;
    this.logger = logger;
  }

  public abstract bool Run();

  protected bool RunSuiteWithLog(
    string name, Func<bool> action)
  {
    stopwatch = Stopwatch.StartNew();

    logger.Log(new Message(
      Message.Severity.INFO, Message.SuiteType.START_SUITE, name));
    bool isPassed = action();

    Message.Severity severity = isPassed
      ? Message.Severity.PASSED
      : Message.Severity.FAILED;

    logger.Log(new Message(
      severity, Message.SuiteType.END_SUITE, name, GetTimeTookMs()));

    return isPassed;
  }

  public long GetTimeTookMs() => stopwatch.ElapsedMilliseconds;
}