using Database.Entities;
using Database.Interfaces;
using Organiser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Organiser.Managers
{
    public class UserManager
    {
        private SHA256 hasher;
        private readonly IUnitOfWork uow;
        private readonly IRepository<User> userRepository;
        private readonly EmailManager emailManager;

        private const string PassSalt = "123456789101";
        private const string EmailSalt = "234567891011";

        public UserManager(IUnitOfWork uow, IRepository<User> userRepository, EmailManager emailManager)
        {
            hasher = SHA256.Create();
            this.uow = uow;
            this.userRepository = userRepository;
            this.emailManager = emailManager;
        }

        public void Register(UriBuilder callback, UserRegisterViewModel model)
        {
            var user = userRepository.Get(x => x.Email == model.Email).FirstOrDefault();
            if (user != null && user.IsConfirmed)
            {
                throw new Exception("User email is already registered");
            }


            if (user == null)
            {
                user = new User { Email = model.Email, Username = model.Username, PasswordHash = ComputeHash(model.Password + PassSalt) };
                userRepository.Insert(user);
            }
            else
            {
                user = new User { Email = model.Email, Username = model.Username, PasswordHash = ComputeHash(model.Password + PassSalt) };
                userRepository.Update(user);
            }
            uow.Commit();

            callback.Query = $"username={user.Username}&token={ComputeHash(user.Email + EmailSalt)}";
            var message = "Для завершения регистрации перейдите по ссылке: <a href=\""
                          + callback.Uri + "\">завершить регистрацию</a>";

            emailManager.SendEmail(model.Email, "Confirm your account", message);
        }

        private string ComputeHash(string data)
        {
            var bytes = hasher.ComputeHash(Encoding.UTF8.GetBytes(data));

            var builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }

        private bool VerifyHash(string password, string hash) => hash == ComputeHash(password);

        public User CheckPassword(string email, string password)
        {
            var user = userRepository.Get(x => x.Email == email && x.IsConfirmed == true).FirstOrDefault();
            if (user != null)
            {
                return VerifyHash(password + PassSalt, user.PasswordHash) ? user : null;
            }

            return null;
        }

        public bool IsOriginToken(string email, string token)
        {
            return VerifyHash(email + EmailSalt, token) ? true : false;
        }
    }
}
