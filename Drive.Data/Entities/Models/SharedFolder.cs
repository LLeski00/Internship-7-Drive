namespace Drive.Data.Entities.Models
{
    public class SharedFolder
    {
        public SharedFolder(int userId, int folderId)
        {
            UserId = userId;
            FolderId = folderId;
        }

        public int UserId { get; set; }
        public User? User { get; set; }
        public int FolderId { get; set; }
        public Folder? Folder { get; set; }
    }
}