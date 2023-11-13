namespace SerializationExample.Serialization;

public interface ISerializable
{
    string[] ToCSV();

    void FromCSV(string[] values);
}