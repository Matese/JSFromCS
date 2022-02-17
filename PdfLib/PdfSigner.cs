using Jering.Javascript.NodeJS;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace PdfLib
{
    public class PdfSigner
    {
        private static void ProjectExtractTo(string projectPath)
        {
            if (!Directory.Exists(projectPath))
            {
                var tempPath = Path.Combine(projectPath, "temp.zip");
                var resourceName = "PdfLib.Javascript.zip";

                using (var inputStream = typeof(PdfSigner).Assembly.GetManifestResourceStream(resourceName))
                {
                    using (var fileStream = File.Create(tempPath))
                    {
                        inputStream.Seek(0, SeekOrigin.Begin);
                        inputStream.CopyTo(fileStream);
                    }
                }

                ZipFile.ExtractToDirectory(tempPath, projectPath);
                File.Delete(tempPath);
            }
        }

        private static string ProjectCallYarnInstall(string projectPath)
        {
            var command = $"\"cd /D {projectPath} && yarn install\"";

            var startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Verb = "runas",
                Arguments = "/C " + command,
                RedirectStandardOutput = true,
                UseShellExecute = false
            };

            var process = Process.Start(startInfo);
            process.WaitForExit();

            var output = process.StandardOutput.ReadToEnd();
            return output;
        }

        private static INodeJSService ProjectGetService(string projectPath)
        {
            var nodeJSService = new ServiceCollection()
                .AddNodeJS()
                .Configure<NodeJSProcessOptions>(options => options.ProjectPath = projectPath)
                .BuildServiceProvider()
                .GetRequiredService<INodeJSService>();

            return nodeJSService;
        }

        public static INodeJSService Install(string projectPath)
        {
            ProjectExtractTo(projectPath);
            ProjectCallYarnInstall(projectPath);
            return ProjectGetService(projectPath);
        }

        public static async Task<string> Sign(byte[] pdfBuffer, byte[] p12Buffer)
        {
            var projectPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Javascript");
            var nodeJSService = Install(projectPath);
            
            // Invoke from file
            string result = await nodeJSService
                .InvokeFromFileAsync<string>("./interop.js", args: new[] { "TODO" })
                .ConfigureAwait(false);

            return result;
        }
    }
}
