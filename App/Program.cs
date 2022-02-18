using PdfLib.Signatures;
using System;
using System.IO;
using System.Threading.Tasks;

namespace App
{
    class Program
    {
        static void Main()
        {
            try
            {
                MainAsync().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        static async Task MainAsync()
        {
            var projectPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Javascript");

            var args = new PdfSignerArgs()
            {
                Reason = "reason",
                Location = "location",
                Name = "name",
                ContactInfo = "contact@contact.com",
                PdfBuffer = Properties.Resources.w3dummy,
                PdfPath = $"{Path.Combine(projectPath, "tempPdfFile.pdf")}",
                P12Buffer = Properties.Resources.cert,
                P12Path = $"{Path.Combine(projectPath, "tempP12File.p12")}",
                Passphrase = "12345678",
                OutputPath = $"{Path.Combine(projectPath, $"exported_file_{new Random().Next(5000)}.pdf")}",
                SignatureLength = 1612,
                Rect = new[] { 50, 50, 100, 100 },
            };

            var result = await PdfSigner.Sign(args);

            Console.WriteLine(result);
        }
    }
}