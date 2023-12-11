using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace BassoLegnami.Controllers
{
	[Authorize]
	[Extensions.Authorization]
	public class BaseController : Controller
	{
	}
}
