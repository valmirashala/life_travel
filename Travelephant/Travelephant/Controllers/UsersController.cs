using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Travelephant.Body;
using Travelephant.Data;
using Travelephant.Model;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Travelephant.Controllers
{
  
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly TravelephantContext _context;
        private readonly AppSettings _applicationSettings;

        public UsersController(TravelephantContext context, IOptions<AppSettings> _applicationSettings)
        {
            _context = context;
            this._applicationSettings = _applicationSettings.Value;
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
        public IEnumerable<User> AddUser([FromBody] UserBody userBody)
        {
            var user = _context.User
                .Where(x => x.Username == userBody.Username).FirstOrDefault();

            if (user == null)
            {
                var newUser = new User
                {
                    Name = userBody.Name,
                    Surname = userBody.Surname,
                    Username = userBody.Username,
                    Address = userBody.Address,
                    IsAdmin = true
                };
                _context.Add(newUser);
                _context.SaveChanges();
                var Users = _context.User
                    .Where(x => x.Username == newUser.Username).ToList();
                return Users;
            }
            else
            {
                return Enumerable.Empty<User>();
            }
        }
        [HttpPut("update-username")]
        public User UpdateUsername(string Username, [FromBody] UserBody UserBody)
        {
            //Get User with username == Username
            var user = _context.User
                .Where(x => x.Username == Username).FirstOrDefault();

                if (user == null)
                    return new User();

                user.Name = UserBody.Name;
                user.Surname = UserBody.Surname;
                user.Username = UserBody.Username;
                user.Address = UserBody.Address;

                _context.User.Update(user);
                _context.SaveChanges();

            return user;
            
        }
        [HttpPost("Login")]
        public IActionResult Login([FromBody] UserBody userBody)
        {
            var user = _context.User
                 .Where(x => x.Username == userBody.Username).FirstOrDefault();
           
            //check if user with that username exists
            if (user == null)
            {
                return BadRequest("Username is invalid");
            }

            //Check if name the user wrote id compatible with the name of the user with the same id
            //This is just for testing purposes(in a real website we should check the password)
            var match = (user.Name == userBody.Name) ? true : false;
            if (!match)
            {
                return BadRequest("Username or Name was Invalid");
            }
            //If name and username matches create the jwt token and return to the user
            var tokenHandler = new JwtSecurityTokenHandler();
            var key= Encoding.ASCII.GetBytes(this._applicationSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Username) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var encrypterToken = tokenHandler.WriteToken(token);
            return Ok(new { token = encrypterToken, username = user.Username, id=user.UserId});

        }
    }
}
