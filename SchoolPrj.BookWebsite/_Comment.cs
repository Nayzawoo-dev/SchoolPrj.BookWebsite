using System;
using System.Collections.Generic;

namespace SchoolPrj.BookWebsite
{
    public class _Comment
    {
        public int CommentId { get; set; }

        public string UserProfile { get; set; }
        public string UserName { get; set; }
        public int BookId { get; set; }
        public int UserId { get; set; }
        public string Comment { get; set; }
        public string VideoPath { get; set; } = null;
        public DateTime Date { get; set; }

        public string ImagePath { get; set; } = null;

        public List<Reply> Replies { get; set; } = new List<Reply>();

        public string Role { get; set; }
    }

}