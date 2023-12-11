using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace In.Core.Configuration
{
	public class ApplicationConfigurations
	{
		public string ApplicationName { get; set; }
		public string ApplicationShortName { get; set; }
		public string ApplicationURL { get; set; }
		public string CompanyName { get; set; }
		public string CompanyWebSite { get; set; }
		public string CompanyLogo { get; set; }
		public int PasswordExpireDays { get; set; }
		public List<Job> Jobs { get; set; }

		public ApplicationConfigurations()
		{
			Jobs = new List<Job>();
		}
	}

	public class Job
	{
		public class JobDataRow
		{
			public string Key { get; set; }
			public string Value { get; set; }
		}

		public string Name { get; set; }
		public string CRON { get; set; }
		public bool Enabled { get; set; }
		public List<JobDataRow> Data { get; set; }
		public Job()
		{
			Data = new List<JobDataRow>();
		}
	}
}
