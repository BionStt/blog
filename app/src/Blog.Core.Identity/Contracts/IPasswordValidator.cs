﻿using System;
using System.Threading.Tasks;
using Blog.Core.Identity.Managers;
using IdentityResult = Microsoft.AspNetCore.Identity.IdentityResult;

namespace Blog.Core.Identity.Contracts
{
    /// <summary>
    /// Provides an abstraction for validating passwords.
    /// </summary>
    /// <typeparam name="TUser">The type that represents a user.</typeparam>
    public interface IPasswordValidator<TUser> where TUser : class
    {
        /// <summary>
        /// Validates a password as an asynchronous operation.
        /// </summary>
        /// <param name="manager">The <see cref="UserManager{TUser}"/> to retrieve the <paramref name="user"/> properties from.</param>
        /// <param name="user">The user whose password should be validated.</param>
        /// <param name="password">The password supplied for validation</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user, String password);
    }
}
