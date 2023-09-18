namespace SGT;
using System.IO;
using System.Xml.Serialization;

public static class ObjectSerializer
{
  public static void SaveToFile<T>(string filePath, T objectToSerialize)
  {
    if (File.Exists(filePath))
    {
      File.Delete(filePath);
    }

    XmlSerializer xmlSerializer = new(typeof(T));
    using var writer = new StreamWriter(filePath);
    xmlSerializer.Serialize(writer, objectToSerialize);
  }

  public static T LoadFromFile<T>(string filePath)
  {
    if (!File.Exists(filePath))
    {
      throw new FileNotFoundException("Could not find file: " + filePath);
    }

    XmlSerializer xmlSerializer = new(typeof(T));
    T loadedObject;

    using (var reader = new StreamReader(filePath))
    {
      loadedObject = (T)xmlSerializer.Deserialize(reader);
    }

    return loadedObject;
  }
}