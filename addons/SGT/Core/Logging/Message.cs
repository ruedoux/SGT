namespace SGT;
using System;

public class Message
{
  public enum TYPE
  {
    NORMAL,
    INFO,
    SUCCESS,
    WARNING,
    ERROR
  }

  public enum SUIT
  {
    INFORMATION,
    START_OF_SUIT,
    ERROR_INFORMATION,
    METHOD_RESULT,
    CLASS_RESULT,
    NAMESPACE_RESULT,
    ALL_TEST_RESULT
  }

  private readonly string text = String.Empty;
  private readonly string messagePrefix;
  private string indentaion;
  private readonly TYPE messageType;
  private readonly SUIT messageSuit;

  private Message() { }

  public Message(
    TYPE messageType,
    SUIT messageSuit,
    string messagePrefix,
    params object[] msgs)
  {
    foreach (var msg in msgs)
    {
      text += msg.ToString();
    }
    this.messageType = messageType;
    this.messageSuit = messageSuit;
    this.messagePrefix = messagePrefix;
  }

  internal void SetIndentation(uint tabs)
  {
    for (uint i = 0; i < tabs; i++)
    {
      indentaion += '\t';
    }
  }

  public string GetWhole() => indentaion + messagePrefix + text;
  public string GetPrefix() => messagePrefix;
  public string GetText() => text;
  public string GetIndentation() => indentaion;
  public TYPE GetMessageType() => messageType;
  public SUIT GetMessageSuit() => messageSuit;

  public static Message GetNormal(
    string messagePrefix, SUIT messageSuit, params object[] text)
    => new(TYPE.NORMAL, messageSuit, messagePrefix, text);
  public static Message GetInfo(
    string messagePrefix, SUIT messageSuit, params object[] text)
  => new(TYPE.INFO, messageSuit, messagePrefix, text);
  public static Message GetWarning(
    string messagePrefix, SUIT messageSuit, params object[] text)
    => new(TYPE.WARNING, messageSuit, messagePrefix, text);
  public static Message GetError(
    string messagePrefix, SUIT messageSuit, params object[] text)
    => new(TYPE.ERROR, messageSuit, messagePrefix, text);
  public static Message GetSuccess(
    string messagePrefix, SUIT messageSuit, params object[] text)
    => new(TYPE.SUCCESS, messageSuit, messagePrefix, text);
}