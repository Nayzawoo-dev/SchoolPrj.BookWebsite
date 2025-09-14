using System;

namespace SchoolPrj.BookWebsite
{
    public class Reply
    {
        public int ReplyId { get; set; }
        public int CommentId { get; set; }
        public int UserId { get; set; }
        public string ReplyText { get; set; }
        public string UserProfile { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }

        public string VideoPath { get; set; }

        public string ImagePath { get; set; }
        public string ReplyUserName { get; set; }
        public DateTime Date { get; set; }
    }

}