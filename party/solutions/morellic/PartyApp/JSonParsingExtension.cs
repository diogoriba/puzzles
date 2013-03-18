using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Json;
using System.IO;

namespace ChallengeParty
{
    public static class JSonParsingExtension
    {
        public static string ToJson(this Tree json)
        {
            var serializer = new DataContractJsonSerializer(json.GetType());
            byte[] dataBytes;

            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, json);
                long length = stream.Length;

                dataBytes = new byte[length];
                stream.Position = 0;
                stream.Read(dataBytes, 0, (int)length);
            }

            return Encoding.UTF8.GetString(dataBytes);
        }

        public static Tree FromJson(this string json)
        {
            return json.FromJson<Tree>();
        }

        public static T FromJson<T>(this string json) where T : Tree
        {
            using (var mStream = new MemoryStream(Encoding.Unicode.GetBytes(json)))
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                return (T)serializer.ReadObject(mStream);
            }
        }
    }
}
