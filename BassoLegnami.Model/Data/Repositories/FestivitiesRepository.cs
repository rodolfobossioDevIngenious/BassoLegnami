using In.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BassoLegnami.Model.Data.Repositories
{
	public interface IFestivitiesRepository : In.Core.Data.IGenericRepository<Models.Support.Festivity>
	{
		bool IsFestivity(DateTime dateToCheck);
		bool IsFestivity(DateTime dateToCheck, string city);
	}

	public class FestivitiesRepository : In.Core.Data.GenericRepository<Models.Support.Festivity>, IFestivitiesRepository
	{
		public FestivitiesRepository(IHttpContextAccessor httpContext, IdentityDbContext<ApplicationUser> context, Guid user) : base(httpContext, context, user)
		{
		}

		public bool IsFestivity(DateTime dateToCheck)
		{
			int day = dateToCheck.Day;
			int month = dateToCheck.Month;
			int year = dateToCheck.Year;

			return Any(r => string.IsNullOrEmpty(r.City) && r.Day == day && r.Month == month && (!r.Year.HasValue || (r.Year.HasValue && r.Year == year)));
		}

		public bool IsFestivity(DateTime dateToCheck, string city)
		{
			if (string.IsNullOrEmpty(city))
			{
				throw new ArgumentException();
			}

			int day = dateToCheck.Day;
			int month = dateToCheck.Month;
			int year = dateToCheck.Year;

			return Any(r => city.Equals(r.City, StringComparison.InvariantCultureIgnoreCase) && r.Day == day && r.Month == month && (!r.Year.HasValue || (r.Year.HasValue && r.Year == year)));
		}
	}
}
