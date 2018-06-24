﻿using System;
using System.Threading.Tasks;
using Blog.Core.Identity.Managers;

namespace Blog.Core.Identity.Contracts
{
    /// <summary>
    /// Provides an abstraction for two factor token generators.
    /// </summary>
    /// <typeparam name="TUser">The type encapsulating a user.</typeparam>
    public interface IUserTwoFactorTokenProvider<TUser> where TUser : class
    {
        /// <summary>
        /// Generates a token for the specified <paramref name="user"/> and <paramref name="purpose"/>.
        /// </summary>
        /// <param name="purpose">The purpose the token will be used for.</param>
        /// <param name="manager">The <see cref="UserManager{TUser}"/> that can be used to retrieve user properties.</param>
        /// <param name="user">The user a token should be generated for.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the token for the specified 
        /// <paramref name="user"/> and <paramref name="purpose"/>.
        /// </returns>
        /// <remarks>
        /// The <paramref name="purpose"/> parameter allows a token generator to be used for multiple types of token whilst
        /// insuring a token for one purpose cannot be used for another. For example if you specified a purpose of "Email" 
        /// and validated it with the same purpose a token with the purpose of TOTP would not pass the heck even if it was
        /// for the same user.
        /// 
        /// Implementations of <see cref="IUserTwoFactorTokenProvider{TUser}"/> should validate that purpose is not null or empty to
        /// help with token separation.
        /// </remarks>
        Task<String> GenerateAsync(String purpose, UserManager<TUser> manager, TUser user);

        /// <summary>
        /// Returns a flag indicating whether the specified <paramref name="token"/> is valid for the given
        /// <paramref name="user"/> and <paramref name="purpose"/>.
        /// </summary>
        /// <param name="purpose">The purpose the token will be used for.</param>
        /// <param name="token">The token to validate.</param>
        /// <param name="manager">The <see cref="UserManager{TUser}"/> that can be used to retrieve user properties.</param>
        /// <param name="user">The user a token should be validated for.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the a flag indicating the result
        /// of validating the <paramref name="token"> for the specified </paramref><paramref name="user"/> and <paramref name="purpose"/>.
        /// The task will return true if the token is valid, otherwise false.
        /// </returns>
        Task<Boolean> ValidateAsync(String purpose, String token, UserManager<TUser> manager, TUser user);

        /// <summary>
        /// Returns a flag indicating whether the token provider can generate a token suitable for two factor authentication token for
        /// the specified <paramref name="user"/>.
        /// </summary>
        /// <param name="manager">The <see cref="UserManager{TUser}"/> that can be used to retrieve user properties.</param>
        /// <param name="user">The user a token could be generated for.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the a flag indicating if a two
        /// factor token could be generated by this provider for the specified <paramref name="user"/>.
        /// The task will return true if a two factor authentication token could be generated, otherwise false.
        /// </returns>
        Task<Boolean> CanGenerateTwoFactorTokenAsync(UserManager<TUser> manager, TUser user);
    }
}
