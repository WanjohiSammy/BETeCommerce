
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Text;

namespace BETeCommerce.UtilityLayer.Helpers
{
    public class UtilHelper
    {
        /// <summary>
        /// Calcualte Records to be skipped using pageSize and pageNumber
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        public static int CalculateSkippedRecords(int pageSize, int pageNumber)
        {
            return (pageSize * pageNumber) - pageSize;
        }

        /// <summary>
        /// Generate Token
        /// </summary>
        /// <param name="userId">Application User Id</param>
        /// <param name="userEmail">Application User Email</param>
        /// <param name="key">Jwt Key in appsettings config</param>
        /// <returns></returns>
        public static string GenerateJwtToken(GenerateJwtInputParamsHelper jwtInputParams)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtInputParams.JwtKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.NameIdentifier, jwtInputParams.UserId),
                    new Claim(JwtRegisteredClaimNames.Email, jwtInputParams.UserEmail),
                    new Claim(JwtRegisteredClaimNames.Sub, jwtInputParams.UserEmail),
                    new Claim(JwtRegisteredClaimNames.GivenName, jwtInputParams.UserFullName),
                    new Claim(JwtRegisteredClaimNames.Aud, jwtInputParams.JwtAudience),
                    new Claim(JwtRegisteredClaimNames.Iss, jwtInputParams.JwtIssuer),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.Now.AddMinutes(30),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = jwtInputParams.JwtAudience,
                Issuer = jwtInputParams.JwtIssuer
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Get Resource Images Folder path
        /// </summary>
        public static string GetResourceImagesFolderName
        {
            get
            {
                return Path.Combine("Resources", "Images");
            }
        }
    }
}
