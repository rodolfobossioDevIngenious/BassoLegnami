using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace In.Core.Data
{
	public interface IUnitOfWork
	{
		int Save();
		Task<int> SaveChangesAsync();
	}
}
