using System.Threading.Tasks;
using Blog.Core.Identity.Managers;
using IdentityResult = Microsoft.AspNetCore.Identity.IdentityResult;

namespace Blog.Core.Identity.Contracts
{
    /// <summary>
    /// Provides an abstraction for user validation.
    /// </summary>
    /// <typeparam name="TUser">The type encapsulating a user.</typeparam>
    public interface IUserValidator<TUser> where TUser : class
    {
        /// <summary>
        /// Validates the specified <paramref name="user"/> as an asynchronous operation.
        /// </summary>
        /// <param name="manager">The <see cref="UserManager{TUser}"/> that can be used to retrieve user properties.</param>
        /// <param name="user">The user to validate.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="Microsoft.AspNetCore.Identity.IdentityResult"/> of the validation operation.</returns>
        Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user);
    }
}