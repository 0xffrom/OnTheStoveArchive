using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace AndroidLibrary
{
    public static class DataContext
    {
        public static T ByteArrayToObject<T>(byte[] arrBytes)
        {
            var binaryFormatter = new BinaryFormatter();

            using var memoryStream = new MemoryStream(arrBytes);

            return (T) binaryFormatter.Deserialize(memoryStream);
        }

        public static byte[] RecipeToByteArray(object obj)
        {
            if (obj == null)
                return null;

            using MemoryStream memoryStream = new MemoryStream();

            BinaryFormatter binaryFormatter = new BinaryFormatter();

            binaryFormatter.Serialize(memoryStream, obj);

            return memoryStream.ToArray();
        }
    }
}