using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using In.Core.Models;
using BassoLegnami.Models.ManageViewModels;

namespace BassoLegnami.Controllers
{
	[Authorize]
	[Route("[controller]/[action]")]
	public class ManageController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly ILogger _logger;
		private readonly UrlEncoder _urlEncoder;

		private const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";
		private const string RecoveryCodesKey = nameof(RecoveryCodesKey);

		public ManageController(
		  UserManager<ApplicationUser> userManager,
		  SignInManager<ApplicationUser> signInManager,
		  ILogger<ManageController> logger,
		  UrlEncoder urlEncoder)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_logger = logger;
			_urlEncoder = urlEncoder;
		}

		[TempData]
		public string StatusMessage { get; set; }

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			ApplicationUser user = await _userManager.GetUserAsync(User).ConfigureAwait(false);
			if (user == null)
			{
				throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
			}

			IndexViewModel model = new()
			{
				Username = user.UserName,
				Email = user.Email,
				PhoneNumber = user.PhoneNumber,
				IsEmailConfirmed = user.EmailConfirmed,
				StatusMessage = StatusMessage
			};

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Index(IndexViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			ApplicationUser user = await _userManager.GetUserAsync(User).ConfigureAwait(false);
			if (user == null)
			{
				throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
			}

			string email = user.Email;
			if (model.Email != email)
			{
				IdentityResult setEmailResult = await _userManager.SetEmailAsync(user, model.Email).ConfigureAwait(false);
				if (!setEmailResult.Succeeded)
				{
					throw new ApplicationException($"Unexpected error occurred setting email for user with ID '{user.Id}'.");
				}
			}

			string phoneNumber = user.PhoneNumber;
			if (model.PhoneNumber != phoneNumber)
			{
				IdentityResult setPhoneResult = await _userManager.SetPhoneNumberAsync(user, model.PhoneNumber).ConfigureAwait(false);
				if (!setPhoneResult.Succeeded)
				{
					throw new ApplicationException($"Unexpected error occurred setting phone number for user with ID '{user.Id}'.");
				}
			}

			StatusMessage = "Your profile has been updated";
			return RedirectToAction(nameof(Index));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> SendVerificationEmail(IndexViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			ApplicationUser user = await _userManager.GetUserAsync(User).ConfigureAwait(false);
			if (user == null)
			{
				throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
			}

			string code = await _userManager.GenerateEmailConfirmationTokenAsync(user).ConfigureAwait(false);
			string callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
			string email = user.Email;

			StatusMessage = "Verification email sent. Please check your email.";
			return RedirectToAction(nameof(Index));
		}

		[HttpGet]
		public async Task<IActionResult> ChangePassword()
		{
			ApplicationUser user = await _userManager.GetUserAsync(User).ConfigureAwait(false);
			if (user == null)
			{
				throw new ApplicationException($"Impossibile caricare l'utente con ID '{_userManager.GetUserId(User)}'.");
			}

			bool hasPassword = await _userManager.HasPasswordAsync(user).ConfigureAwait(false);
			if (!hasPassword)
			{
				return RedirectToAction(nameof(SetPassword));
			}

			ChangePasswordViewModel model = new() { StatusMessage = StatusMessage };
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			ApplicationUser user = await _userManager.GetUserAsync(User).ConfigureAwait(false);
			if (user == null)
			{
				throw new ApplicationException($"Impossibile caricare l'utente con ID '{_userManager.GetUserId(User)}'.");
			}

			IdentityResult changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword).ConfigureAwait(false);
			if (!changePasswordResult.Succeeded)
			{
				AddErrors(changePasswordResult);
				return View(model);
			}

			await _signInManager.SignInAsync(user, isPersistent: false).ConfigureAwait(false);

			ApplicationUser t = await _userManager.FindByIdAsync(user.Id).ConfigureAwait(false);
			t.ChangePassword = false;
			t.LastPasswordChangedDate = DateTime.Today;
			IdentityResult userTask = await _userManager.UpdateAsync(t).ConfigureAwait(false);

			_logger.LogInformation("User changed their password successfully.");
			StatusMessage = "Password modificata con successo.";

			return RedirectToAction(nameof(ChangePassword));
		}

		[HttpGet]
		public async Task<IActionResult> SetPassword()
		{
			ApplicationUser user = await _userManager.GetUserAsync(User).ConfigureAwait(false);
			if (user == null)
			{
				throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
			}

			bool hasPassword = await _userManager.HasPasswordAsync(user).ConfigureAwait(false);

			if (hasPassword)
			{
				return RedirectToAction(nameof(ChangePassword));
			}

			SetPasswordViewModel model = new() { StatusMessage = StatusMessage };
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> SetPassword(SetPasswordViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			ApplicationUser user = await _userManager.GetUserAsync(User).ConfigureAwait(false);
			if (user == null)
			{
				throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
			}

			IdentityResult addPasswordResult = await _userManager.AddPasswordAsync(user, model.NewPassword).ConfigureAwait(false);
			if (!addPasswordResult.Succeeded)
			{
				AddErrors(addPasswordResult);
				return View(model);
			}

			await _signInManager.SignInAsync(user, isPersistent: false).ConfigureAwait(false);
			StatusMessage = "Your password has been set.";

			return RedirectToAction(nameof(SetPassword));
		}

		[HttpGet]
		public async Task<IActionResult> ExternalLogins()
		{
			ApplicationUser user = await _userManager.GetUserAsync(User).ConfigureAwait(false);
			if (user == null)
			{
				throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
			}

			ExternalLoginsViewModel model = new() { CurrentLogins = await _userManager.GetLoginsAsync(user).ConfigureAwait(false) };
			model.OtherLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync().ConfigureAwait(false))
				.Where(auth => model.CurrentLogins.All(ul => auth.Name != ul.LoginProvider))
				.ToList();
			model.ShowRemoveButton = await _userManager.HasPasswordAsync(user).ConfigureAwait(false) || model.CurrentLogins.Count > 1;
			model.StatusMessage = StatusMessage;

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> LinkLogin(string provider)
		{
			// Clear the existing external cookie to ensure a clean login process
			await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme).ConfigureAwait(false);

			// Request a redirect to the external login provider to link a login for the current user
			string redirectUrl = Url.Action(nameof(LinkLoginCallback));
			AuthenticationProperties properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl, _userManager.GetUserId(User));
			return new ChallengeResult(provider, properties);
		}

		[HttpGet]
		public async Task<IActionResult> LinkLoginCallback()
		{
			ApplicationUser user = await _userManager.GetUserAsync(User).ConfigureAwait(false);
			if (user == null)
			{
				throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
			}

			ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync(user.Id).ConfigureAwait(false);
			if (info == null)
			{
				throw new ApplicationException($"Unexpected error occurred loading external login info for user with ID '{user.Id}'.");
			}

			IdentityResult result = await _userManager.AddLoginAsync(user, info).ConfigureAwait(false);
			if (!result.Succeeded)
			{
				throw new ApplicationException($"Unexpected error occurred adding external login for user with ID '{user.Id}'.");
			}

			// Clear the existing external cookie to ensure a clean login process
			await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme).ConfigureAwait(false);

			StatusMessage = "The external login was added.";
			return RedirectToAction(nameof(ExternalLogins));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> RemoveLogin(RemoveLoginViewModel model)
		{
			ApplicationUser user = await _userManager.GetUserAsync(User).ConfigureAwait(false);
			if (user == null)
			{
				throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
			}

			IdentityResult result = await _userManager.RemoveLoginAsync(user, model.LoginProvider, model.ProviderKey).ConfigureAwait(false);
			if (!result.Succeeded)
			{
				throw new ApplicationException($"Unexpected error occurred removing external login for user with ID '{user.Id}'.");
			}

			await _signInManager.SignInAsync(user, isPersistent: false).ConfigureAwait(false);
			StatusMessage = "The external login was removed.";
			return RedirectToAction(nameof(ExternalLogins));
		}

		[HttpGet]
		public async Task<IActionResult> TwoFactorAuthentication()
		{
			ApplicationUser user = await _userManager.GetUserAsync(User).ConfigureAwait(false);
			if (user == null)
			{
				throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
			}

			TwoFactorAuthenticationViewModel model = new()
			{
				HasAuthenticator = await _userManager.GetAuthenticatorKeyAsync(user).ConfigureAwait(false) != null,
				Is2faEnabled = user.TwoFactorEnabled,
				RecoveryCodesLeft = await _userManager.CountRecoveryCodesAsync(user).ConfigureAwait(false),
			};

			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> Disable2faWarning()
		{
			ApplicationUser user = await _userManager.GetUserAsync(User).ConfigureAwait(false);
			if (user == null)
			{
				throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
			}

			if (!user.TwoFactorEnabled)
			{
				throw new ApplicationException($"Unexpected error occured disabling 2FA for user with ID '{user.Id}'.");
			}

			return View(nameof(Disable2fa));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Disable2fa()
		{
			ApplicationUser user = await _userManager.GetUserAsync(User).ConfigureAwait(false);
			if (user == null)
			{
				throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
			}

			IdentityResult disable2faResult = await _userManager.SetTwoFactorEnabledAsync(user, false).ConfigureAwait(false);
			if (!disable2faResult.Succeeded)
			{
				throw new ApplicationException($"Unexpected error occured disabling 2FA for user with ID '{user.Id}'.");
			}

			_logger.LogInformation("User with ID {UserId} has disabled 2fa.", user.Id);
			return RedirectToAction(nameof(TwoFactorAuthentication));
		}

		[HttpGet]
		public async Task<IActionResult> EnableAuthenticator()
		{
			ApplicationUser user = await _userManager.GetUserAsync(User).ConfigureAwait(false);
			if (user == null)
			{
				throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
			}

			EnableAuthenticatorViewModel model = new();
			await LoadSharedKeyAndQrCodeUriAsync(user, model).ConfigureAwait(false);

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> EnableAuthenticator(EnableAuthenticatorViewModel model)
		{
			ApplicationUser user = await _userManager.GetUserAsync(User).ConfigureAwait(false);
			if (user == null)
			{
				throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
			}

			if (!ModelState.IsValid)
			{
				await LoadSharedKeyAndQrCodeUriAsync(user, model).ConfigureAwait(false);
				return View(model);
			}

			// Strip spaces and hypens
			string verificationCode = model.Code.Replace(" ", string.Empty).Replace("-", string.Empty);

			bool is2faTokenValid = await _userManager.VerifyTwoFactorTokenAsync(user, _userManager.Options.Tokens.AuthenticatorTokenProvider, verificationCode).ConfigureAwait(false);

			if (!is2faTokenValid)
			{
				ModelState.AddModelError("Code", "Verification code is invalid.");
				await LoadSharedKeyAndQrCodeUriAsync(user, model).ConfigureAwait(false);
				return View(model);
			}

			await _userManager.SetTwoFactorEnabledAsync(user, true).ConfigureAwait(false);
			_logger.LogInformation("User with ID {UserId} has enabled 2FA with an authenticator app.", user.Id);
			IEnumerable<string> recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10).ConfigureAwait(false);
			TempData[RecoveryCodesKey] = recoveryCodes.ToArray();

			return RedirectToAction(nameof(ShowRecoveryCodes));
		}

		[HttpGet]
		public IActionResult ShowRecoveryCodes()
		{
			string[] recoveryCodes = (string[])TempData[RecoveryCodesKey];
			if (recoveryCodes == null)
			{
				return RedirectToAction(nameof(TwoFactorAuthentication));
			}

			ShowRecoveryCodesViewModel model = new() { RecoveryCodes = recoveryCodes };
			return View(model);
		}

		[HttpGet]
		public IActionResult ResetAuthenticatorWarning()
		{
			return View(nameof(ResetAuthenticator));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ResetAuthenticator()
		{
			ApplicationUser user = await _userManager.GetUserAsync(User).ConfigureAwait(false);
			if (user == null)
			{
				throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
			}

			await _userManager.SetTwoFactorEnabledAsync(user, false).ConfigureAwait(false);
			await _userManager.ResetAuthenticatorKeyAsync(user).ConfigureAwait(false);
			_logger.LogInformation("User with id '{UserId}' has reset their authentication app key.", user.Id);

			return RedirectToAction(nameof(EnableAuthenticator));
		}

		[HttpGet]
		public async Task<IActionResult> GenerateRecoveryCodesWarning()
		{
			ApplicationUser user = await _userManager.GetUserAsync(User).ConfigureAwait(false);
			if (user == null)
			{
				throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
			}

			if (!user.TwoFactorEnabled)
			{
				throw new ApplicationException($"Cannot generate recovery codes for user with ID '{user.Id}' because they do not have 2FA enabled.");
			}

			return View(nameof(GenerateRecoveryCodes));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> GenerateRecoveryCodes()
		{
			ApplicationUser user = await _userManager.GetUserAsync(User).ConfigureAwait(false);
			if (user == null)
			{
				throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
			}

			if (!user.TwoFactorEnabled)
			{
				throw new ApplicationException($"Cannot generate recovery codes for user with ID '{user.Id}' as they do not have 2FA enabled.");
			}

			IEnumerable<string> recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10).ConfigureAwait(false);
			_logger.LogInformation("User with ID {UserId} has generated new 2FA recovery codes.", user.Id);

			ShowRecoveryCodesViewModel model = new() { RecoveryCodes = recoveryCodes.ToArray() };

			return View(nameof(ShowRecoveryCodes), model);
		}

		#region Helpers

		private void AddErrors(IdentityResult result)
		{
			foreach (IdentityError error in result.Errors)
			{
				ModelState.AddModelError(string.Empty, error.Description);
			}
		}

		private string FormatKey(string unformattedKey)
		{
			StringBuilder result = new();
			int currentPosition = 0;
			while (currentPosition + 4 < unformattedKey.Length)
			{
				result.Append(unformattedKey, currentPosition, 4).Append(" ");
				currentPosition += 4;
			}
			if (currentPosition < unformattedKey.Length)
			{
				result.Append(unformattedKey.Substring(currentPosition));
			}

			return result.ToString().ToLowerInvariant();
		}

		private string GenerateQrCodeUri(string email, string unformattedKey)
		{
			return string.Format(
				AuthenticatorUriFormat,
				_urlEncoder.Encode("BassoLegnami"),
				_urlEncoder.Encode(email),
				unformattedKey);
		}

		private async Task LoadSharedKeyAndQrCodeUriAsync(ApplicationUser user, EnableAuthenticatorViewModel model)
		{
			string unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user).ConfigureAwait(false);
			if (string.IsNullOrEmpty(unformattedKey))
			{
				await _userManager.ResetAuthenticatorKeyAsync(user).ConfigureAwait(false);
				unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user).ConfigureAwait(false);
			}

			model.SharedKey = FormatKey(unformattedKey);
			model.AuthenticatorUri = GenerateQrCodeUri(user.Email, unformattedKey);
		}

		#endregion
	}
}
