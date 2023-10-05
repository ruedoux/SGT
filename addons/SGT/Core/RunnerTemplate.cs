namespace SGT;

using System;
using System.Diagnostics;
using System.Threading.Tasks;

public abstract class RunnerTemplate
{
  internal readonly GodotTestRoot godotTestRoot;
  internal Logger logger;

  internal RunnerTemplate(GodotTestRoot godotTestRoot, Logger logger)
  {
    this.godotTestRoot = godotTestRoot;
    this.logger = logger;
  }

  protected bool RunSuiteWithLog(
    string name, Message.SuiteKind suiteKind, Func<bool> action)
  {
    var stopwatch = Stopwatch.StartNew();

    logger.Log(new Message(
      Message.Severity.INFO, Message.SuiteType.START_SUITE, suiteKind, name));
    bool isPassed = action();

    Message.Severity severity = isPassed
      ? Message.Severity.PASSED
      : Message.Severity.FAILED;

    logger.Log(new Message(
      severity, Message.SuiteType.END_SUITE, suiteKind, name, stopwatch.ElapsedMilliseconds));

    return isPassed;
  }
}