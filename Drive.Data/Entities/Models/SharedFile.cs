namespace Drive.Data.Entities.Models
{
    public class SharedFile
    {
        public SharedFile(int userId, int fileId)
        {
            UserId = userId;
            FileId = fileId;
        }

        public int UserId { get; set; }
        public User? User { get; set; }
        public int FileId { get; set; }
        public File? File { get; set; }
    }
}