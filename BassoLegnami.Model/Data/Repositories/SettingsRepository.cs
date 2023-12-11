using In.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BassoLegnami.Model.Data.Repositories
{
	public interface ISettingsRepository : In.Core.Data.IGenericRepository<Models.Support.Setting>
	{
		Models.Support.Setting GetByKey(string key);
	}

	public class SettingsRepository : In.Core.Data.GenericRepository<Models.Support.Setting>, ISettingsRepository
	{
		public SettingsRepository(IHttpContextAccessor httpContext, IdentityDbContext<ApplicationUser> context, Guid user) : base(httpContext, context, user)
		{
		}

		public Models.Support.Setting GetByKey(string key)
		{
			return FirstOrDefault(r => r.Key == key);
		}
	}
}
