using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace UserManagementApp.DataAccess
{
    public class UserDbContextFactory : IDesignTimeDbContextFactory<UserDbContext>
    {
        public UserDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<UserDbContext>();
            optionsBuilder.UseSqlServer("Server=AHMET;Database=UserManagementDb;Trusted_Connection=True;TrustServerCertificate=true;");

            return new UserDbContext(optionsBuilder.Options);
        }
    }
}
