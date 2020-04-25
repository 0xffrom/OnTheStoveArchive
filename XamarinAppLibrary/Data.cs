using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace XamarinAppLibrary
{
    public static class Data
    {
        public static T ByteArrayToObject<T>(byte[] arrBytes)
        {


            BinaryFormatter binaryFormatter = new BinaryFormatter();

            using MemoryStream memoryStream = new MemoryStream(arrBytes);

            return (T)binaryFormatter.Deserialize(memoryStream);

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