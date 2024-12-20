using Drive.Data.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Drive.Data.Entities;
using Drive.Domain.Enums;

namespace Drive.Domain.Repositories;

public class UserRepository : BaseRepository
{
    public UserRepository(DriveDbContext dbContext) : base(dbContext)
    {
    }

    public ResponseResultType Add(User user)
    {
        DbContext.Users.Add(user);

        return SaveChanges();
    }

    public ResponseResultType Delete(int id)
    {
        var userToDelete = DbContext.Users.Find(id);
        if (userToDelete is null)
        {
            return ResponseResultType.NotFound;
        }

        DbContext.Users.Remove(userToDelete);

        return SaveChanges();
    }

    public ResponseResultType Update(User user, int id)
    {
        var userToUpdate = DbContext.Users.Find(id);
        if (userToUpdate is null)
        {
            return ResponseResultType.NotFound;
        }

        userToUpdate.FirstName = user.FirstName;
        userToUpdate.LastName = user.LastName;

        return SaveChanges();
    }

    public User? GetById(int id) => DbContext.Users.FirstOrDefault(u => u.Id == id);

    public User? GetByEmail(string email) => DbContext.Users.FirstOrDefault(u => u.Email == email);

    public User? GetByEmailAndPassword(string email, string password) => DbContext.Users.FirstOrDefault(u => u.Password == password && u.Email == email);

    public ICollection<User> GetAll() => DbContext.Users.ToList();
}