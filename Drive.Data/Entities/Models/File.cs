namespace Drive.Data.Entities.Models
{
    public class File
    {
        public File(string name, string extension, int ownerId, int parentFolderId)
        {
            Name = name;
            Extension = extension;
            OwnerId = ownerId;
            ParentFolderId = parentFolderId;
            CreatedOn = DateTime.UtcNow;
            LastChanged = DateTime.UtcNow;
        }

        public File(string name, string extension, string content, long size, int ownerId, int parentFolderId)
        {
            Name = name;
            Extension = extension;
            Content = content;
            Size = size;
            OwnerId = ownerId;
            ParentFolderId = parentFolderId;
            CreatedOn = DateTime.UtcNow;
            LastChanged = DateTime.UtcNow;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public string? Content { get; set; }
        public long Size { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastChanged { get; set; }

        public int OwnerId { get; set; }
        public User? Owner { get; set; }
        public int ParentFolderId { get; set; }
        public Folder? ParentFolder { get; set; }

        public ICollection<SharedFile> SharedFiles { get; set; } = new List<SharedFile>();
    }
}