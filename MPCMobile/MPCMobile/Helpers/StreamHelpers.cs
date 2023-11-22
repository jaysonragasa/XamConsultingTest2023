using System;
using System.IO;

namespace MPCMobile.Helpers
{
    public class StreamHelpers
    {
        public static string ConvertStreamToBase64(Stream stream)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                byte[] bytes = memoryStream.ToArray();
                string base64String = Convert.ToBase64String(bytes);
                return base64String;
            }
        }
    }
}
