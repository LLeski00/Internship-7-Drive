namespace Drive.Data.Entities.Models
{
    public class FileComment
    {
        public FileComment(string content, int authorId, int fileId)
        {
            Content = content;
            AuthorId = authorId;
            FileId = fileId;
            CreatedOn = DateTime.UtcNow;
            LastChanged = DateTime.UtcNow;
        }

        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastChanged { get; set; }

        public int AuthorId { get; set; }
        public User? Author { get; set; }
        public int FileId { get; set; }
        public File? File { get; set; }
    }
}