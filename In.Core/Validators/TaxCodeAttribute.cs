using System;
using System.Collections.Generic;
using System.Text;

namespace System.ComponentModel.DataAnnotations
{
	[AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
	public class TaxCodeAttribute : ValidationAttribute
	{
		private const string TAXCODE_REGEX = "^([A-Za-z]{6}[0-9lmnpqrstuvLMNPQRSTUV]{2}[abcdehlmprstABCDEHLMPRST]{1}[0-9lmnpqrstuvLMNPQRSTUV]{2}[A-Za-z]{1}[0-9lmnpqrstuvLMNPQRSTUV]{3}[A-Za-z]{1})|([0-9]{11})$";

		public override bool IsValid(object value)
		{
			string taxCode = Convert.ToString(value);

			if (string.IsNullOrEmpty(taxCode))
			{
				return true;
			}
			else
			{
				return Text.RegularExpressions.Regex.IsMatch(taxCode, TAXCODE_REGEX);
			}
		}
	}
}
