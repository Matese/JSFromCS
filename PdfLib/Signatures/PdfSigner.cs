using Jering.Javascript.NodeJS;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using PdfLib.IO;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace PdfLib.Signatures
{
    public class PdfSigner
    {
        private static void ProjectExtractTo(string projectPath)
        {
            if (!Directory.Exists(projectPath))
            {
                Directory.CreateDirectory(projectPath);

                var tempPath = Path.Combine(projectPath, "temp.zip");

                var resourceName = "PdfLib.Javascript.zip";

                FileHelper.CreateFile(typeof(PdfSigner).Assembly.GetManifestResourceStream(resourceName), tempPath);

                ZipFile.ExtractToDirectory(tempPath, projectPath);
                File.Delete(tempPath);
            }
        }

        private static string ProjectCallYarnInstall(string projectPath)
        {
            var command = $"\"cd /D {projectPath}\\node-signpdf && yarn install\"";

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

        public static async Task<string> Sign(PdfSignerArgs args)
        {
            var projectPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Javascript");
            var nodeJSService = Install(projectPath);

            try
            {
                args.CreateTempFiles();

                string result = await nodeJSService
                    .InvokeFromFileAsync<string>("./interop.js", args: new[] { JsonConvert.SerializeObject(args) })
                    .ConfigureAwait(false);

                return result;
            }
            finally
            {
                args.DeleteTempFiles();
            }
        }
    }
}
