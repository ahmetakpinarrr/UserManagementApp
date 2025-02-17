using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UserManagementApp.DataAccess;
using UserManagementApp.Entities;
using UserManagementApp.Models;

namespace UserManagementApp.Business
{
    public class UserManager : IUserService
    {
        private readonly IUserDal _userDal;
        private readonly IConfiguration _configuration;

        public UserManager(IUserDal userDal, IConfiguration configuration)
        {
            _userDal = userDal;
            _configuration = configuration;
        }

        public List<User> GetAll()
        {
            return _userDal.GetAll();
        }

        public User GetById(int id)
        {
            return _userDal.GetById(id);
        }

        public void Add(User user)
        {
            user.Password=PasswordHelper.HashPassword(user.Password);
            _userDal.Add(user);
        }

        public void Update(User user)
        {
            _userDal.Update(user);
        }

        public void Delete(int id)
        {
            _userDal.Delete(id);
        }
        public string Authenticate(string email, string password)
        {
            //var user = _userDal.GetByEmail(email);
            //if (user == null || !PasswordHelper.VerifyPassword(password, user.Password))
            //{
            //    return null; // Kullanıcı bulunamadı veya şifre yanlış
            //}
            var user = _userDal.GetByEmail(email);
            if (user == null)
            {
                return null; // Kullanıcı bulunamadı
            }

            Console.WriteLine($"DB Password: {user.Password}");  // Veritabanındaki şifreyi kontrol et
            Console.WriteLine($"Entered Password: {password}");  // Girilen şifreyi kontrol et

            if (!PasswordHelper.VerifyPassword(password, user.Password))
            {
                return null; // Şifre yanlış
            }


            // JWT Token oluştur
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.AddMinutes(1), // 1 dakika geçerli olacak
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
