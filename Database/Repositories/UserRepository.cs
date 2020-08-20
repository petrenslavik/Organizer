using Database.Entities;
using Database.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Repositories
{
    public class UserRepository : EFGenericRepository<User>, IUserRepository
    {
        public UserRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
