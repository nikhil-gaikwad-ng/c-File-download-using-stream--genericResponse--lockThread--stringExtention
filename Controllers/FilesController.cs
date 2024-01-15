﻿using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Headers;
namespace Test.Controllers
{

    [ApiController]
    [Route("api/[controller]/[action]")]
    public class FilesController : ControllerBase
    {
        private readonly string filePath = @"Test.rar"; // Provide the actual path to your file

        [HttpGet("{fileName}")]
        public IActionResult GetFile(string fileName)
        {

            // Optimized dont take RAM space
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            var contentType = "application/octet-stream";
            var fileStreamResult = new FileStreamResult(fileStream, contentType)
            {
                FileDownloadName = fileName // Specify the name the client should use when saving the file
            };

            return fileStreamResult;


            //Not Optimized took RAM space
            //var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            //MemoryStream stream = new MemoryStream();
            //fileStream.CopyTo(stream);

            //var contentType = "application/octet-stream";
            //return File(stream.ToArray(), contentType, "asd");

        }


        [HttpGet("{name}")]
        public async Task<IActionResult> DownloadFile3rdParty(string name)
        {
            // Optimized dont take RAM space without await keyword
            HttpClient client = new HttpClient();
            Task<Stream> response =  client.GetStreamAsync("https://localhost:7071/api/Files/GetFile/1"); //without await

            var contentType = "application/octet-stream";

            var fileStreamResult = new FileStreamResult(response.Result, contentType)
            {
                FileDownloadName = name // Specify the name the client should use when saving the file
            };

            return fileStreamResult;


            // Optimized dont take RAM space with await
            //HttpClient client = new HttpClient();
            //Stream response = await client.GetStreamAsync("https://localhost:7071/api/Files/GetFile/1"); //with await

            //var contentType = "application/octet-stream";

            //var fileStreamResult = new FileStreamResult(response, contentType)
            //{
            //    FileDownloadName = name // Specify the name the client should use when saving the file
            //};

            //return fileStreamResult;


            //Not optimized took RAM space
            //HttpClient client = new HttpClient();
            //Task<Stream> response= client.GetStreamAsync("https://localhost:7071/api/Files/GetFile/1");
            //MemoryStream memory = new MemoryStream();
            //response.Result.CopyTo(memory);
            //memory.Position = 0;

            //var contentType = "application/octet-stream";

            //var fileStreamResult = new FileStreamResult(memory, contentType)
            //{
            //    FileDownloadName = name // Specify the name the client should use when saving the file
            //};

            //return fileStreamResult;
        }


    }


}
