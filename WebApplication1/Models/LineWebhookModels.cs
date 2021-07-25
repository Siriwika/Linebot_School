using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public enum EventType { message, follow, unfollow, join, leave, postback, beacon }
    public enum SourceType { user, group, room }
    public enum MessageType { text, image, video, audio, location, sticker }

    public class LineWebhookModels
    {
        public List<Event> events { get; set; }
    }

    public class Event
    {
        public EventType type { get; set; }
        public string timestamp { get; set; }
        public Source source { get; set; }
        public string replyToken { get; set; }
        public ReceiveMessage message { get; set; }
    }

    public class Source
    {
        public SourceType type { get; set; }
        public string userId { get; set; }
        public string groupId { get; set; }
        public string roomId { get; set; }
    }

    public abstract class Message<T>
    {
        public string id { get; set; }
        public T type { get; set; }
        public string text { get; set; }
        public string title { get; set; }
        public string address { get; set; }
        public decimal latitude { get; set; }
        public decimal longitude { get; set; }
        public string packageId { get; set; }
        public string stickerId { get; set; }
    }

    public class UserProfile
    {
        public string userId { get; set; }
        public string displayName { get; set; }
        public string pictureUrl { get; set; }
        public string statusMessage { get; set; }
        public string phoneNumber { get; set; }
        public string studentId { get; set; }
        public int status { get; set; }
    }

    public class Student
    {
        public string studentId { get; set; }
        public string student_Name { get; set; }
    }

    public class Grade
    {
        public string studentId { get; set; }
        public int semester { get; set; }
        public int classId { get; set; }

        [Column(TypeName = "decimal(3,2)")]
        public decimal grade { get; set; }
    }


    public class ReceiveMessage : Message<MessageType> { }
    public class SendMessage : Message<string> { }



    public class ReplyBody
    {
        public string replyToken { get; set; }
        public List<SendMessage> messages { get; set; }
    }
}