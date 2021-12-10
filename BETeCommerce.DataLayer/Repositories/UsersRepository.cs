using BETeCommerce.BunessEntities.Configurations;
using BETeCommerce.BunessEntities.DataEntities;
using BETeCommerce.BunessEntities.Requests;
using BETeCommerce.DataLayer.Models;
using BETeCommerce.UtilityLayer.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace BETeCommerce.DataLayer.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _singInManager;
        private readonly JwtConfig _jwtConfig;

        public UsersRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> singInManager, IOptions<JwtConfig> jwtConfigOptions)
        {
            _userManager = userManager;
            _singInManager = singInManager;
            _jwtConfig = jwtConfigOptions.Value;
        }

        public async Task<UserDetailsDto> RegisterUser(AddOrUdateUserRequest request, SecureString securedPassword)
        {
            var now = DateTimeOffset.Now;
            var user = new ApplicationUser
            {
                FullName = request.FullName,
                Email = request.Email,
                UserName = request.Email,
                DateCreated = now
            };

            // SecuredPassword to String
            request.Password = new NetworkCredential(request.Email, securedPassword).Password;
            var result = await _userManager.CreateAsync(user, request.Password).ConfigureAwait(true);

            var response = new UserDetailsDto
            {
                FullName = user.FullName,
                Email = user.Email,
                UserName = user.UserName,
                DateCreated = now,
                IsSucceeded = result.Succeeded
            };

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description).ToArray();
                response.Message = string.Join("\n", errors);
            }

            return response;
        }

        public async Task<UserDetailsDto> LoginUser(LoginUserRequest request, SecureString securedPassword)
        {
            // SecuredPassword to String
            request.Password = new NetworkCredential(request.Email, securedPassword).Password;
            var result = await _singInManager.PasswordSignInAsync(request.Email, request.Password, false, false);

            var response = new UserDetailsDto
            {
                IsSucceeded = result.Succeeded,
                Email = request.Email,
                UserName = request.Email
            };

            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                response.AccessToken = UtilHelper.GenerateJwtToken(new GenerateJwtInputParamsHelper {
                    UserId = user.Id,
                    UserEmail = user.Email,
                    UserFullName = user.FullName,
                    JwtKey = _jwtConfig.Key,
                    JwtAudience = _jwtConfig.Audience,
                    JwtIssuer = _jwtConfig.Issuer
                });
            }

            return response;
        }
    }
}
