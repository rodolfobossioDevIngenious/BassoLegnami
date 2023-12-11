using In.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BassoLegnami.Model.Data.Repositories
{
    public interface IAuthorizationsRepository : In.Core.Data.IGenericRepository<In.Core.Models.Authorization.Authorization>
    {
        bool IsAuthorized(string controller, string action);
        void Flush();
    }

    public class AuthorizationsRepository : In.Core.Data.GenericRepository<In.Core.Models.Authorization.Authorization>, IAuthorizationsRepository
    {
        private static System.Collections.Concurrent.ConcurrentBag<In.Core.Models.Authorization.Authorization> _authorizations;

        public AuthorizationsRepository(IHttpContextAccessor httpContext, IdentityDbContext<ApplicationUser> context, Guid user) : base(httpContext, context, user)
        {
        }

        public bool IsAuthorized(string controller, string action)
        {
            string[] standardActions = new string[] { "create", "edit", "details", "delete", "index" };
            if (_authorizations == null)
            {
                _authorizations = new System.Collections.Concurrent.ConcurrentBag<In.Core.Models.Authorization.Authorization>(_context.Set<In.Core.Models.Authorization.Authorization>().AsNoTracking().ToList());
            }

            if (IsInRole(In.Core.Models.Authorization.Authorization.ROLE_ADMINISTRATORS))
            {
                return true;
            }

            string searchAction = action?.ToLower();
            bool output = _authorizations.Any(r => Roles.ContainsKey(r.RoleId) && r.Controller.Equals(controller, StringComparison.CurrentCultureIgnoreCase) && r.Action.Equals(searchAction, StringComparison.CurrentCultureIgnoreCase) && r.Authorized);
            if (!output && !standardActions.Contains(searchAction))
            {
                output = _authorizations.Any(r => Roles.ContainsKey(r.RoleId) && r.Controller.Equals(controller, StringComparison.CurrentCultureIgnoreCase) && r.Action.Equals("*", StringComparison.CurrentCultureIgnoreCase) && r.Authorized);
            }
            return output;
        }

        public void Flush()
        {
            _authorizations = null;
        }
    }
}
