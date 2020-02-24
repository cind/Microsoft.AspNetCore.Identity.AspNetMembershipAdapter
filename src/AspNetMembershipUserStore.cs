using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Identity.AspNetMembershipAdapter
{
    public class AspNetMembershipUserStore : 
        IUserPasswordStore<AspNetMembershipUser>, 
        IUserEmailStore<AspNetMembershipUser>,
        IUserRoleStore<AspNetMembershipUser>
    {
        private readonly AspNetMembershipDbContext _dbcontext;

        public AspNetMembershipUserStore(AspNetMembershipDbContext dbContext)
        {
            _dbcontext = dbContext;
        }

        public Task<IdentityResult> CreateAsync(AspNetMembershipUser user, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    AspNetUser dbUser = new AspNetUser();
                    this.Convert(user, dbUser);
                    _dbcontext.AspNetUsers.Add(dbUser);
                    _dbcontext.SaveChanges();
                    return IdentityResult.Success;
                }
                catch (Exception ex)
                {
                    return IdentityResult.Failed(new IdentityError
                    {
                        Code = ex.GetType().Name,
                        Description = ex.Message
                    });
                }
            });
        }

        public Task<IdentityResult> DeleteAsync(AspNetMembershipUser user, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    AspNetUser dbUser = _dbcontext.AspNetUsers
                        .Include(u => u.AspNetApplication)
                        .Include(u => u.AspNetMembership)
                        .SingleOrDefault(u => u.LoweredUserName == user.NormalizedUserName.ToLower());

                    if (dbUser != null)
                    {
                        _dbcontext.AspNetUsers.Remove(dbUser);
                        _dbcontext.SaveChanges();
                    }

                    return IdentityResult.Success;
                }
                catch (Exception ex)
                {
                    return IdentityResult.Failed(new IdentityError
                    {
                        Code = ex.GetType().Name,
                        Description = ex.Message
                    });
                }
            });
        }

        public void Dispose()
        {
            _dbcontext.Dispose();
        }

        public async Task<AspNetMembershipUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            AspNetUser dbUser = await _dbcontext.AspNetUsers
                .Include(u => u.AspNetApplication)
                .Include(u => u.AspNetMembership)
                .Where(u => u.AspNetMembership.Email.ToLower() == normalizedEmail.ToLower())
                .SingleOrDefaultAsync(cancellationToken).ConfigureAwait(false);

            if (dbUser == null)
            {
                return null;
            }

            AspNetMembershipUser user = new AspNetMembershipUser();
            this.Convert(dbUser, user);
            return user;
        }

        public async Task<AspNetMembershipUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            Guid gUserId = Guid.Parse(userId);
            AspNetUser dbUser = await _dbcontext.AspNetUsers
                .Include(u => u.AspNetApplication)
                .Include(u => u.AspNetMembership)
                .Where(u => u.UserId == gUserId)
                .SingleOrDefaultAsync(cancellationToken).ConfigureAwait(false);

            if (dbUser == null)
            {
                return null;
            }

            AspNetMembershipUser user = new AspNetMembershipUser();
            this.Convert(dbUser, user);
            return user;
        }

        public async Task<AspNetMembershipUser> FindByNameAsync(string normalizedUserName,
            CancellationToken cancellationToken)
        {
            AspNetUser dbUser = await _dbcontext.AspNetUsers
                .Include(u => u.AspNetApplication)
                .Include(u => u.AspNetMembership)
                .Where(u => u.LoweredUserName == normalizedUserName.ToLower())
                .SingleOrDefaultAsync(cancellationToken).ConfigureAwait(false);

            if (dbUser == null)
            {
                return null;
            }

            AspNetMembershipUser user = new AspNetMembershipUser();
            this.Convert(dbUser, user);
            return user;
        }

        public Task<string> GetEmailAsync(AspNetMembershipUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(AspNetMembershipUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task<string> GetNormalizedEmailAsync(AspNetMembershipUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedEmail);
        }

        public Task<string> GetNormalizedUserNameAsync(AspNetMembershipUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task<string> GetPasswordHashAsync(AspNetMembershipUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<string> GetUserIdAsync(AspNetMembershipUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id);
        }

        public Task<string> GetUserNameAsync(AspNetMembershipUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task<bool> HasPasswordAsync(AspNetMembershipUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(!string.IsNullOrEmpty(user.PasswordHash));
        }

        public Task SetEmailAsync(AspNetMembershipUser user, string email, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email = email);
        }

        public Task SetEmailConfirmedAsync(AspNetMembershipUser user, bool confirmed, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.EmailConfirmed = confirmed);
        }

        public Task SetNormalizedEmailAsync(AspNetMembershipUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedEmail = normalizedEmail);
        }

        public Task SetNormalizedUserNameAsync(AspNetMembershipUser user, string normalizedName, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUserName = normalizedName);
        }

        public Task SetPasswordHashAsync(AspNetMembershipUser user, string passwordHash, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash = passwordHash);
        }

        public Task SetUserNameAsync(AspNetMembershipUser user, string userName, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName = userName);
        }

        public async Task<IdentityResult> UpdateAsync(AspNetMembershipUser user, CancellationToken cancellationToken)
        {
            try
            {
                AspNetUser dbUser = await _dbcontext.AspNetUsers
                    .Include(u => u.AspNetApplication)
                    .Include(u => u.AspNetMembership)
                    .Where(u => u.UserId.ToString() == user.Id)
                    .SingleOrDefaultAsync(cancellationToken).ConfigureAwait(false);

                if (dbUser != null)
                {
                    this.Convert(user, dbUser);
                    _dbcontext.AspNetUsers.Update(dbUser);
                    _dbcontext.SaveChanges();
                }

                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = ex.GetType().Name,
                    Description = ex.Message
                });
            }
        }

        private AspNetMembershipUser Convert(AspNetUser from)
        {
            var to = new AspNetMembershipUser();
            Convert(from, to);
            return to;
        }

        private void Convert(AspNetUser from, AspNetMembershipUser to)
        {
            to.Id = from.UserId.ToString();
            to.UserName = from.UserName;
            to.NormalizedUserName = from.LoweredUserName.ToUpper();
            to.Email = from.AspNetMembership.Email.ToLower();
            to.NormalizedEmail = from.AspNetMembership.Email.ToUpper();
            to.EmailConfirmed = true;
            to.PasswordHash = from.AspNetMembership.Password;
            to.PasswordSalt = from.AspNetMembership.PasswordSalt;
            to.PasswordFormat = from.AspNetMembership.PasswordFormat;
            to.AccessFailedCount = from.AspNetMembership.FailedPasswordAttemptCount;
            to.EmailConfirmed = true;
            to.SecurityStamp = Guid.NewGuid().ToString(); // TODO: This isn't right.
        }

        private void Convert(AspNetMembershipUser from , AspNetUser to)
        {
            AspNetApplication application = _dbcontext.AspNetApplications.First();

            to.ApplicationId = application.ApplicationId;
            to.AspNetApplication = application;
            to.AspNetMembership = new AspNetMembership
            {
                ApplicationId = application.ApplicationId,
                AspNetApplication = application
            };

            to.UserId = Guid.Parse(from.Id);
            to.UserName = from.UserName;
            to.LoweredUserName = from.UserName.ToLower();
            to.LastActivityDate = DateTime.UtcNow;
            to.IsAnonymous = false;
            to.ApplicationId = application.ApplicationId;
            to.AspNetMembership.CreateDate = DateTime.UtcNow;
            to.AspNetMembership.Email = from.Email;
            to.AspNetMembership.IsApproved = true;
            to.AspNetMembership.LastLoginDate = DateTime.Parse("1754-01-01 00:00:00.000");
            to.AspNetMembership.LastLockoutDate = DateTime.Parse("1754-01-01 00:00:00.000");
            to.AspNetMembership.LastPasswordChangedDate = DateTime.Parse("1754-01-01 00:00:00.000");
            to.AspNetMembership.LoweredEmail = from.NormalizedEmail.ToLower();
            to.AspNetMembership.Password = from.PasswordHash;
            to.AspNetMembership.PasswordSalt = from.PasswordSalt;
            to.AspNetMembership.PasswordFormat = from.PasswordFormat;
            to.AspNetMembership.IsLockedOut = false;
            to.AspNetMembership.FailedPasswordAnswerAttemptWindowStart = DateTime.Parse("1754-01-01 00:00:00.000");
            to.AspNetMembership.FailedPasswordAttemptWindowStart = DateTime.Parse("1754-01-01 00:00:00.000");
        }

        public async Task AddToRoleAsync(AspNetMembershipUser user, string roleName, CancellationToken cancellationToken)
        {
            var dbuser = await _dbcontext.AspNetUsers
                .Where(u => u.UserId.ToString() == user.Id)
                .Include(u => u.AspNetUsersInRoles.Select(ur => ur.Role))
                .SingleOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);

            var role = await _dbcontext.AspNetRoles
                .Where(r => r.LoweredRoleName == roleName.ToLower())
                .SingleOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);

            var userRole = new AspNetUsersInRoles
            {
                RoleID = role.RoleId,
                UserID = dbuser.UserId
            };

            await _dbcontext.AspNetUsersInRoles.AddAsync(userRole, cancellationToken).ConfigureAwait(false);
            await _dbcontext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task RemoveFromRoleAsync(AspNetMembershipUser user, string roleName, CancellationToken cancellationToken)
        {
            var userRole = await _dbcontext.AspNetUsersInRoles
                .Where(ur => ur.UserID.ToString() == user.Id && ur.Role.LoweredRoleName == roleName.ToLower())
                .SingleOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);

            if (userRole == null)
                return;

            _dbcontext.AspNetUsersInRoles.Remove(userRole);
            await _dbcontext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<IList<string>> GetRolesAsync(AspNetMembershipUser user, CancellationToken cancellationToken)
        {
            return await _dbcontext.AspNetUsers
                .Where(u => u.UserId.ToString() == user.Id)
                .SelectMany(u => u.AspNetUsersInRoles)
                .Select(ur => ur.Role.LoweredRoleName)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<bool> IsInRoleAsync(AspNetMembershipUser user, string roleName, CancellationToken cancellationToken)
        {
            return await _dbcontext.AspNetUsersInRoles
                .AnyAsync(ur => ur.UserID.ToString() == user.Id && ur.Role.LoweredRoleName == roleName.ToLower(),
                    cancellationToken)
                .ConfigureAwait(false);

        }

        public async Task<IList<AspNetMembershipUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            var users = await _dbcontext.AspNetUsersInRoles
                .Where(ur => ur.Role.LoweredRoleName == roleName.ToLower())
                .Select(ur => ur.User)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            return users.Select(u => Convert(u)).ToList();
        }
    }
}
