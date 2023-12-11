using In.Core.Data;
using In.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BassoLegnami.Model.Data.Repositories
{
	public class EmailMessageRepository : GenericRepository<Models.EmailMessage>
	{
		public EmailMessageRepository(IHttpContextAccessor httpContext, IdentityDbContext<ApplicationUser> context, Guid user) : base(httpContext, context, user)
		{
		}
	}
}
