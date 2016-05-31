using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web;

namespace SimpleCMS.Web
{
    public class Json
    {
        public static string Serialize<T>(MediaTypeFormatter formatter, T value)
        {
            using (Stream stream = new MemoryStream())
            {
                var content = new StreamContent(stream);
                /// Serialize the object.
                formatter.WriteToStreamAsync(typeof(T), value, stream, content, null).Wait();
                // Read the serialized string.
                stream.Position = 0;
                return content.ReadAsStringAsync().Result;
            }
        }
        
        public static T Deserialize<T>(MediaTypeFormatter formatter, string str) where T : class
        {
            // Write the serialized string to a memory stream.
            using (Stream stream = new MemoryStream())
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(str);
                    writer.Flush();
                    stream.Position = 0;
                    // Deserialize to an object of type T
                    return formatter.ReadFromStreamAsync(typeof(T), stream, null, null).Result as T;
                }
            }
        }
    }
}