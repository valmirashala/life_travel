
using Microsoft.AspNetCore.Mvc;

using Travelephant.Body;
using Travelephant.Data;
using Travelephant.Model;
using Travelephant.Helper;
using Travelephant.Dtos;

namespace Travelephant.Controllers
{

    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly TravelephantContext _context;
        private readonly JwtService _jwtService;

        public UsersController(TravelephantContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
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
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto userBody)
        {
            var user = _context.User
                 .Where(x => x.Username == userBody.username).FirstOrDefault();

            //check if user with that username exists
            if (user == null)
            {
                return BadRequest("Username is invalid");
            }

            //Check if name the user wrote id compatible with the name of the user with the same id
            //This is just for testing purposes(in a real website we should check the password)
            var match = (user.Name == userBody.name) ? true : false;
            if (!match)
            {
                return BadRequest("Username or Name was Invalid");
            }
            var jwt = _jwtService.Generate(user.UserId);

            Response.Cookies.Append("jwt", jwt, new CookieOptions
            {
                HttpOnly = true
            });

            return Ok(new
            {
                message = "success"
            });
        }

        [HttpGet("user")]
        public IActionResult User()
        {
            try
            {
                var jwt = Request.Cookies["jwt"];

                var token = _jwtService.Verify(jwt);

                int userId = int.Parse(token.Issuer);

                var user = _context.User.Where(x => x.UserId == userId).FirstOrDefault();

                return Ok(user);
            }
            catch (Exception)
            {
                return Unauthorized("error");
            }
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");

            return Ok(new
            {
                message = "success"
            });
        }


    }
}
