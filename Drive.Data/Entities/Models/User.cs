namespace Drive.Data.Entities.Models
{
    public class User
    {
        public User(string email, string password, string firstName, string lastName)
        {
            Email = email;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
        }

        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ICollection<SharedFile> SharedFiles { get; set; } = new List<SharedFile>();
        public ICollection<SharedFolder> SharedFolders { get; set; } = new List<SharedFolder>();
    }
}