using System;
using System.Collections.Generic;
using System.Text;

namespace System.ComponentModel.DataAnnotations
{
	[AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
	public class VATCodeAttribute : ValidationAttribute
	{
		private const string VATCODE_REGEX = "^([0-9]{11})$";

		public override bool IsValid(object value)
		{
			string vatCode = Convert.ToString(value);

			if (string.IsNullOrEmpty(vatCode))
			{
				return true;
			}
			else if (Text.RegularExpressions.Regex.IsMatch(vatCode, VATCODE_REGEX))
			{
				int[] checkDigits = { 0, 2, 4, 6, 8, 1, 3, 5, 7, 9 };
				int sum = 0;
				for (int k = 0; k < 11; k++)
				{
					string s = vatCode.Substring(k, 1);
					int i = "0123456789".IndexOf(s);
					if (i == -1)
					{
						return false;
					}

					int x = int.Parse(s);
					if (k % 2 == 1) // Pari perchè iniziamo da zero
					{
						x = checkDigits[i];
					}

					sum += x;
				}
				return (sum % 10 == 0) && (sum != 0);
			}
			else
			{
				return false;
			}
		}
	}
}
