using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.ComponentModel.DataAnnotations
{
	public class VATCodeValueAttribute : ValidationAttribute
	{
		private const string VATCODE_REGEX = "^[A-Za-z]{2}[0-9]{11}$";

		public override bool IsValid(object value)
		{
			string vatCode = Convert.ToString(value)?.ToUpper();

			if (string.IsNullOrEmpty(vatCode) || vatCode.Count() == 13)
			{
				return true;
			}
			else
			{
				if (vatCode.StartsWith("IT"))
				{
					return Text.RegularExpressions.Regex.IsMatch(vatCode, VATCODE_REGEX);
				}
				return true;
			}
		}

		public VATCodeValueAttribute()
		{
		}
	}
}
