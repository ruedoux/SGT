namespace SGT;
using System.IO;
using System.Xml.Serialization;

public class ObjectSerializer<T>
{
  private readonly string filePath;

  public ObjectSerializer(string filePath)
  {
    this.filePath = filePath;
  }

  public void SaveToFile(T objectToSerialize)
  {
    if (File.Exists(filePath))
    {
      File.Delete(filePath);
    }

    XmlSerializer xmlSerializer = new(typeof(T));
    using var writer = new StreamWriter(filePath);
    xmlSerializer.Serialize(writer, objectToSerialize);
  }

  public T LoadFromFile()
  {
    if (!File.Exists(filePath))
    {
      throw new FileNotFoundException("Could not find file: " + filePath);
    }

    XmlSerializer xmlSerializer = new(typeof(T));
    T loadedObject;

    using (var reader = new StreamReader(Config.runnerConfigPath))
    {
      loadedObject = (T)xmlSerializer.Deserialize(reader);
    }

    return loadedObject;
  }
}