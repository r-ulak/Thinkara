using Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace FileUpload
{
    public class UploadServiceController : ApiController
    {
        private readonly string thumbprefix = "";
        private readonly JavaScriptSerializer _js = new JavaScriptSerializer { MaxJsonLength = 41943040 };
        private readonly string _storageRoot;
        private readonly string _storageRootThumb;
        public bool _isReusable { get { return false; } }

        public UploadServiceController()
        {
            _storageRoot = AppSettings.FileUploadPath;
            _storageRootThumb = AppSettings.FileUploadPathThumb;
        }

        #region Post & Put
        [HttpPost]
        public HttpResponseMessage UploadImage()
        {
            return UploadFile(HttpContext.Current);
        }

        private HttpResponseMessage UploadFile(HttpContext context)
        {
            var statuses = new List<FilesStatus>();
            var headers = context.Request.Headers;

            if (string.IsNullOrEmpty(headers["X-File-Name"]))
            {
                UploadWholeFile(context, statuses);
            }
            else
            {
                //UploadPartialFile(headers["X-File-Name"], context, statuses);
            }


            return WriteJsonIframeSafe(context, statuses);
        }

        private HttpResponseMessage WriteJsonIframeSafe(HttpContext context, List<FilesStatus> statuses)
        {
            context.Response.AddHeader("Vary", "Accept");
            var response = new HttpResponseMessage()
            {
                Content = new StringContent(_js.Serialize(statuses.ToArray()))
            };
            if (context.Request["HTTP_ACCEPT"].Contains("application/json"))
            {
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }
            else
            {
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
            }
            return response;
        }



        // Upload entire file
        private void UploadWholeFile(HttpContext context, List<FilesStatus> statuses)
        {
            for (int i = 0; i < context.Request.Files.Count; i++)
            {

                var file = context.Request.Files[i];
                string guidValue = "";
                guidValue = context.Request.Form["fileGuid"];
                string source = "";
                source = context.Request.Form["fileSource"];

                if (guidValue.Length <= 1)
                {
                    guidValue = Guid.NewGuid().ToString();
                }
                string fileName = guidValue + Path.GetExtension(file.FileName);
                string fileThumbNailName = guidValue + thumbprefix + Path.GetExtension(file.FileName);
                string fullPath = _storageRoot;
                string fullPaththumb = _storageRootThumb;
                if (source.Length > 0)
                {
                    fullPath = _storageRoot + source + "\\";
                    fullPaththumb = _storageRootThumb + source + "\\";
                }
                file.SaveAs(fullPath + fileName);
                Task taskA = Task.Run(() => GenerateThumbnail(fullPaththumb + fileThumbNailName, fullPath + fileName));
                string originalName = Path.GetFileName(file.FileName);
                statuses.Add(new FilesStatus(fileThumbNailName, source, originalName));
                taskA.Wait(1000);
            }
        }
        #endregion

        #region Thumbnail
        private void GenerateThumbnail(string fileName, string fromfileName)
        {

            Image image = Image.FromFile(fromfileName);
            int originalHeigth = image.Height;
            int originalWidth = image.Width;
            int thumbWidth = 120;
            Image thumb = image.GetThumbnailImage(thumbWidth, originalHeigth * thumbWidth / originalWidth, () => false, IntPtr.Zero);
            thumb.Save(fileName);
        }
        #endregion


    }





}