namespace Drive.Data.Entities.Models
{
    public class Folder
    {
        public Folder(string name, int ownerId)
        {
            Name = name;
            OwnerId = ownerId;
            IsRoot = true;
        }

        public Folder(string name, int ownerId, int parentFolderId)
        {
            Name = name;
            OwnerId = ownerId;
            ParentFolderId = parentFolderId;
            IsRoot = false;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsRoot { get; set; }

        public int OwnerId { get; set; }
        public User? Owner { get; set; }
        public int? ParentFolderId { get; set; }
        public Folder? ParentFolder { get; set; }

        public ICollection<FolderFile> FolderFiles { get; set; } = new List<FolderFile>();
        public ICollection<Folder> Subfolders { get; set; } = new List<Folder>();
        public ICollection<SharedFolder> SharedFolders { get; set; } = new List<SharedFolder>();
    }
}