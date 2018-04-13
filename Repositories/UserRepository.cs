using System;
using System.Collections.Generic;
using System.Data;
using keepr.Models;
using Dapper;


namespace keepr.Repositories
{
    public class UserRepository
    {
        //your service talking to the database
        //the connection is a "field" because its a private method
        private readonly IDbConnection _db;
        public UserRepository(IDbConnection db)
        {
            _db = db;
        }
        public UserReturnModel Register(UserCreateModel userData)
        {
            //Generate an Id, then construct a user, run a SQL command,
            Guid g;
            g = Guid.NewGuid();
            string id = g.ToString(); //generates a long char/num string
            User user = new User()
            {
                Id = id,
                Name = userData.Name,
                Email = userData.Email,
                //encrypt the password using bcrypt for security purposes.
                Password = BCrypt.Net.BCrypt.HashPassword(userData.Password)
            };
            var success = _db.Execute(@"
            INSERT INTO users(
                id,
                name,
                email,
                password
            ) VALUES(@Id, @Name, @Email, @Password)
            ", user);
            //and then create user and return UserReturnModel.
            if(success < 1)
            {
                throw new Exception("Invalid Credentials");
            }
            return new UserReturnModel()
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            };
        }
        public UserReturnModel Login(UserLoginModel userData)
        {
            //look up user by email, if it works it sends back a user
            User user = _db.QueryFirstOrDefault<User>(@"
            SELECT * FROM users WHERE email = @Email
            ", userData);
            //Check and make sure password is valid, will need to hash the userData.Password 
            //to check against the saved hashed password. ".Verify" method takes care of that.
            var valid = BCrypt.Net.BCrypt.Verify(userData.Password, user.Password);
            if(valid)
            {
                return new UserReturnModel()
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email
                };
            }
            throw new Exception("Invalid Credentials");
        }

        public UserReturnModel GetUserById(string id)
        {
            User user = _db.QueryFirstOrDefault<User>(@"
            SELECT * FROM users WHERE id = @Id
            ", new {Id = id});
            //if this happens someones most likely hacking your application, as the 
            //page is only accessible by someone whos logged in, and it pulls the id from
            //the users Claim. Or your connection to the database went down.
            if(user == null){throw new Exception("Oh Boy Something Bad Happened");}
            return new UserReturnModel
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            };
        }
        public UserReturnModel UpdateAccount(UserReturnModel user, UserReturnModel userData)
        {
            //this only allows the user to update their email and name.
            var i = _db.Execute(@"
                UPDATE users SET
                    email = @Email,
                    name = @Name
                WHERE id = @Id
            ", userData);
            if(i > 0)
            {
                return user;
            }
            return null;
        }
        public string ChangeUserPassword(ChangeUserPasswordModel user)
        {
            User savedUser = _db.QueryFirstOrDefault<User>(@"
                SELECT * FROM users WHERE id = @id
                ", user);
            var valid = BCrypt.Net.BCrypt.Verify(user.OldPassword, savedUser.Password);
            if(valid)
            {
                user.NewPassword = BCrypt.Net.BCrypt.HashPassword(user.NewPassword);
                var i = _db.Execute(@"
                    UPDATE users SET
                        password = @NewPassword
                    WHERE id = @id
                ", user);
                return "Good Job!";
            }
            return "Ummm Nope!";
        }
    }
}