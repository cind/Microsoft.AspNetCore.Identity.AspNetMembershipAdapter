using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.AspNetCore.Identity.AspNetMembershipAdapter
{
    public class AspNetMembershipRoleStore:IRoleStore<AspNetMembershipRole>
    {
        private readonly AspNetMembershipDbContext _dbcontext;

        public AspNetMembershipRoleStore(AspNetMembershipDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public void Dispose()
        {
            _dbcontext.Dispose();
        }

        public async Task<IdentityResult> CreateAsync(AspNetMembershipRole role, CancellationToken cancellationToken)
        {
            try
            {
                var dbRole = Convert(role);
                _dbcontext.AspNetRoles.Add(dbRole);
                await _dbcontext.SaveChangesAsync(cancellationToken);
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

        public async Task<IdentityResult> UpdateAsync(AspNetMembershipRole role, CancellationToken cancellationToken)
        {
            try
            {
                var dbUser = await _dbcontext.AspNetRoles
                    .Where(u => u.RoleId == role.Id)
                    .SingleOrDefaultAsync(cancellationToken).ConfigureAwait(false);

                if (dbUser != null)
                {
                    dbUser.RoleName = role.Name;
                    dbUser.LoweredRoleName = role.NormalizedName.ToLower();
                    await _dbcontext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
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

        public async Task<IdentityResult> DeleteAsync(AspNetMembershipRole role, CancellationToken cancellationToken)
        {
            try
            {
                var dbUser = await _dbcontext.AspNetRoles
                    .SingleOrDefaultAsync(u => u.LoweredRoleName == role.NormalizedName.ToLower(),
                        cancellationToken: cancellationToken)
                    .ConfigureAwait(false);

                if (dbUser != null)
                {
                    _dbcontext.AspNetRoles.Remove(dbUser);
                    await _dbcontext.SaveChangesAsync(cancellationToken);
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

        public Task<string> GetRoleIdAsync(AspNetMembershipRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Id.ToString());
        }

        public Task<string> GetRoleNameAsync(AspNetMembershipRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Name);
        }

        public Task SetRoleNameAsync(AspNetMembershipRole role, string roleName, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Name = roleName);
        }

        public Task<string> GetNormalizedRoleNameAsync(AspNetMembershipRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.NormalizedName);
        }

        public Task SetNormalizedRoleNameAsync(AspNetMembershipRole role, string normalizedName, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.NormalizedName = normalizedName);
        }

        public async Task<AspNetMembershipRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            var roleGuid = Guid.Parse(roleId);
            var dbRole = await _dbcontext.AspNetRoles
                .Where(u => u.RoleId == roleGuid)
                .SingleOrDefaultAsync(cancellationToken).ConfigureAwait(false);

            if (dbRole == null)
                return null;

            return Convert(dbRole);
        }

        public async Task<AspNetMembershipRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            var dbRole = await _dbcontext.AspNetRoles
                .Where(u => u.LoweredRoleName == normalizedRoleName.ToLower())
                .SingleOrDefaultAsync(cancellationToken).ConfigureAwait(false);

            if (dbRole == null)
                return null;

            return Convert(dbRole);
        }

        private AspNetMembershipRole Convert(AspNetRole role)
        {
            return new AspNetMembershipRole
            {
                Id = role.RoleId,
                Name = role.RoleName,
                NormalizedName = role.LoweredRoleName,
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };
        }

        private AspNetRole Convert(AspNetMembershipRole role)
        {
            AspNetApplication application = _dbcontext.AspNetApplications.First();

            return new AspNetRole
            {
                ApplicationId = application.ApplicationId,
                RoleId = role.Id,
                RoleName = role.Name,
                LoweredRoleName = role.NormalizedName.ToLower()
            };
        }

    }
}
