namespace FlueFlame.Core.Serialization;

public interface ISerializer
{
	T DeserializeObject<T>(string response);
	string SerializeObject(object value);
}