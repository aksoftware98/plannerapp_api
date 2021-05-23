using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PlannerApp.Models;
using PlannerApp.Server.Exceptions;
using PlannerApp.Server.Interfaces;
using PlannerApp.Server.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PlannerApp.Server.Services.V2
{
  
    public class UserService : IUserService
    {

        private UserManager<ApplicationUser> _userManger;
        private IConfiguration _configuration;
        public UserService(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManger = userManager;
            _configuration = configuration;
        }

        public async Task<UserManagerResponse> RegisterUserAsync(RegisterRequest model)
        {
            if (model == null)
                throw new NullReferenceException("Reigster Model is null");

            if (model.Password != model.ConfirmPassword)
                throw new ValidationException("Confirm Password doesn't match the password", null);

            var userByEmail = await _userManger.FindByEmailAsync(model.Email);
            if (userByEmail != null)
                throw new AlreadyExistsException($"User with the email {model.Email} already exists");

            var identityUser = new ApplicationUser
            {
                Email = model.Email,
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            var result = await _userManger.CreateAsync(identityUser, model.Password);

            if (result.Succeeded)
            {

                return new UserManagerResponse
                {
                    Message = "User created successfully!",
                    IsSuccess = true,
                };
            }
            throw new ValidationException("Failed to create the user", null); 
        }

        public async Task<UserManagerResponse> LoginUserAsync(LoginRequest model)
        {
            var user = await _userManger.FindByEmailAsync(model.Email);

            if (user == null)
            {
                throw new NotFoundException($"Username or password is invalid");
            }

            var result = await _userManger.CheckPasswordAsync(user, model.Password);

            if (!result)
                throw new NotFoundException($"Username or password is invalid");

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, model.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthSettings:Key"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["AuthSettings:Issuer"],
                audience: _configuration["AuthSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);

            return new UserManagerResponse
            {
                UserInfo = claims.ToDictionary(c => c.Type, c => c.Value),
                Message = tokenAsString,
                IsSuccess = true,
                ExpireDate = token.ValidTo
            };
        }
    }

}
