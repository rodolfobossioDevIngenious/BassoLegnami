using Microsoft.AspNetCore.Mvc;
using BassoLegnami.Controllers;

namespace BassoLegnami.Areas.Support.Controllers
{
    [Area("Support")]
    public class WorkflowsController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
