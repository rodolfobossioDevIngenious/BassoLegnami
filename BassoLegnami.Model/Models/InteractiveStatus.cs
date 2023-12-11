using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace BassoLegnami.Model.Models
{
	public class InteractiveStatus
	{
		#region Constants
		public const string CSS_INFO = "status-cm status-cm-info";
		public const string CSS_SUCCESS = "status-cm status-cm-success";
		public const string CSS_WARNING = "status-cm status-cm-warning";
		public const string CSS_DANGER = "status-cm status-cm-danger";
		#endregion

		#region Properties
		public string Description { get; set; }
		public string Icon { get; set; }
		public string CSSClass { get; set; }
		#endregion

		#region Constructor
		public InteractiveStatus(string description, string icon, string cssClass)
		{
			Description = description;
			Icon = icon;
			CSSClass = cssClass;
		}
		#endregion
	}
}
