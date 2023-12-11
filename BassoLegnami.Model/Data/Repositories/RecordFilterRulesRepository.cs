using In.Core.Models;
using In.Core.Models.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BassoLegnami.Model.Data.Repositories
{
	public interface IRecordFilterRulesRepository : In.Core.Data.IGenericRepository<RecordFilterRule>
	{
		IQueryable<RecordFilterRule> GetByUser(Guid userId);
		IQueryable<RecordFilterRule> GetByUser();
	}

	public class RecordFilterRulesRepository : In.Core.Data.GenericRepository<RecordFilterRule>, IRecordFilterRulesRepository
	{
		public RecordFilterRulesRepository(IHttpContextAccessor httpContext, IdentityDbContext<ApplicationUser> context, Guid user) : base(httpContext, context, user)
		{
		}

		public void _UpdateRelated(RecordFilterRule t)
		{
			IEnumerable<int> entityToUpdateRowList1 = t.RecordFilterRuleValues.Where(r => r.RecordFilterRuleValueID != 0).Select(s => s.RecordFilterRuleValueID);
			IQueryable<RecordFilterRuleValue> listToDelete1 = _context.Set<RecordFilterRuleValue>().AsQueryable().Where(r => r.RecordFilterRuleID == t.RecordFilterRuleID && !entityToUpdateRowList1.Contains(r.RecordFilterRuleValueID));
			_context.Set<RecordFilterRuleValue>().RemoveRange(listToDelete1);
			_context.Set<RecordFilterRuleValue>().UpdateRange(t.RecordFilterRuleValues.Where(r => r.RecordFilterRuleValueID != 0));
			t.RecordFilterRuleValues.Where(r => r.RecordFilterRuleValueID == 0).ToList().ForEach(r => { r.RecordFilterRuleID = t.RecordFilterRuleID; _context.Set<RecordFilterRuleValue>().Add(r); });
		}

		public override RecordFilterRule Update(RecordFilterRule t, object key)
		{
			_UpdateRelated(t);
			return base.Update(t, key);
		}

		public override Task<RecordFilterRule> UpdateAsync(RecordFilterRule t, object key)
		{
			_UpdateRelated(t);
			return base.UpdateAsync(t, key);
		}

		public IQueryable<RecordFilterRule> GetByUser(Guid userId)
		{
			return _context.Set<RecordFilterRule>()
				.AsQueryable()
				.Where(r => r.UserId == userId)
				.Include(r => r.RecordFilterRuleType)
				.Include(r => r.RecordFilterRuleValues);
		}

		public IQueryable<RecordFilterRule> GetByUser()
		{
			return GetByUser(User);
		}
	}
}
