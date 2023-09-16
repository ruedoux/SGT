using System;
using Godot;

namespace SGT;


/// <summary> 
/// Converts a message into console friendly string
/// </summary>
public class MessagePrinter
{
  private readonly Action<string> printFunction;
  private uint indentation = 0;
  private bool addIndentationInNext = false;

  public MessagePrinter(Action<string> printFunction = null)
  {
    this.printFunction = printFunction;
  }

  public void Print(Message message)
  {
    if (printFunction == null) { return; }

    printFunction(GetAsString(message));
  }

  public string GetAsString(Message message, string bbcodeSeverityColor = "")
  {
    if (addIndentationInNext)
    {
      indentation++;
      addIndentationInNext = false;
    }

    UpdateIndentation(message);
    string output = new('\t', (int)indentation);

    if (message.severity != Message.Severity.INFO)
      output += $" [{AddBBCodeColor(message.severity.ToString(), bbcodeSeverityColor)}]";

    output += $" {message.text}";

    if (message.timeTook != -1)
      output += $" <{message.timeTook}ms>";

    return output;
  }

  private string AddBBCodeColor(string text, string color)
  {
    if (string.IsNullOrEmpty(color))
      return text;
    return $"[color={color}]{text}[/color]";
  }

  private void UpdateIndentation(Message message)
  {
    if (message.suiteType == Message.SuiteType.START_SUITE)
    {
      addIndentationInNext = true;
      return;
    }
    if (message.suiteType == Message.SuiteType.END_SUITE)
    {
      indentation--;
      return;
    }
  }
}