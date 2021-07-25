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
    public class Parent_Profile
    {
        public const string API_URL = "https://falcobot.dyndns-office.com:8084/api/Parent/RegisterPR";
        private WebRequest req;
        public Parent_Profile(UserProfile userProfile)
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
        public string senduserprofile()
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
    public class GetParent
    {
        public const string API_URL = "https://falcobot.dyndns-office.com:8084/api/Parent/GetPR?parentId=";
        private WebRequest req;
        public GetParent(string parentId)
        {
            req = WebRequest.Create(API_URL + parentId);
            req.Method = "GET";
            req.ContentType = "application/json";
        }
        public UserProfile getParent()
        {
            UserProfile parent = new UserProfile();
            try
            {
                WebResponse response = req.GetResponse();
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    string result = streamReader.ReadToEnd();
                    parent = JsonConvert.DeserializeObject<UserProfile>(result);
                }
            }
            catch (WebException ex)
            {
                Trace.WriteLine(ex.ToString());
            }
            return parent;
        }

    }
}