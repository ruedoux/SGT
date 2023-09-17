using System;
using System.Collections.Generic;
namespace SGT;

/// <summary> 
/// Converts a message flow into console friendly output
/// </summary>
public class MessagePrinter
{
  private readonly Action<string> printFunction;
  private readonly Dictionary<Message.Severity, string> SeverityColorMap = new();
  private readonly bool addBBCode;
  private bool shouldBeTabbed = false; // switched on/off based on namespace

  public MessagePrinter(Action<string> printFunction, bool addBBCode)
  {
    this.printFunction = printFunction;
    this.addBBCode = addBBCode;

    SeverityColorMap.Add(Message.Severity.PASSED, "Lawngreen");
    SeverityColorMap.Add(Message.Severity.FAILED, "Red");
  }

  public void Print(Message message)
  {
    if (message.suiteKind == Message.SuiteKind.NAMESPACE && message.suiteType == Message.SuiteType.END_SUITE)
      shouldBeTabbed = false;

    if (message.suiteType == Message.SuiteType.NONE)
    {
      PrintBasedOnTab($"{GetLeftBar(message.suiteType, message.severity)} {message.GetInfo()} <{message.GetTimeTook()}>{message.GetDetails(true)}");
    }
    else if (message.suiteType == Message.SuiteType.START_SUITE)
      StartSuite(message);
    else if (message.suiteType == Message.SuiteType.END_SUITE)
      EndSuite(message);

    if (message.suiteKind == Message.SuiteKind.NAMESPACE && message.suiteType == Message.SuiteType.START_SUITE)
      shouldBeTabbed = true;

  }

  private void PrintBasedOnTab(string message)
  {
    if (shouldBeTabbed)
      printFunction($"\t {message}");
    else
      printFunction(message);
  }

  private void StartSuite(Message message)
  {
    PrintBasedOnTab($"{GetLeftBar(message.suiteType, message.severity)} Starting {message.suiteKind.ToString().ToLower()} {message.GetInfo()}");
    if (message.suiteKind != Message.SuiteKind.CLASS)
    {
      printFunction("");
    }
  }

  private void EndSuite(Message message)
  {
    if (message.suiteKind == Message.SuiteKind.CLASS)
    {
      PrintBasedOnTab($"{GetLeftBar(message.suiteType, message.severity)} Ending {message.suiteKind.ToString().ToLower()} {message.GetInfo()}");
    }
    PrintSummaryBlock(message);
    printFunction("");
  }

  private void PrintSummaryBlock(Message message)
  {
    PrintBasedOnTab($"{GetLeftBar(message.suiteType, message.severity)}");
    PrintBasedOnTab($"{GetLeftBar(Message.SuiteType.NONE, message.severity)} Summary of: {message.suiteKind.ToString().ToLower()} {message.GetInfo()}, took: {message.GetTimeTook()}");
    PrintBasedOnTab($"{GetLeftBar(message.suiteType, message.severity)}");
  }

  private string GetLeftBar(
    Message.SuiteType suiteType,
    Message.Severity severity,
    uint barSize = 8,
    char barFiller = '-')
  {
    string output = "";
    if (suiteType == Message.SuiteType.START_SUITE || suiteType == Message.SuiteType.END_SUITE)
      output += '[' + CenterString("", barFiller, barSize) + ']';
    else
      output += '[' + CenterString(severity.ToString(), barFiller, barSize) + ']';

    if (addBBCode && SeverityColorMap.ContainsKey(severity) && suiteType == Message.SuiteType.NONE)
    {
      output = AddBBCodeColor(output, SeverityColorMap[severity]);
    }

    return output;
  }

  private string CenterString(string inputString, char filler, uint maxSize)
  {
    if (inputString.Length > maxSize)
      inputString = inputString[..(int)maxSize];

    int totalSpaces = (int)maxSize - inputString.Length;
    int leftSpaces = totalSpaces / 2;
    int rightSpaces = totalSpaces - leftSpaces;

    return new string(filler, leftSpaces) + inputString + new string(filler, rightSpaces);
  }

  private string AddBBCodeColor(string text, string color)
  {
    if (string.IsNullOrEmpty(color))
      return text;
    return $"[color={color}]{text}[/color]";
  }
}