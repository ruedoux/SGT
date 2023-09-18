using System;
using System.Collections.Generic;
namespace SGT;

[Serializable]
public class MessageAgregator
{
  public List<Message> messages = new();

  public void AddMessage(Message message) => messages.Add(message);
}