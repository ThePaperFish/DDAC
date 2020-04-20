using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.Azure;

using System.Dynamic;

namespace DDAC2.Controllers
{
    public class BlobsController : Controller
    {
        private CloudBlobContainer GetCloudBlobContainer()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            IConfigurationRoot Configuration = builder.Build();
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                    Configuration["ConnectionStrings:AzureStorageConnectionString-1"]);

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("test-blob-container");
            return container;
        }

        public IActionResult Index()
        {
            return View();
        }



        public async Task<ActionResult> CreateBlobContainer()
        {
            CloudBlobContainer container = GetCloudBlobContainer();
            ViewBag.Success = await container.CreateIfNotExistsAsync();
            ViewBag.BlobContainerName = container.Name;
            return View();
        }


        public string UploadBlob()
        {
            //if (UploadBlobFunction("strawberry.png", "D:/downloads/strawberry.png"))
            //    return "success!";
            return "failed";
        }


        public bool UploadBlobFunction(String blobReference, Stream _stream)
        {
            try
            {
                CloudBlobContainer container = GetCloudBlobContainer();
                CloudBlockBlob blob = container.GetBlockBlobReference(blobReference);
                using (Stream stream = _stream)
                {
                    blob.UploadFromStreamAsync(stream).Wait();
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }


        public class Blob
        {
            public string Name { get; set; }
            public string Path { get; set; }
        }



        public ActionResult ListBlobs()
        {
            CloudBlobContainer container = GetCloudBlobContainer();
            List<Blob> blobs = new List<Blob>();

            BlobResultSegment resultSegment = container.ListBlobsSegmentedAsync(null).Result;
            foreach (IListBlobItem item in resultSegment.Results)
            {
                if (item.GetType() == typeof(CloudBlockBlob))
                {
                    CloudBlockBlob blob = (CloudBlockBlob)item;
                    blobs.Add(new Blob { Name = blob.Name, Path = item.Uri.ToString() });
                }
                else if (item.GetType() == typeof(CloudPageBlob))
                {
                    CloudPageBlob blob = (CloudPageBlob)item;
                    blobs.Add(new Blob { Name = blob.Name, Path = item.Uri.ToString() });
                }
                else if (item.GetType() == typeof(CloudBlobDirectory))
                {
                    CloudBlobDirectory dir = (CloudBlobDirectory)item;
                    blobs.Add( new Blob { Name = dir.Uri.ToString(), Path = "" });
                }
            }

            dynamic mymodel = new ExpandoObject();
            mymodel.blobs = blobs;

            return View(mymodel);
        }



        public string DownloadBlob()
        {
            if(DownloadBlobFunction("myBlobs", "./data/strawberry.png"))
                return "success!";
            return "failed";
        }



        public bool DownloadBlobFunction(String blobReference, String filePath)
        {
            try 
            { 
                CloudBlobContainer container = GetCloudBlobContainer();
                CloudBlockBlob blob = container.GetBlockBlobReference(blobReference);
                using (var fileStream = System.IO.File.OpenWrite(filePath))
                {
                    blob.DownloadToStreamAsync(fileStream).Wait();
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }



        public bool DeleteBlobFunction(String blobReference)
        {
            try
            {
                CloudBlobContainer container = GetCloudBlobContainer();
                CloudBlockBlob blob = container.GetBlockBlobReference(blobReference);
                blob.DeleteAsync();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }


        public string DeleteBlob()
        {
            if (DeleteBlobFunction("myBlobs"))
               return "success!";
            return "Failed";
        }
    }
}