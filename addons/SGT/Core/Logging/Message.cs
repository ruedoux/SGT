using System;
using System.Collections.Generic;

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

  private readonly string text = String.Empty;
  private readonly string messagePrefix;
  private string indentaion;
  private readonly TYPE messageType;

  private Message() { }

  public Message(TYPE messageType, string messagePrefix, params object[] msgs)
  {
    foreach (var msg in msgs)
    {
      text += msg.ToString();
    }
    this.messageType = messageType;
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

  public static Message GetNormal(string messagePrefix, params object[] text)
    => new(TYPE.NORMAL, messagePrefix, text);
  public static Message GetInfo(string messagePrefix, params object[] text)
  => new(TYPE.INFO, messagePrefix, text);
  public static Message GetWarning(string messagePrefix, params object[] text)
    => new(TYPE.WARNING, messagePrefix, text);
  public static Message GetError(string messagePrefix, params object[] text)
    => new(TYPE.ERROR, messagePrefix, text);
  public static Message GetSuccess(string messagePrefix, params object[] text)
    => new(TYPE.SUCCESS, messagePrefix, text);
}