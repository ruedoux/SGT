using System;
using System.Collections.Generic;
namespace SGT;

/// <summary> 
/// Converts a message flow into console friendly printFunction
/// </summary>
internal class MessagePrinter
{
  private static readonly Dictionary<Message.Severity, string> SeverityColorMap = new()
  {
    {Message.Severity.PASSED, "Lawngreen"},
    {Message.Severity.FAILED, "Red"},
    {Message.Severity.TIMEOUT, "Red"},
    {Message.Severity.INFO, "Skyblue"}
  };

  private readonly Action<string> printFunction;
  private readonly bool addBBCode;
  private bool shouldBeTabbed = false; // switched on/off based on namespace


  public MessagePrinter(Action<string> printFunction, bool addBBCode)
  {
    this.printFunction = printFunction;
    this.addBBCode = addBBCode;
  }

  public void Print(Message message)
  {
    if (message.suiteKind == Message.SuiteKind.NAMESPACE && message.suiteType == Message.SuiteType.END_SUITE)
      shouldBeTabbed = false;

    if (message.suiteType == Message.SuiteType.STAY)
      PrintBasedOnTab($"{GetLeftBar(message.suiteType, message.severity)} {message.GetInfo()} {message.GetTimeTook()}{message.GetDetails(true)}");
    else if (message.suiteType == Message.SuiteType.START_SUITE)
    {
      PrintBasedOnTab($"{GetLeftBar(message.suiteType, message.severity)} Starting {message.suiteKind.ToString().ToLower()} {message.GetInfo()}");
      if (message.suiteKind != Message.SuiteKind.CLASS)
        printFunction("");
    }
    else if (message.suiteType == Message.SuiteType.END_SUITE)
    {
      if (message.suiteKind == Message.SuiteKind.CLASS)
        PrintBasedOnTab($"{GetLeftBar(message.suiteType, message.severity)} Ending {message.suiteKind.ToString().ToLower()} {message.GetInfo()}");

      PrintSummaryBlock(message);
      printFunction("");
    }

    if (message.suiteKind == Message.SuiteKind.NAMESPACE && message.suiteType == Message.SuiteType.START_SUITE)
      shouldBeTabbed = true;
  }

  private void PrintBasedOnTab(string message)
    => printFunction(shouldBeTabbed ? $"\t {message}" : message);


  private void PrintSummaryBlock(Message message)
  {
    PrintBasedOnTab($"{GetLeftBar(message.suiteType, message.severity)}");
    PrintBasedOnTab($"{GetLeftBar(Message.SuiteType.STAY, message.severity)} Summary of: {message.suiteKind.ToString().ToLower()} {message.GetInfo()}, took: {message.GetTimeTook()}");
    PrintBasedOnTab($"{GetLeftBar(message.suiteType, message.severity)}");
  }

  private string GetLeftBar(
    Message.SuiteType suiteType,
    Message.Severity severity,
    uint barSize = 8,
    char barFiller = '-')
  {
    string leftBar = "";
    if (suiteType == Message.SuiteType.START_SUITE || suiteType == Message.SuiteType.END_SUITE)
      leftBar += $"[{PadMiddle("", barFiller, barSize)}]";
    else
      leftBar += $"[{PadMiddle(severity.ToString(), barFiller, barSize)}]";

    if (suiteType == Message.SuiteType.STAY && addBBCode)
      leftBar = AddBBCodeColor(leftBar, SeverityColorMap[severity]);

    return leftBar;
  }

  private string PadMiddle(string inputString, char filler, uint maxSize)
  {
    if (inputString.Length > maxSize)
      inputString = inputString[..(int)maxSize];

    int totalSpaces = (int)maxSize - inputString.Length;
    int leftSpaces = totalSpaces / 2;
    int rightSpaces = totalSpaces - leftSpaces;

    return $"{new string(filler, leftSpaces)}{inputString}{new string(filler, rightSpaces)}";
  }

  private string AddBBCodeColor(string text, string color) =>
    string.IsNullOrEmpty(color) ? text : $"[color={color}]{text}[/color]";
}