namespace Drive.Data.Entities.Models
{
    public class File
    {
        public File(string name, string extension, string content, long size, int ownerId)
        {
            Name = name;
            Extension = extension;
            Content = content;
            Size = size;
            OwnerId = ownerId;
            CreatedOn = DateTime.Now;
            LastChanged = DateTime.Now;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public string Content { get; set; }
        public long Size { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastChanged { get; set; }

        public int OwnerId { get; set; }
        public User? Owner { get; set; }

        public ICollection<FolderFile> Folders { get; set; } = new List<FolderFile>();
    }
}