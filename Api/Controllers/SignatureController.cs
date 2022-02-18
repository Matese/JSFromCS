using Api.Model;
using Microsoft.AspNetCore.Mvc;
using PdfLib.Signatures;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [ApiController]
    [Route("Signatures")]
    public class SignatureController : ControllerBase
    {
        [HttpPost("")]
        public async Task<IActionResult> Post([FromBody] SignatureDTO dto)
        {
            try
            {
                byte[] pdf = Convert.FromBase64String(dto.PdfBuffer);
                byte[] p12 = Convert.FromBase64String(dto.P12Buffer);

                var projectPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Javascript");

                var args = new PdfSignerArgs()
                {
                    Reason = dto.Reason,
                    Location = dto.Location,
                    Name = dto.Name,
                    ContactInfo = dto.ContactInfo,
                    PdfBuffer = pdf,
                    PdfPath = $"{Path.Combine(projectPath, "tempPdfFile.pdf")}",
                    P12Buffer = p12,
                    P12Path = $"{Path.Combine(projectPath, "tempP12File.p12")}",
                    Passphrase = dto.Passphrase,
                    OutputPath = $"{Path.Combine(projectPath, $"exported_file_{new Random().Next(5000)}.pdf")}",
                    SignatureLength = dto.SignatureLength,
                    Rect = dto.Rect,
                };

                var result = await PdfSigner.Sign(args);

                return Ok($"{result}");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}

