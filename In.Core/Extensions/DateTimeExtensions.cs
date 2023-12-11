using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System
{
	public static class DateTimeExtensions
	{
		public static DateTime GetNextWeekday(this DateTime start, DayOfWeek day)
		{
			return start.AddDays(((int)day - (int)start.DayOfWeek + 7) % 7);
		}
	}
}
