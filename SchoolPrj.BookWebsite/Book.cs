namespace SchoolPrj.BookWebsite
{
        public class Book
        {
            public int BookId { get; set; }
            public string BookTitle { get; set; }
            public string Category { get; set; }
            public string Author { get; set; }
            public string Description { get; set; }
            public string ImagePath { get; set; }
            public string PdfPath { get; set; }

            public string keyword { get; set; }
        }
    
}