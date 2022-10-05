using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Travelephant.Data;
using Travelephant.Model;

namespace Travelephant.Controllers
{
  
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly TravelephantContext _context;

        public UsersController(TravelephantContext context)
        {
            _context = context;
        }

        [HttpGet("user-admin")]
        public bool IsAdmin(int Id)
        {
            var user = _context.User.Where(x => x.UserId == Id).FirstOrDefault();
            return user.IsAdmin;
        }
        [HttpGet("user-id")]
        public int GetUserId(string Username)
        {
            var user = _context.User.Where(x => x.Username == Username).FirstOrDefault();
            return user.UserId;
        }

        [HttpPost("add-user")]
        public IEnumerable<User> AddUser(string Name, string Surname, string Username,
            string Address)
        {
            var user = _context.User
                .Where(x => x.Username == Username).FirstOrDefault();

            if (user == null)
            {
                var newUser = new User
                {
                    Name = Name,
                    Surname = Surname,
                    Username = Username,
                    Address = Address,
                    IsAdmin = false
                };
                _context.Add(newUser);
                _context.SaveChanges();
                var Users = _context.User
                    .Where(x => x.Username == newUser.Username).ToList();
                return Users;
            }
            else
            {
                var Users = _context.User
                    .Where(x => x.UserId == 0).ToList();
                return Users;
            }
        }
    }
}
