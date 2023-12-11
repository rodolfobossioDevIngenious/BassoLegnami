using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BassoLegnami.Model.Data;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using In.Core.Models;
using BassoLegnami.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using BassoLegnami.Model.Models.Support;
using Elsa.Persistence.Specifications;
using TableDependency.SqlClient.Exceptions;
using Hangfire;
using DotLiquid.Util;
using System.ComponentModel.DataAnnotations;
using BassoLegnami.Extensions;

namespace BassoLegnami.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(IUnitOfWork unitOfWork, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public JsonResult ImpersonateUser()
        {
            //bool result = _unitOfWork.CustomersRepository.Impersonate(customerID ?? 0);
            return Json(new { result = true });
        }
    }
}
