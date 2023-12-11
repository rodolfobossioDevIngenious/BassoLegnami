using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using In.Core.Models;
using Microsoft.Extensions.Localization;
using BassoLegnami.Model.Data;
using System.Reflection;
using System.IO;
using Microsoft.AspNetCore.Mvc.Localization;
using In.Core.Configuration;
using Microsoft.AspNetCore.Hosting;
using BassoLegnami.Models.AccountViewModels;
using BassoLegnami.Model.Plugins;
using Microsoft.EntityFrameworkCore;
using In.Core.Models.Authorization;
using DocumentFormat.OpenXml.Office2010.Excel;
using BassoLegnami.Model.Models.Users;

namespace BassoLegnami.Controllers
{
    [Route("[controller]/[action]")]
    public class AccountController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ILogger _logger;
        private readonly IStringLocalizer<AccountController> _localizer;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHtmlLocalizer<Models.SharedResource> _sharedResourceLocalizer;
        private readonly IOptions<ApplicationConfigurations> _applicationConfigurations;
        private readonly Elsa.Services.IWorkflowPublisher _workflowPublisher;
        private readonly Elsa.Serialization.IContentSerializer _contentSerializer;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            IWebHostEnvironment hostingEnvironment,
            ILogger<AccountController> logger,
            IStringLocalizer<AccountController> localizer,
            IUnitOfWork unitOfWork,
            IHtmlLocalizer<Models.SharedResource> sharedResourceLocalizer,
            IOptions<ApplicationConfigurations> applicationConfigurations,
            Elsa.Services.IWorkflowPublisher workflowPublisher,
            Elsa.Serialization.IContentSerializer contentSerializer)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _logger = logger;
            _localizer = localizer;
            _unitOfWork = unitOfWork;
            _sharedResourceLocalizer = sharedResourceLocalizer;
            _applicationConfigurations = applicationConfigurations;
            _hostingEnvironment = hostingEnvironment;
            _workflowPublisher = workflowPublisher;
            _contentSerializer = contentSerializer;

            string[] roleNames = { UnitOfWork.ROLE_ADMINISTRATORS };
            foreach (string roleName in roleNames)
            {
                bool roleExist = _roleManager.RoleExistsAsync(roleName).Result;
                if (!roleExist)
                {
                    //create the roles and seed them to the database: Question 1
                    _ = _roleManager.CreateAsync(new IdentityRole(roleName)).Result;
                }
            }

            ApplicationUser applicationUser;
            ApplicationUser user;
            Task<ApplicationUser> userTask;
            Task<IdentityResult> createUserTask;
            IdentityResult newUser;
            Task<IdentityResult> userToRoleTask;

            // import users
            const string INPUT_FILE = @"users.csv";

            if (System.IO.File.Exists(INPUT_FILE))
            {
                string emailTemplate = null;

                Assembly assembly = Assembly.GetExecutingAssembly();

                using (Stream stream = assembly.GetManifestResourceStream("BassoLegnami.Resources.EmailTemplates.MigrationUserCreated.html"))
                using (StreamReader reader = new(stream))
                {
                    emailTemplate = reader.ReadToEnd();
                }

                // perform seed
                DatabaseSeed.Seed(_hostingEnvironment, _unitOfWork, _roleManager, _userManager, _workflowPublisher, _contentSerializer);

                // record filter rules types
                List<RecordFilterRuleType> recordFilterRulesTypes = _unitOfWork.RecordFilterRuleTypesRepository.GetAll().AsNoTracking().ToList();

                string[] records = System.IO.File.ReadAllLinesAsync(INPUT_FILE).Result;
                foreach (string record in records)
                {
                    string[] fields = record.Split(';');

                    //User creation
                    applicationUser = new ApplicationUser
                    {
                        Id = fields[0],
                        UserName = fields[1],
                        Email = fields[2],
                        Enabled = fields[3].Equals("true", StringComparison.InvariantCultureIgnoreCase),
                        LastPasswordChangedDate = DateTime.Today,
                    };

                    userTask = _userManager.FindByEmailAsync(applicationUser.Email);
                    userTask.Wait();
                    user = userTask.Result;

                    if (user == null)
                    {
                        string passwword = In.Core.Extensions.PasswordGenerator.GenerateRandomPassword();
                        if (applicationUser.Email == "admin@ingenioussrl.it")
                        {
                            passwword = "Password2023!!";
                        }
                        createUserTask = _userManager.CreateAsync(applicationUser, passwword);
                        createUserTask.Wait();
                        newUser = createUserTask.Result;
                        if (newUser.Succeeded)
                        {
                            // bind to roles
                            string[] roles = fields[6].Split('|', StringSplitOptions.RemoveEmptyEntries);
                            foreach (string role in roles)
                            {
                                //here we tie the new user to the role
                                userToRoleTask = _userManager.AddToRoleAsync(applicationUser, role);
                                userToRoleTask.Wait();
                                if (!userToRoleTask.Result.Succeeded)
                                {
                                    throw new ArgumentException();
                                }
                            }

                            // create record filter rules
                            if (!string.IsNullOrEmpty(fields[5]) && fields[5] != "0")
                            {
                                _unitOfWork.RecordFilterRulesRepository.Add(new RecordFilterRule()
                                {
                                    RecordFilterRuleTypeID = recordFilterRulesTypes.First(r => r.Flag1 == fields[5]).RecordFilterRuleTypeID,
                                    UserId = new Guid(applicationUser.Id),
                                    RecordFilterRuleValues = new HashSet<RecordFilterRuleValue>()
                                    {
                                        new RecordFilterRuleValue() { RecordID = int.Parse(fields[4]) }
                                    }
                                });
                            }

                            // send email
                            _unitOfWork.EmailMessagesRepository.Add(new Model.Models.EmailMessage()
                            {
                                RecipientEmail = applicationUser.Email,
                                Subject = "ICRI BassoLegnami si aggiorna alla versione 2.0",
                                Message = emailTemplate
                                    .Replace("{{username}}", applicationUser.Email)
                                    .Replace("{{password}}", passwword),
                            });
                        }
                        else
                        {
                            throw new ArgumentException();
                        }
                    }
                }
                _unitOfWork.Save();
            }
            else
            {
                //User creation
                applicationUser = new()
                {
                    UserName = ApplicationUser.USER_ADMINISTRATOR,
                    Email = "admin@ingenioussrl.it",
                    Enabled = true,
                    LastPasswordChangedDate = DateTime.Today,
                };

                const string userPWD = "Password2018!!";
                userTask = _userManager.FindByEmailAsync(applicationUser.Email);
                userTask.Wait();
                user = userTask.Result;

                if (user == null)
                {
                    createUserTask = _userManager.CreateAsync(applicationUser, userPWD);
                    createUserTask.Wait();
                    newUser = createUserTask.Result;
                    if (newUser.Succeeded)
                    {
                        //here we tie the new user to the role
                        userToRoleTask = _userManager.AddToRoleAsync(applicationUser, In.Core.Models.Authorization.Authorization.ROLE_ADMINISTRATORS);
                        userToRoleTask.Wait();
                    }
                }

                //User creation
                applicationUser = new ApplicationUser
                {
                    UserName = ApplicationUser.USER_SYSTEM,
                    Email = "system@ingenioussrl.it",
                    Enabled = true,
                    LastPasswordChangedDate = DateTime.Today,
                };

                userTask = _userManager.FindByEmailAsync(applicationUser.Email);
                userTask.Wait();
                user = userTask.Result;

                if (user == null)
                {
                    createUserTask = _userManager.CreateAsync(applicationUser, userPWD);
                    createUserTask.Wait();
                    newUser = createUserTask.Result;
                    if (newUser.Succeeded)
                    {
                        //here we tie the new user to the role
                        userToRoleTask = _userManager.AddToRoleAsync(applicationUser, In.Core.Models.Authorization.Authorization.ROLE_ADMINISTRATORS);
                        userToRoleTask.Wait();
                    }
                }
            }
        }

        [TempData]
        public string ErrorMessage { get; set; }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme).ConfigureAwait(false);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                ApplicationUser user = await _userManager.FindByEmailAsync(model.Email).ConfigureAwait(false);
                if (user?.Enabled == true)
                {
                    Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, lockoutOnFailure: false).ConfigureAwait(false);
                    if (result.Succeeded)
                    {
                        if (await _userManager.IsInRoleAsync(user, In.Core.Models.Authorization.Authorization.ROLE_ADMINISTRATORS).ConfigureAwait(false))
                        {
                            DatabaseSeed.Seed(_hostingEnvironment, _unitOfWork, _roleManager, _userManager, _workflowPublisher, _contentSerializer);

                            List<Assembly> plugins = PluginAssemblyLoader.LoadPlugins(UnitOfWork.PLUGINS_FOLDER);
                            plugins.ForEach(r => PluginAssemblyLoader.Seed(r, _unitOfWork));
                        }

                        _logger.LogInformation("User logged in.");
                        return RedirectToLocal(returnUrl);
                    }
                    if (result.RequiresTwoFactor)
                    {
                        return RedirectToAction(nameof(LoginWith2fa), new { returnUrl, model.RememberMe });
                    }
                    if (result.IsLockedOut)
                    {
                        _logger.LogWarning("User account locked out.");
                        return RedirectToAction(nameof(Lockout));
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, _localizer["InvalidLoginAttempt"]);
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, _localizer["InvalidLoginAttempt"]);
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> LoginWith2fa(bool rememberMe, string returnUrl = null)
        {
            // Ensure the user has gone through the username & password screen first
            ApplicationUser user = await _signInManager.GetTwoFactorAuthenticationUserAsync().ConfigureAwait(false);

            if (user == null)
            {
                throw new ApplicationException($"Unable to load two-factor authentication user.");
            }

            LoginWith2faViewModel model = new() { RememberMe = rememberMe };
            ViewData["ReturnUrl"] = returnUrl;

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginWith2fa(LoginWith2faViewModel model, bool rememberMe, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            ApplicationUser user = await _signInManager.GetTwoFactorAuthenticationUserAsync().ConfigureAwait(false);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            string authenticatorCode = model.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);

            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, rememberMe, model.RememberMachine).ConfigureAwait(false);

            if (result.Succeeded)
            {
                _logger.LogInformation("User with ID {UserId} logged in with 2fa.", user.Id);
                return RedirectToLocal(returnUrl);
            }
            else if (result.IsLockedOut)
            {
                _logger.LogWarning("User with ID {UserId} account locked out.", user.Id);
                return RedirectToAction(nameof(Lockout));
            }
            else
            {
                _logger.LogWarning("Invalid authenticator code entered for user with ID {UserId}.", user.Id);
                ModelState.AddModelError(string.Empty, "Invalid authenticator code.");
                return View();
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> LoginWithRecoveryCode(string returnUrl = null)
        {
            // Ensure the user has gone through the username & password screen first
            ApplicationUser user = await _signInManager.GetTwoFactorAuthenticationUserAsync().ConfigureAwait(false);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load two-factor authentication user.");
            }

            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginWithRecoveryCode(LoginWithRecoveryCodeViewModel model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            ApplicationUser user = await _signInManager.GetTwoFactorAuthenticationUserAsync().ConfigureAwait(false);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load two-factor authentication user.");
            }

            string recoveryCode = model.RecoveryCode.Replace(" ", string.Empty);

            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.TwoFactorRecoveryCodeSignInAsync(recoveryCode).ConfigureAwait(false);

            if (result.Succeeded)
            {
                _logger.LogInformation("User with ID {UserId} logged in with a recovery code.", user.Id);
                return RedirectToLocal(returnUrl);
            }
            if (result.IsLockedOut)
            {
                _logger.LogWarning("User with ID {UserId} account locked out.", user.Id);
                return RedirectToAction(nameof(Lockout));
            }
            else
            {
                _logger.LogWarning("Invalid recovery code entered for user with ID {UserId}", user.Id);
                ModelState.AddModelError(string.Empty, "Invalid recovery code entered.");
                return View();
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Lockout()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                ApplicationUser user = new() { UserName = model.Email, Email = model.Email };
                IdentityResult result = await _userManager.CreateAsync(user, model.Password).ConfigureAwait(false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    string code = await _userManager.GenerateEmailConfirmationTokenAsync(user).ConfigureAwait(false);
                    string callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);

                    await _signInManager.SignInAsync(user, isPersistent: false).ConfigureAwait(false);
                    _logger.LogInformation("User created a new account with password.");
                    return RedirectToLocal(returnUrl);
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync().ConfigureAwait(false);
            _logger.LogInformation("User logged out.");
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            string redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
            AuthenticationProperties properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToAction(nameof(Login));
            }
            ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync().ConfigureAwait(false);
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            // Sign in the user with this external login provider if the user already has a login.
            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true).ConfigureAwait(false);
            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in with {Name} provider.", info.LoginProvider);
                return RedirectToLocal(returnUrl);
            }
            if (result.IsLockedOut)
            {
                return RedirectToAction(nameof(Lockout));
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                ViewData["ReturnUrl"] = returnUrl;
                ViewData["LoginProvider"] = info.LoginProvider;
                string email = info.Principal.FindFirstValue(ClaimTypes.Email);
                return View("ExternalLogin", new ExternalLoginViewModel { Email = email });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync().ConfigureAwait(false);
                if (info == null)
                {
                    throw new ApplicationException("Error loading external login information during confirmation.");
                }
                ApplicationUser user = new() { UserName = model.Email, Email = model.Email };
                IdentityResult result = await _userManager.CreateAsync(user).ConfigureAwait(false);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info).ConfigureAwait(false);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false).ConfigureAwait(false);
                        _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(nameof(ExternalLogin), model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            ApplicationUser user = await _userManager.FindByIdAsync(userId).ConfigureAwait(false);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{userId}'.");
            }
            IdentityResult result = await _userManager.ConfirmEmailAsync(user, code).ConfigureAwait(false);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.FindByEmailAsync(model.Email).ConfigureAwait(false);
                if (user == null)
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToAction(nameof(ForgotPasswordConfirmation));
                }

                // For more information on how to enable account confirmation and password reset please
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                string code = await _userManager.GeneratePasswordResetTokenAsync(user).ConfigureAwait(false);
                string callbackUrl = Url.ResetPasswordCallbackLink(user.Id, code, Request.Scheme);

                //_unitOfWork.EmailMessagesRepository.Add(_unitOfWork.EmailTemplateRepository.ResetPasswordEmail(code, model.Email));

                return RedirectToAction(nameof(ForgotPasswordConfirmation));
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            if (code == null)
            {
                throw new ApplicationException("A code must be supplied for password reset.");
            }
            ResetPasswordViewModel model = new() { Code = code };
            return View(model);
        }

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }
        //    ApplicationUser user = await _userManager.FindByEmailAsync(model.Email).ConfigureAwait(false);
        //    if (user == null)
        //    {
        //        // Don't reveal that the user does not exist
        //        return RedirectToAction(nameof(ResetPasswordConfirmation));
        //    }
        //    IdentityResult result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password).ConfigureAwait(false);
        //    if (result.Succeeded)
        //    {
        //        return RedirectToAction(nameof(ResetPasswordConfirmation));
        //    }
        //    AddErrors(result);
        //    return View();
        //}

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            ApplicationUser user = await _userManager.Users.FirstOrDefaultAsync(m => m.Email == model.Email).ConfigureAwait(false);
            if (user == null)
            {
                return NotFound();
            }

            try
            {
                user.Email = user.Email;
                user.Enabled = user.Enabled;
                user.ChangePassword = false;
                user.SecurityStamp = Guid.NewGuid().ToString();
                user.LastPasswordChangedDate = DateTime.Today;
                IdentityResult userTask = await _userManager.UpdateAsync(user).ConfigureAwait(false);
                string userToken = await _userManager.GeneratePasswordResetTokenAsync(user).ConfigureAwait(false);
                IdentityResult result = await _userManager.ResetPasswordAsync(user, userToken, model.Password).ConfigureAwait(false);

                if (result.Errors.Any())
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError(nameof(model.Password), error.Description);
                    }
                    return View();
                }

                return RedirectToAction("Index", "Home", new { area = string.Empty });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(user.Email))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        private bool UserExists(string email)
        {
            return _userManager.Users.Any(r => r.Email == email);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult ContactUs(string companyName, string address, string city, string zip, string province, string phoneNumber, string emailAddress)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            const string resourceName = "BassoLegnami.Resources.EmailTemplates.ContactUs.html";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new(stream))
            {
                string mailtemplate = reader.ReadToEnd();
                _unitOfWork.EmailMessagesRepository.Add(new Model.Models.EmailMessage(_unitOfWork.SettingsRepository.GetByKey("System.ContactEmailAddresses").Value, _sharedResourceLocalizer["ContactUsSubject"].Value,
                    mailtemplate
                    .Replace("{ApplicationUrl}", _applicationConfigurations.Value.ApplicationURL)
                    .Replace("{Date}", $"{DateTime.Now.ToShortDateString()}  {DateTime.Now.ToLongTimeString()}")
                    .Replace("{CompanyName}", companyName)
                    .Replace("{Address}", address)
                    .Replace("{City}", city)
                    .Replace("{ZIP}", zip)
                    .Replace("{Province}", province)
                    .Replace("{PhoneNumber}", phoneNumber)
                    .Replace("{EmailAddress}", emailAddress),
                    Model.Models.EmailMessage.PrioritySendEmailValueEnum.Medium
                ));
                _unitOfWork.Save();
            }
            return Json(new { result = true });
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        #endregion
    }
}
