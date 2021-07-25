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
    public class Students
    {
        public const string API_URL = "https://falcobot.dyndns-office.com:8084/api/Student/GetStd?studentid=";
        private WebRequest req;

        public Students(string studentid)
        {
            req = WebRequest.Create(API_URL + studentid);
            req.Method = "GET";
            req.ContentType = "application/json";
        }

        public Student getstudent()
        {
            Student student = new Student();
            try
            {
                WebResponse response = req.GetResponse();
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    string result = streamReader.ReadToEnd();
                    student = JsonConvert.DeserializeObject<Student>(result);
                }
            }
            catch (WebException ex)
            {
                Trace.WriteLine(ex.ToString());
            }
            return student;
        }
    }

    public class Grades
    {
        public const string API_URL = "https://falcobot.dyndns-office.com:8084/api/Student/Mygrade?studentId=";
        private WebRequest req;
        public Grades(string studentid)
        {
            req = WebRequest.Create(API_URL + studentid);
            req.Method = "GET";
            req.ContentType = "application/json";
        }

        public List<Grade> getgrade()
        {
            List<Grade> grade = new List<Grade>();
            try
            {
                WebResponse response = req.GetResponse();
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    string result = streamReader.ReadToEnd();
                    grade = JsonConvert.DeserializeObject<List<Grade>>(result).ToList();
                }
            }
            catch (WebException ex)
            {
                Trace.WriteLine(ex.ToString());
            }
            return grade;
        }
    }
}