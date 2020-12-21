using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using System;
using System.Net.Http.Headers;

namespace TransPerfectWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QRReaderController : ControllerBase
    {
        private readonly ILogger<QRReaderController> _logger;
        private HttpClient _client;
        private const string POSTURL = "http://api.qrserver.com/v1/read-qr-code/";

        /// <summary>
        /// Controller with HttpClient injected.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="client"></param>
        public QRReaderController(HttpClient client, ILogger<QRReaderController> logger = null)
        { 
            _client = client;
            _logger = logger;
        }

        /// <summary>
        /// Function that will save the QR (given URI) into a default directory of repo/imageDirectory/image.jpg
        /// Just for testing on my end. Returns image in browser.
        /// Default string is the Hello World QR code given as an example here: http://goqr.me/api/doc/create-qr-code/ 
        /// </summary>
        /// <param name="uriString"></param>
        /// <param name="directoryPath"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetQRImage")]
        public async Task<PhysicalFileResult> GetQRImage(string uriString = "http://api.qrserver.com/v1/create-qr-code/?data=HelloWorld", string directoryPath = "imageDirectory", string fileNameWithExtension = "image.jpg")
        {
            Uri uri = new Uri(uriString);

            // Create file path and ensure directory exists
            var path = Path.Combine(directoryPath, $"{fileNameWithExtension}");
            try
            {
                var responseString = await _client.GetAsync(uri);
                Directory.CreateDirectory(directoryPath);

                // Download the image and write to the file
                var imageBytes = await _client.GetByteArrayAsync(uri);
                await System.IO.File.WriteAllBytesAsync(path, imageBytes);
            } catch (Exception e)
            {
                _logger.LogError("Error with downloading image: " + e);
            }

            return PhysicalFile(Path.GetFullPath(path), "image/jpg");
        }

        /// <summary>
        /// Retrieves the image generated. You can specify the fileNameWithExtension, else it just defaults to what was generated in GetQRImage.
        /// For proof of concept you can run GetQRImage and GetQRMessage without any parameters specified.
        /// Otherwise specify.
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <param name="fileNameWithExtension"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetQRMessage")]
        public async Task<string> GetQRMessage(string directoryPath = "imageDirectory", string fileNameWithExtension = "image.jpg")
        {
            var path = Path.Combine(directoryPath, $"{fileNameWithExtension}");

            var requestContent = new MultipartFormDataContent();
            var imageContent = new ByteArrayContent(System.IO.File.ReadAllBytes(Path.GetFullPath(path)));
            imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpg");

            requestContent.Add(imageContent, "file", fileNameWithExtension);

            return await _client.PostAsync(POSTURL, requestContent).Result.Content.ReadAsStringAsync();
        }
    }
}
