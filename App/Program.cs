using PdfLib;
using System;
using System.Threading.Tasks;

namespace App
{
    class Program
    {
        static void Main()
        {
            MainAsync().GetAwaiter().GetResult();
        }

        static async Task MainAsync()
        {
            var result = await PdfSigner.Sign(null, null);
            Console.WriteLine(result);
        }
    }
}