using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace API.Helpers
{
    public static class PasswordHash
    {
        /// <summary>
        /// If the password doesn't match it returns false
        /// </summary>
        /// <param name="user"></param>
        /// <param name="loginDTO"></param>
        /// <returns></returns>
        public static bool CheckPassword(AppUser user, LoginDTO loginDTO)
        {
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password));
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return false;
            }
            return true;
        }

    }
}
