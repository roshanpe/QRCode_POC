using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace QR.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QRCodeController : ControllerBase
    {
        private readonly QRCodeService _qrCodeService;

        public QRCodeController(QRCodeService qrCodeService)
        {
            _qrCodeService = qrCodeService;
        }

        /// <summary>
        /// Generate a QR code from contact information
        /// </summary>
        /// <param name="name">Person's full name</param>
        /// <param name="email">Person's email address</param>
        /// <param name="dateOfBirth">Person's date of birth (yyyy-MM-dd format)</param>
        /// <returns>File path of the generated QR code image</returns>
        [HttpPost("generate")]
        [ProducesResponseType(typeof(QRCodeResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GenerateQRCode([FromQuery] string name, [FromQuery] string email, [FromQuery] string dateOfBirth)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(dateOfBirth))
            {
                return BadRequest("Name, email, and dateOfBirth are required.");
            }

            if (!DateTime.TryParse(dateOfBirth, out DateTime dob))
            {
                return BadRequest("Invalid date format. Please use yyyy-MM-dd format.");
            }

            try
            {
                string filePath = _qrCodeService.GenerateQRCode(name, email, dob);
                return Ok(new QRCodeResponse { FilePath = filePath, Message = "QR Code generated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error generating QR code", error = ex.Message });
            }
        }

        /// <summary>
        /// Generate a QR code as a byte array
        /// </summary>
        [HttpPost("generate-bytes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GenerateQRCodeBytes([FromQuery] string name, [FromQuery] string email, [FromQuery] string dateOfBirth)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(dateOfBirth))
            {
                return BadRequest("Name, email, and dateOfBirth are required.");
            }

            if (!DateTime.TryParse(dateOfBirth, out DateTime dob))
            {
                return BadRequest("Invalid date format. Please use yyyy-MM-dd format.");
            }

            try
            {
                byte[] qrCodeBytes = _qrCodeService.GenerateQRCodeBytes(name, email, dob);
                return File(qrCodeBytes, "image/png", $"QRCode_{DateTime.Now:yyyyMMddHHmmss}.png");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error generating QR code", error = ex.Message });
            }
        }

        /// <summary>
        /// Generate a VCard from contact information
        /// </summary>
        [HttpGet("vcard")]
        [ProducesResponseType(typeof(VCardResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GenerateVCard([FromQuery] string name, [FromQuery] string email, [FromQuery] string dateOfBirth)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(dateOfBirth))
            {
                return BadRequest("Name, email, and dateOfBirth are required.");
            }

            if (!DateTime.TryParse(dateOfBirth, out DateTime dob))
            {
                return BadRequest("Invalid date format. Please use yyyy-MM-dd format.");
            }

            string vCard = _qrCodeService.GenerateVCard(name, email, dob);
            return Ok(new VCardResponse { VCard = vCard });
        }
    }

    public class QRCodeResponse
    {
        public string FilePath { get; set; }
        public string Message { get; set; }
    }

    public class VCardResponse
    {
        public string VCard { get; set; }
    }
}
