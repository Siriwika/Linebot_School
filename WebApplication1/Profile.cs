using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;
using WebApplication1.Models;

namespace WebApplication1
{
   
    public class Profile
    {
        public const string API_URL = "https://api.line.me/v2/bot/profile/";
        private WebRequest req;
        public Profile(string userId)
        { 
            
            // ---set header and body required infos-- -
            req = WebRequest.Create(API_URL + userId);
            req.Method = "GET";
            req.ContentType = "application/json";
            req.Headers["Authorization"] = "Bearer " + WebConfigurationManager.AppSettings["AccessToken"];

            //format to json and add to request body
            //using (var streamWriter = new StreamWriter(req.GetRequestStream()))
            //{
            //    string data = JsonConvert.SerializeObject(body);
            //    streamWriter.Write(data);
            //    streamWriter.Flush();
            //}
        }
        public UserProfile getuserprofile()
        {
            UserProfile userProfile = new UserProfile();
            try
            {
                WebResponse response = req.GetResponse();
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    string result = streamReader.ReadToEnd();
                    userProfile = JsonConvert.DeserializeObject<UserProfile>(result);
                }
            }
            catch (WebException ex)
            {
                Trace.WriteLine(ex.ToString());
            }
            return userProfile;
        }

        public string checktext(string text,int index)
        {
            char[] delimiterCharss = { ' ' };
            string textt = text;
            string[] wordss = textt.Split(delimiterCharss);
            string word = wordss[index];
            return word;
        }
    }
}