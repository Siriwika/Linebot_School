using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class LineBotController : ApiController
    {
        [HttpPost]
        [Signature]
        public IHttpActionResult webhook([FromBody] LineWebhookModels data)
        {
            if (data == null) return BadRequest();
            if (data.events == null) return BadRequest();
            foreach (Event e in data.events)
            {
                if (e.type == EventType.message)
                {
                    Profile profile = new Profile(e.source.userId);
                    var user = profile.getuserprofile();
                    ReplyBody rb = new ReplyBody()
                    {
                        replyToken = e.replyToken,
                        messages = procMessage(e.message, e.source, user)
                    };
                    Reply reply = new Reply(rb);
                    reply.send();
                }
            }
            return Ok(data);
        }




        private List<SendMessage> procMessage(ReceiveMessage m, Source source, UserProfile userProfile)
        {
            List<SendMessage> msgs = new List<SendMessage>();
            SendMessage sm = new SendMessage()
            {
                type = Enum.GetName(typeof(MessageType), m.type)
            };
            switch (m.type)
            {
                case MessageType.sticker:
                    sm.packageId = m.packageId;
                    sm.stickerId = m.stickerId;
                    break;
                case MessageType.text:
                    Profile profiles = new Profile(m.text);
                    if (m.text == "ลงทะเบียน")
                    {
                        string message = checkUserRegister(source);
                        sm.text = message;
                    }
                    if (m.text == "ผลการเรียน")
                    {
                        sm.text = ("กรุณากรอกรหัสนักเรียนเพื่อตรวจสอบผลการเรียนตัวอย่างเช่น \nเกรด 6101070311");
                    }
                    if ((profiles.checktext(m.text, 0)) == "เบอร์โทร")
                    {
                        string phone = profiles.checktext(m.text, 1);
                        int phone1 = phone.Count();
                        var prefix = phone.Substring(0, 2);
                        string message = checkphone(phone, phone1, prefix, source, userProfile);
                        sm.text = message;
                    }
                    if ((profiles.checktext(m.text, 0)) == "รหัสนักเรียน")
                    {
                        string stdId = profiles.checktext(m.text, 1);
                        int stdId1 = stdId.Count();
                        string message = checkstudentId(stdId, stdId1, source);
                        sm.text = message;
                    }
                    if ((profiles.checktext(m.text, 0)) == "เกรด")
                    {
                        string stdId = profiles.checktext(m.text, 1);
                        int stdId1 = stdId.Count();
                        string message = checkgradestudent(stdId, stdId1);
                        sm.text = message;
                    }
                    if (m.text == "userprofile")
                        sm.text = ("userprofile = \n  UId :" + userProfile.userId + "\n name : " + userProfile.displayName + "\n Imageurl : " + userProfile.pictureUrl);
                    break;
                default:
                    sm.type = Enum.GetName(typeof(MessageType), MessageType.text);
                    sm.text = "ขอโทษ ฉันเป็นแค่หุ่นยนต์นกแก้ว ตอนนี้ฉันตอบได้เฉพาะสติกเกอร์และข้อความพื้นฐานเท่านั้น! ";
                    break;
            }
            msgs.Add(sm);
            return msgs;
        }

        private string checkphone(string phone, int phone1, string prefix, Source source, UserProfile userProfile)
        {
            string message = "";
            if (phone1 != 10 && (prefix != "06" || prefix != "08" || prefix != "09"))
            {
                message = ("กรุณาตรวจสอบรูปแบบเบอร์โทรของท่านให้ถูกต้อง");
            }
            else if (phone1 == 10 && (prefix == "06" || prefix == "08" || prefix == "09"))
            {
                UserProfile user = new UserProfile()
                {
                    userId = source.userId,
                    displayName = userProfile.displayName,
                    pictureUrl = userProfile.pictureUrl,
                    phoneNumber = phone,
                };
                Parent_Profile profile = new Parent_Profile(user);
                profile.senduserprofile();
                message = ("กรุณากรอกรหัสนักเรียน ตัวอย่าง เช่น \nรหัสนักเรียน 6101070300");
            }
            return message;
        }

        private string checkUserRegister(Source source)
        {
            string message = "";
            //GetParent pr = new GetParent(source.userId);
            //var parent = pr.getParent();
            //if (parent.userId == source.userId)
            //{
            //    message = ("กรุณากรอกรหัสนักเรียน ตัวอย่างเช่น \nรหัสนักเรียน: 6101070300");
            //}
            //else
            //{
            message = ("กรุณากรอกเบอร์โทรผู้ปกครอง ตัวอย่างเช่น \nเบอร์โทร 0699999999");
            //}
            return message;
        }

        private string checkstudentId(string stdId, int stdId1, Source source)
        {
            string message = "";
            if (stdId1 == 10)
            {
                //Students std = new Students(stdId);
                //var student = std.getstudent();
                //if (student.studentId == stdId)
                //{
                //    message = ("ท่านได้ลงทะเบียน\nรหัสนักเรียน " + stdId + " ไปแล้ว");
                //}
                //else
                //{
                UserProfile PRS = new UserProfile()
                {
                    userId = source.userId,
                    studentId = stdId,
                    status = 1
                };
                Register register = new Register(PRS);
                register.register();
                message = ("บันทึกข้อมูลการลงทะเบียน\nรหัสนักเรียน " + stdId + " สำเร็จ");
                //}                            
            }
            if (stdId1 != 10)
            {
                message = ("รูปแบบรหัสนักเรียนของท่านผิด");
            }
            return message;
        }

        private string checkgradestudent(string stdId, int stdId1)
        {
            string message = null;
            if (stdId1 == 10)
            {
                Grades grades = new Grades(stdId);
                var mygrade = grades.getgrade();
                foreach (var item in mygrade)
                {
                    message = message+("ผลการเรียนภาคเรียนที่ "+item.semester+ " = " +item.grade + "\n");
                }
            }
            return message;
        }
    }
}