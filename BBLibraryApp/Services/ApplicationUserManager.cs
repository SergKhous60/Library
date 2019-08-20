using BBLibraryApp.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BBLibraryApp.Services
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<ApplicationUser> passwordHasher,
            IEnumerable<IUserValidator<ApplicationUser>> userValidators,
            IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<ApplicationUser>> logger)
            : base(store, optionsAccessor, passwordHasher, userValidators,
                  passwordValidators, keyNormalizer, errors, services, logger)
        {
        }

        /// <summary>
        /// Sets the <paramref name="email"/> address for a <paramref name="user"/>.
        /// </summary>
        /// <param name="user">The user whose email should be set.</param>
        /// <param name="email">The email to set.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, 
        /// containing the <see cref="IdentityResult"/>
        /// of the operation.
        /// </returns>
        public override async Task<IdentityResult> SetEmailAsync(ApplicationUser user, string email)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (user.EmailConfirmed && user.Email.ToUpper() != email.ToUpper())
                user.UnconfirmedEmail = email;
            else
                user.Email = email;

            return await UpdateUserAsync(user);
        }

        /// <summary>
        /// Validates that an email confirmation token matches the specified 
        /// <paramref name="user"/> and if successful sets
        /// EmailConfirmed to true and if UnconfirmedEmail is not NULL or Empty, 
        /// copies the user's UnconfirmedEmail to user's
        /// Email and sets UnconfirmedEmail to NULL.
        /// </summary>
        /// <param name="user">The user to validate the token against.</param>
        /// <param name="token">The email confirmation token to validate.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, 
        /// containing the <see cref="IdentityResult"/>
        /// of the operation.
        /// </returns>
        public override async Task<IdentityResult> ConfirmEmailAsync(ApplicationUser user, string token)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            IdentityResult result;
            var provider = Options.Tokens.EmailConfirmationTokenProvider;
            var isValidToken = await base.VerifyUserTokenAsync(user, provider, "EmailConfirmation", token);
            if (isValidToken)
            {
                if (!string.IsNullOrEmpty(user.UnconfirmedEmail))
                {
                    user.Email = user.UnconfirmedEmail;
                    user.UnconfirmedEmail = null;
                }
                user.EmailConfirmed = true;
                result = await UpdateUserAsync(user);
            }
            else
            {
                result = IdentityResult.Failed(new IdentityErrorDescriber().InvalidToken());
            }
            return result;
        }
    }
}
