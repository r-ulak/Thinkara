
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;
using Microsoft.WindowsAzure;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using DTO.Custom;

namespace AzureServices
{
    /// <summary>
    /// This class contains the Windows Azure Storage initialization and common functions.
    /// </summary>
    public class AzureStorage
    {
        //private static CloudStorageAccount StorageAccount = CloudStorageAccount.DevelopmentStorageAccount;
        private static CloudStorageAccount StorageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("azure.storageConnectionString"));

        public static CloudBlobClient BlobClient
        {
            get;
            private set;
        }
        public static CloudBlobContainer ProfileImagesContainer
        {
            get;
            private set;
        }
        public static CloudBlobContainer PartyImagesContainer
        {
            get;
            private set;
        }
        public static CloudBlobContainer AdsImagesContainer
        {
            get;
            private set;
        }

        public const string ProfileImageContainerName = "profilecontainer";
        public const string PartyImageContainerName = "partycontainer";
        public const string AdsImageContainerName = "adscontainer";

        /// <summary>
        /// Initialize Windows Azure Storage accounts and CORS settings.
        /// </summary>
        public static void InitializeAccountPropeties()
        {
            BlobClient = StorageAccount.CreateCloudBlobClient();

            InitializeCors(BlobClient);

            ProfileImagesContainer = BlobClient.GetContainerReference(AzureStorage.ProfileImageContainerName);
            ProfileImagesContainer.CreateIfNotExists(BlobContainerPublicAccessType.Container);

            PartyImagesContainer = BlobClient.GetContainerReference(AzureStorage.PartyImageContainerName);
            PartyImagesContainer.CreateIfNotExists(BlobContainerPublicAccessType.Container);

            AdsImagesContainer = BlobClient.GetContainerReference(AzureStorage.AdsImageContainerName);
            AdsImagesContainer.CreateIfNotExists(BlobContainerPublicAccessType.Container);
        }

        /// <summary>
        /// Initialize Windows Azure Storage CORS settings.
        /// </summary>
        /// <param name="blobClient">Windows Azure storage blob client</param>
        private static void InitializeCors(CloudBlobClient blobClient)
        {
            // CORS should be enabled once at service startup
            ServiceProperties blobServiceProperties = new ServiceProperties();

            // Nullifying un-needed properties so that we don't
            // override the existing ones
            blobServiceProperties.HourMetrics = null;
            blobServiceProperties.MinuteMetrics = null;
            blobServiceProperties.Logging = null;

            // Enable and Configure CORS
            ConfigureCors(blobServiceProperties);

            // Commit the CORS changes into the Service Properties
            blobClient.SetServiceProperties(blobServiceProperties);
        }

        /// <summary>
        /// Adds CORS rule to the service properties.
        /// </summary>
        /// <param name="serviceProperties">ServiceProperties</param>
        private static void ConfigureCors(ServiceProperties serviceProperties)
        {
            serviceProperties.Cors = new CorsProperties();
            serviceProperties.Cors.CorsRules.Add(new CorsRule()
            {
                AllowedHeaders = new List<string>() { "*" },
                AllowedMethods = CorsHttpMethods.Put | CorsHttpMethods.Get | CorsHttpMethods.Head | CorsHttpMethods.Post,
                AllowedOrigins = new List<string>() { "https://thinkara.com" ,"https://www.thinkara.com"},
                ExposedHeaders = new List<string>() { "*" },
                MaxAgeInSeconds = 1800 // 30 minutes
            });
        }
        public static string GetSasForBlob(SasUrlDTO sasurl)
        {
            CloudBlockBlob blob;
            if (sasurl.SourceType == "profile")
            {
                blob = AzureStorage.ProfileImagesContainer.GetBlockBlobReference(sasurl.BlobName);
            }
            else if (sasurl.SourceType == "ads")
            {
                blob = AzureStorage.AdsImagesContainer.GetBlockBlobReference(sasurl.BlobName);
            }
            else if (sasurl.SourceType == "partynew" || sasurl.SourceType == "partymanage")
            {
                blob = AzureStorage.PartyImagesContainer.GetBlockBlobReference(sasurl.BlobName);
            }
            else
            {
                return string.Empty;
            }

            if (blob == null)
            {
                throw new ArgumentNullException("blob can't be null");
            }
            var sas = blob.GetSharedAccessSignature(
         new SharedAccessBlobPolicy()
         {
             Permissions = SharedAccessBlobPermissions.Write | SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.List,
             SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(30),
         });
            return string.Format(CultureInfo.InvariantCulture, "{0}{1}", blob.Uri, sas);
        }
        public static void UploadAllFodlerContent(string folderPath)
        {
            string[] extensions = new[] { ".jpg", ".png" };
            DirectoryInfo directory = new DirectoryInfo(folderPath);
            FileInfo[] files =
                directory.GetFiles()
                    .Where(f => extensions.Contains(f.Extension.ToLower()))
                         .ToArray();

            foreach (var item in files)
            {
                CloudBlockBlob blockBlob = ProfileImagesContainer.GetBlockBlobReference(item.Name);

                using (var fileStream = System.IO.File.OpenRead(item.FullName))
                {
                    blockBlob.UploadFromStream(fileStream);
                }
            }
        }
    }
}