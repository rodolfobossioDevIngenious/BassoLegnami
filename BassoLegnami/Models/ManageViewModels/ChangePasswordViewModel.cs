using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BassoLegnami.Models.ManageViewModels
{
	public class ChangePasswordViewModel
	{
		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Password corrente")]
		public string OldPassword { get; set; }

		[Required]
		[StringLength(100, ErrorMessage = "La {0} deve essere lunga minimo {2} e massimo {1} caratteri.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "Nuova password")]
		public string NewPassword { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Conferma nuova password")]
		[Compare("NewPassword", ErrorMessage = "La nuova password e la conferma non coincidono.")]
		public string ConfirmPassword { get; set; }

		public string StatusMessage { get; set; }
	}
}
