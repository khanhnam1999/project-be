using CommonDataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace CommonDataLayer.Untilities
{
    public static class EntityUntilities
    {
        // Hash password
        /** @param password: The plain text password to be hashed
         * @return: The hashed password using BCrypt algorithm
         */
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        // Verify password
        /**         * @param hashedPassword: The hashed password stored in the database
         * @param providedPassword: The plain text password provided by the user during login
         * @return: true if the provided password matches the hashed password, false otherwise
         */
        public static bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(hashedPassword, providedPassword);
        }
    }
}
