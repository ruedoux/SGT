namespace SGT;

using System;
using System.Diagnostics;

internal abstract class RunnerTemplate
{
  private readonly Stopwatch stopwatch = new();
  public readonly GodotTestRoot godotTestRoot;
  protected Logger logger;

  public RunnerTemplate(GodotTestRoot godotTestRoot, Logger logger)
  {
    this.godotTestRoot = godotTestRoot;
    this.logger = logger;
  }

  public abstract bool Run();

  protected bool RunBlockWithLog(string name, Func<bool> action)
  {
    stopwatch.Start();
    logger.StartBlock(MessageTemplates.GetStartMessage(name));
    bool isPassed = action();
    logger.EndBlock(MessageTemplates.GetResultMessage(name, GetTimeTookMs(), isPassed));
    stopwatch.Stop();

    return isPassed;
  }

  public long GetTimeTookMs() => stopwatch.ElapsedMilliseconds;
}