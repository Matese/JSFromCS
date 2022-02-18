using System.IO;

namespace PdfLib.IO
{
    public static class FileHelper
    {
        public static void CreateFile(Stream stream, string path)
        {
            var dir = Path.GetDirectoryName(path);

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            using (var fileStream = File.Create(path))
            {
                stream.Seek(0, SeekOrigin.Begin);
                stream.CopyTo(fileStream);
            }

            stream.Dispose();
        }

        public static void CreateFile(byte[] buffer, string path)
        {
            CreateFile(new MemoryStream(buffer), path);
        }
    }
}
