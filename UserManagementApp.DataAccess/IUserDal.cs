using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagementApp.Entities;

namespace UserManagementApp.DataAccess
{
    public interface IUserDal
    {
        List<User> GetAll();
        User GetById(int id);
        void Add(User user);
        void Update(User user);
        void Delete(int id);
        User GetByEmail(string email);

    }
}
