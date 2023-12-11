using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BassoLegnami.Models.AccountViewModels
{
	public class ResetPasswordViewModel
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		[StringLength(100, ErrorMessage = "{0} deve essere lunga minimo {2} e massimo {1} caratteri.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "Le password inserite non coincidono.")]
		public string ConfirmPassword { get; set; }

		public string Code { get; set; }
	}
}
