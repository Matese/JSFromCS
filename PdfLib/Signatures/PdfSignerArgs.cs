using Newtonsoft.Json;
using PdfLib.IO;
using System.IO;

namespace PdfLib.Signatures
{
    public class PdfSignerArgs
    {
        public string Reason { get; set; }
        public string Location { get; set; }
        public string Name { get; set; }
        public string ContactInfo { get; set; }
        public string PdfPath { get; set; }
        public string P12Path { get; set; }
        public string OutputPath { get; set; }
        public int SignatureLength { get; set; }
        public string Passphrase { get; set; }

        /// <summary>
        /// rect [x, y, width, height]
        /// </summary>
        /// <remarks>
        /// Unlike a raster image, in which the origin is located at the top left corner,
        /// the PDF document's origin is (by default) located at the bottom left corner.
        /// </remarks>
        public int[] Rect { get; set; }

        [JsonIgnore] public byte[] PdfBuffer { get; set; }

        [JsonIgnore] public byte[] P12Buffer { get; set; }

        public void CreateTempFiles()
        {
            FileHelper.CreateFile(this.PdfBuffer, this.PdfPath);
            FileHelper.CreateFile(this.P12Buffer, this.P12Path);
        }

        public void DeleteTempFiles()
        {
            File.Delete(this.PdfPath);
            File.Delete(this.P12Path);
        }
    }
}
