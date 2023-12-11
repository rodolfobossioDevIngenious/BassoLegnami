using System;
using System.Collections.Generic;
using System.Text;
using static In.Core.Configuration.Job;

namespace In.Core.Models
{
	public interface IJob
	{
		string JobID { get; }
		bool Execute(IEnumerable<JobDataRow> data);
	}
}
