using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1
{
    public class Register
    {
        public const string API_URL = "https://falcobot.dyndns-office.com:8084/api/Parent/RegisterPRS";
        private WebRequest req;

        public Register(UserProfile userProfile)
        {

            // ---set header and body required infos-- -
            req = WebRequest.Create(API_URL);
            req.Method = "POST";
            req.ContentType = "application/json";

            //format to json and add to request body
            using (var streamWriter = new StreamWriter(req.GetRequestStream()))
            {
                string data = JsonConvert.SerializeObject(userProfile);
                streamWriter.Write(data);
                streamWriter.Flush();
            }

        }

        //--- send a message request to LINE ---
        //return response data
        public string register()
        {
            string result = null;
            try
            {
                WebResponse response = req.GetResponse();
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                Trace.WriteLine(ex.ToString());
            }
            return result;
        }
    }
}