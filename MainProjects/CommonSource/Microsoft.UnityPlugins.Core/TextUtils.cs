using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Microsoft.UnityPlugins
{
    public class TextUtils
    {
        public static void WriteAllText(string fileName, string text)
        {
            // The intent here is actually to give a synchronous API, so hold the thread while writing the data
            Task.Run(async () =>
            {
                var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(fileName,
                    CreationCollisionOption.OpenIfExists);

                await FileIO.WriteTextAsync(file, text);
            }).Wait();
        }

        public static void WriteAllBytes(string fileName, byte[] bytes)
        {
            Task.Run(async () =>
            {
                var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(fileName,
                    CreationCollisionOption.OpenIfExists);

                await FileIO.WriteBytesAsync(file, bytes);
            }).Wait(); 
        }

        public static string ReadAllText(string fileName)
        {
            string allText = null;
            // The intent here is actually to give a synchronous API, so hold the thread while writing the data
            Task.Run(async () =>
            {
                var file = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);

                allText = await FileIO.ReadTextAsync(file);
            }).Wait();

            return allText;
        }

        public static byte[] ReadAllBytes(string fileName)
        {
            IBuffer allBytes = null;
            // The intent here is actually to give a synchronous API, so hold the thread while writing the data
            Task.Run(async () =>
            {
                var file = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);

                allBytes = await FileIO.ReadBufferAsync(file);
            }).Wait();

            string fileContent = DataReader.FromBuffer(allBytes).ReadString(allBytes.Length);

            byte[] bytes = new byte[fileContent.Length * sizeof(char)];
            System.Buffer.BlockCopy(fileContent.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
    }
}
