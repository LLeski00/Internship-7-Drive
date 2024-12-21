namespace Drive.Data.Entities.Models
{
    public class FolderFile
    {
        public int FolderId { get; set; }
        public Folder? Folder { get; set; }
        public int FileId { get; set; }
        public File? File { get; set; }
    }
}