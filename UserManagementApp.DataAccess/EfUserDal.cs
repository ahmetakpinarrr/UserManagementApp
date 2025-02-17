using UserManagementApp.DataAccess;
using UserManagementApp.Entities;
using UserManagementApp.Models;

public class EfUserDal : IUserDal
{
    private readonly UserDbContext _context;

    public EfUserDal(UserDbContext context)
    {
        _context = context;
    }

    public List<User> GetAll()
    {
        return _context.Users.ToList();
    }

    public User GetById(int id)
    {
        return _context.Users.FirstOrDefault(u => u.Id == id);
    }

    public void Add(User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
    }

    public void Update(User user)
    {
        _context.Users.Update(user);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var user = GetById(id);
        if (user != null)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }
    }

   

    public User GetByEmail(string email)
    {
        return _context.Users.FirstOrDefault(u => u.Email == email);
    }
}
