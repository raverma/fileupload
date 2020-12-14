using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
//using Newtonsoft.Json;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using FileApi.Models;

namespace FileRead
{
  class Program
  {
    static void Main(string[] args)
    {
      //byte[] fileData = ReadBinaryFile();

      string url = "http://localhost:50303/api/postjson";
      
      Console.WriteLine(PostData(url));
      //string url = "http://localhost:50303/api/UploadFile";
      //string filePath = @"C:\Users\rverma\Pictures\Images\images_02.jpg";
      //UploadTest(url, filePath );
      Console.ReadLine();


    //------------------------------------------------------------
      ////Instantiate new CustomWebRequest class
      //CustomWebRequest wr = new CustomWebRequest(url);
      ////Set values for parameters
      ////For file type, send the inputstream of selected file
      //wr.ParamsCollection.Add(new ParamsStruct("file", fileData, ParamsStruct.ParamType.File, "Uploaded_file.txt"));
      ////PostData
      //wr.PostData();
      ////Set responsestring to console
      //Console.WriteLine(wr.ResponseString);
    //--------------------------------------------------
     
    }

    static byte[] ReadBinaryFile(string filePath)
    {
      byte[] bytes;
      using (FileStream fsSource = new FileStream(filePath,
            FileMode.Open, FileAccess.Read))
        {

            // Read the source file into a byte array.
            bytes = new byte[fsSource.Length];
            int numBytesToRead = (int)fsSource.Length;
            int numBytesRead = 0;
            while (numBytesToRead > 0)
            {
                // Read may return anything from 0 to numBytesToRead.
                int n = fsSource.Read(bytes, numBytesRead, numBytesToRead);

                // Break when the end of the file is reached.
                if (n == 0)
                    break;

                numBytesRead += n;
                numBytesToRead -= n;
            }
        }

        return bytes;
  
    }
    

    static bool UploadTest(string endPointUrl, string filePath)
    {
      //

      using (var client = new HttpClient())
        using (var formData = new MultipartFormDataContent())
        {
          //formData.Add(new ByteArrayContent(File.ReadAllBytes(filePath)),Path.GetFileNameWithoutExtension(filePath), Path.GetFileName(filePath));
          formData.Add(new ByteArrayContent(ReadBinaryFile(filePath)), "test", "test.jpg");

          //**********************


          //*************************

          var response = client.PostAsync(endPointUrl,formData).Result;
          if (response.StatusCode== HttpStatusCode.OK)
          {
            Console.WriteLine("Upload successful");
            Console.WriteLine(response.Headers);
            return false;
          }
          else
          {
            Console.WriteLine(response.StatusCode);
            
            return true;
          }
        }

    }

    static string PostData(string endpoint)
    {
      string  postJson ="{\"Message\": {\"test\": \"This is test json object\",\"main\": { \"name\": \"Joans HHGGD\", \"section\": \"BBBAAA\", \"data\": {\"level\": \"Easy\", \"case\": \"upper\"}}}}";
      string strResponse = string.Empty;

      HttpWebRequest request =(HttpWebRequest) WebRequest.Create(endpoint);
      request.Method = "POST";
      request.ContentType = "application/json";
      using (StreamWriter swJsonPayload = new StreamWriter(request.GetRequestStream()))
      {
        swJsonPayload.Write(postJson);

        swJsonPayload.Close();
      }

      HttpWebResponse response = null;
      try
      {
        response = (HttpWebResponse)request.GetResponse();
        using (Stream responseStream = response.GetResponseStream())
        {
          if (responseStream != null)
          {
            using (StreamReader reader = new StreamReader(responseStream))
            {
              strResponse = reader.ReadToEnd();
            }
          }
        }
      }
      catch(Exception ex)
      {
        strResponse= "{\"errormessages\": [\"" + ex.Message + "\"],\"errors\":{}}";
      }
      finally
      {
        if (response != null)
        {
          response.Dispose();
        }
      }
      return strResponse;
    } 
  }


  public class FileParameter
  {
    public byte[] File { get; set; }
    public string FileName { get; set; }
    public string ContentType { get; set; }
    public FileParameter(byte[] file) : this(file, null) { }
    public FileParameter(byte[] file, string filename) : this(file, filename, null) { }
    public FileParameter(byte[] file, string filename, string contenttype)
    {
      File = file;
      FileName = filename;
      ContentType = contenttype;
    }
  }
}