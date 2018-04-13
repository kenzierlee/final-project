using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using keepr.Models;
using keepr.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace keepr.Controllers
{
    [Route("[controller]")]
    public class AccountController : Controller
    {
        //establishes the connection to the database "the Middle Man"
        //talks to the user, passes information to the repository/server
        private readonly UserRepository _accountRepo;
        public AccountController(UserRepository repo)
        {
            _accountRepo = repo;
        }
        [Authorize]
        [HttpGet("authenticate")]
        public UserReturnModel Authenticate()
        {
            var user = HttpContext.User;
            var id = user.Identity.Name;
            return _accountRepo.GetUserById(id);
        }
        [HttpPost("register")]
        //tasks can do more than one thing, sends back via cookie, a header w/a token that
        //tells the front end to save the info/credentials.
        public async Task<UserReturnModel> Register([FromBody] UserCreateModel userData)
        {
            if (!ModelState.IsValid) { return null; }//can also simply code: if(!ModelState.IsValid) {return null;} then proceed with other code
            try
            {
                UserReturnModel user = _accountRepo.Register(userData);
                ClaimsPrincipal principal = user.SetClaims();
                //HttpContext allows you to tell the application that the user just 
                //logged in and sets the cookie, allowing the user to remain logged in.
                await HttpContext.SignInAsync(principal);
                return user;
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
            }
            return null;
        }
        [HttpPost("login")]
        public async Task<UserReturnModel> Login([FromBody] UserLoginModel userData)
        {
            if (!ModelState.IsValid) { return null; }
            try
            {
                UserReturnModel user = _accountRepo.Login(userData);
                var principal = user.SetClaims();
                await HttpContext.SignInAsync(principal);
                return user;
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
            }
            return null;
        }
        [HttpDelete("logout")]
        public async Task<string> Logout()
        {
            //removes your Claims "logging you out".
            await HttpContext.SignOutAsync();
            return "Successfully Logged Out";
        }
        //allows user to be able to edit their account information.
        //authorize restrics the access to this path to only users 
        //who are currently logged in.
        [Authorize]
        [HttpPut]
        public UserReturnModel UpdateAccount([FromBody] UserReturnModel userData)
        {
            //gets the session id and finds the user where the id matches the ClaimType.Name/id
            var id = HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Name)
                .Select(c => c.Value).SingleOrDefault();
            UserReturnModel user = _accountRepo.GetUserById(id);
            return _accountRepo.UpdateAccount(user, userData);
        }
        [Authorize]
        [HttpPut("change-password")]
        public string ChangePassword([FromBody]ChangeUserPasswordModel userData)
        {
            if (ModelState.IsValid)
            {
                var id = HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Name)
                    .Select(c => c.Value).SingleOrDefault();
                UserReturnModel user = _accountRepo.GetUserById(id);
                return _accountRepo.ChangeUserPassword(userData);
            }
            return "Invalid Credentials";
        }
    }
}