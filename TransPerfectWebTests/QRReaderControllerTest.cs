using NUnit.Framework;
using TransPerfectWeb.Controllers;
using System.IO;
using System.Threading.Tasks;
using TransPerfectWebCommon.Models;
using System.Text.Json;
using System.Net.Http;

/// <summary>
/// Some basic tests quickly made to have a simpler testing time.
/// </summary>
namespace TransPerfectWeb.Tests
{
    [TestFixture]
    public class QRReaderControllerTest
    {
        private QRReaderController _controller;
        [SetUp]
        public void SetUp()
        {
            _controller = new QRReaderController(new HttpClient());
        }

        /// <summary>
        /// Expect the default to exist after downloading.
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task TestDefaultGetQRImage()
        {
            await _controller.GetQRImage();
            Assert.IsTrue(File.Exists("imageDirectory/image.jpg"));
            File.Delete("imageDirectory/image.jpg");
        }

        /// <summary>
        /// Using same QR URL but using different directory and image name, assert that the file will exist.
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task TestSpecifyPathGetQRImage()
        {
            await _controller.GetQRImage("http://api.qrserver.com/v1/create-qr-code/?data=HelloWorld", "differentDirectory", "differentImageName.jpg");
            Assert.IsTrue(File.Exists("differentDirectory/differentImageName.jpg"));
            File.Delete("differentDirectory/differentImageName.jpg");
        }

        /// <summary>
        /// Deserialize the message that comes out of the default and check if it equals HelloWorld. 
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task TestDefaultQRImageIsHelloWorld()
        {
            await _controller.GetQRImage();
            var responseString = await _controller.GetQRMessage();
            QRMessageObject[] obj = JsonSerializer.Deserialize<QRMessageObject[]>(responseString);
            Assert.IsTrue(obj[0].symbol[0].data.Equals("HelloWorld"));
            File.Delete("imageDirectory/image.jpg");
        }

        /// <summary>
        /// Specify different file path and then generate a random message from the original API, check if the string is as expected.
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task TestSuppliedQRImageGeneratedByWebsite()
        {
            await _controller.GetQRImage("http://api.qrserver.com/v1/create-qr-code/?data=my+name+is+corey", "differentDirectory", "differentImageName.jpg");
            var responseString = await _controller.GetQRMessage("differentDirectory", "differentImageName.jpg");
            QRMessageObject[] obj = JsonSerializer.Deserialize<QRMessageObject[]>(responseString);
            Assert.IsTrue(obj[0].symbol[0].data.Equals("my name is corey"));
            File.Delete("differentDirectory/differentImageName.jpg");
        }
    }
}
