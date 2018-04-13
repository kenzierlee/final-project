using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace keepr.Models
{
    public class User
    {
        //key is saying this is a particularl property that is a way to 
        //reference a user.
        [Key]
        [Required]
        public string Id { get; set; }
        public string Name { get; set; }
        [Required]
        //verifies that the users email is legit
        [EmailAddress]
        public string Email { get; set; }
        [MinLength(6)]
        public string Password { get; set; }
    }
    public class UserCreateModel
    {
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [MinLength(6)]
        public string Password { get; set; }

    }
    public class UserLoginModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
    //the return for a User who's logged in.
    public class UserReturnModel
    {
        [Required]
        public string Id { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string Name { get; set; }
        // setting up session tokens/"Claims" (auto login)
        public ClaimsPrincipal SetClaims()
        {
            //sets up a system that saves a users email address into the "request header"
            var claims = new List<Claim>
            {
                //nodejs equivalent to "req.session.uid = user._id" and "req.session.save()"
                //Claims uses the cookie authentication created and encrypts the information.
                new Claim(ClaimTypes.Email, Email),
                new Claim(ClaimTypes.Name, Id)
            };
            var userIdentity = new ClaimsIdentity(claims, "login");
            var principal = new ClaimsPrincipal(userIdentity);
            return principal;
        }

    }
    //what will show for someone not logged in. The user, 
    //doesnt include id, email or password for security purposes.
    public class PublicUserModel
    {
        public string Name { get; set; }
    }
    public class ChangeUserPasswordModel
    {
        public int Id { get; set; }
        [MaxLength(255), EmailAddress]
        public string Email { get; set; }
        [Required, MinLength(4)]
        public string OldPassword { get; set; }
        [Required, MinLength(4)]
        public string NewPassword { get; set; }
    }
}