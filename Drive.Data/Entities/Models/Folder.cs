namespace Drive.Data.Entities.Models
{
    public class Folder
    {
        public Folder(string name, int ownerId)
        {
            Name = name;
            OwnerId = ownerId;
        }

        public Folder(string name, int ownerId, int? parentFolderId)
        {
            Name = name;
            OwnerId = ownerId;
            ParentFolderId = parentFolderId;
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public int OwnerId { get; set; }
        public User? Owner { get; set; }
        public int? ParentFolderId { get; set; }
        public Folder? ParentFolder { get; set; }

        public ICollection<SharedFolder> SharedFolders { get; set; } = new List<SharedFolder>();
    }
}