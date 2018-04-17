using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace WebApplication.WebAPI.Models.Helper
{
    /// <summary>
    /// 文件下载结果
    /// </summary>
    public class FileResult : IHttpActionResult
    {
        string _fileName = "";
        byte[] _bytes;
        HttpRequestMessage _request;
        public FileResult(string fileName, byte[] bytes, HttpRequestMessage request)
        {
            _fileName = fileName;
            _bytes = bytes;
            _request = request;
        }
        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {

            var response = new HttpResponseMessage()
            {
                Content = new ByteArrayContent(_bytes),
                RequestMessage = _request
            };
            //response.Headers.TransferEncodingChunked = false;
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = HttpUtility.UrlEncode(_fileName, System.Text.Encoding.UTF8);// HttpUtility.UrlEncode(string.Format("{0}{1}", templete.TemplateName, fileExt), System.Text.Encoding.UTF8);
            response.Content.Headers.ContentLength = _bytes.Length;
            //response.Headers.Chche.Add("Cache-Control", "must-revalidate, post-check=0, pre-check=0");
            response.Headers.CacheControl = new CacheControlHeaderValue()
            {
                MustRevalidate = true
            };
            return System.Threading.Tasks.Task.FromResult(response);
        }
    }
}