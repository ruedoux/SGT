using System.Collections.Generic;
using System.Xml;

namespace SGT;

internal class AgregatorXML
{
  private readonly List<Message> aggregatedMessages = new();

  public void QueueMessage(Message message)
  {
    aggregatedMessages.Add(message);
  }

  public void SaveToFile(string filePath)
  {
    using XmlWriter writer = XmlWriter.Create(filePath);


    writer.Flush();
  }
}