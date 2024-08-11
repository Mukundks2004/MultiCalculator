using Microsoft.EntityFrameworkCore;
using MultiCalculator.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiCalculator.Database.Repositories
{
    public class UserRepository : Repository<UserModel>
    {
        public UserRepository(CalculatorDbContext context) : base(context)
        {
        }

        public CalculatorDbContext CalculatorDbContext
        {
            get { return _context as CalculatorDbContext; }
        }

        public IEnumerable<UserModel> GetAllUser()
        {
            return CalculatorDbContext.User;
        }

        public UserModel GetUserById(int id)
        {
            return CalculatorDbContext.User.Where(a => a.Id == id).First();
        }

        public void UpdateUser(UserModel updatedUser)
        {
            var user = GetUserById(updatedUser.Id);
            if (user != null)
            {
                user = updatedUser;
                Update(user);
                SaveChanges();
            }
        }

        public void DeleteUser(int id)
        {
            UserModel user = GetUserById(id);
            if (user != null)
            {
                Remove(user);
                SaveChanges();
            }
        }
    }
}
