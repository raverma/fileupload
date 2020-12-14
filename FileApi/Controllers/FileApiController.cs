using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.IO;
using System.Threading.Tasks; 
using FileApi.Models;
//using System.Web.Extensions;
using System.Web.Script.Serialization;

namespace FileApi.Controllers
{
    public class FileApiController : ApiController
    {
      [HttpPost, Route("api/UploadFile")]
      public async Task<string> UploadFileWithStream()
      {

        var context = HttpContext.Current;
        var root = HttpContext.Current.Server.MapPath("~/app_data/");
        var provider = new MultipartFormDataStreamProvider(root);

        try
        {
          await Request.Content.ReadAsMultipartAsync(provider);
      
          
          foreach ( var file in provider.FileData)
          {
            var name = file.Headers.ContentDisposition.FileName.Trim('"');

            var filePath = Path.Combine(root, name);
            File.Move(file.LocalFileName, filePath);
          }
        }
        catch (Exception ex)
        {
          return "Error: " + ex.Message;
        }
        return "File Uploaded";
        
      }

      [HttpPost, Route("api/PostJson")]
      public IHttpActionResult PostData(Data data)
      {

        if (data == null)
        {
          return BadRequest("The message is invalid or null");
        }
        //string json = new JavaScriptSerializer().Serialize(data);
        string json = data.Message.ToString();
        var root = HttpContext.Current.Server.MapPath("~/app_data/");
        var filePath = Path.Combine(root, "data.json");
        System.IO.File.WriteAllText(filePath, json);
        return Ok("The json data has been saved at " + filePath);
      }

      [HttpPost, Route("api/PostMultipart")]
      public async Task<string> PostMultipart(Data data)
      {
        var root = HttpContext.Current.Server.MapPath("~/app_data/");
        var provider = new MultipartFormDataStreamProvider(root);
        var filePath=root;
        try
        {
          if (data != null)
          { 
            string json = data.Message.ToString();
            filePath = Path.Combine(root, "data.json");
            System.IO.File.WriteAllText(filePath, json);
          }

          await Request.Content.ReadAsMultipartAsync(provider);
          foreach (var file in provider.FileData)
          {
            var name = file.Headers.ContentDisposition.FileName.Trim('"');

            filePath = Path.Combine(root, name);
            File.Move(file.LocalFileName, filePath);
          }
          
        }
        catch (Exception ex)
        {
          return "Error: " + ex.Message;
        }
        return "File Uploaded";
      }
      
    }
}
