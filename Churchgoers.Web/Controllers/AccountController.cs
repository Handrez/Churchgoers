using Churchgoers.Common.Enums;
using Churchgoers.Common.Responses;
using Churchgoers.Web.Data;
using Churchgoers.Web.Data.Entities;
using Churchgoers.Web.Helpers;
using Churchgoers.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Vereyon.Web;

namespace Churchgoers.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly ICombosHelper _combosHelper;
        private readonly IBlobHelper _blobHelper;
        private readonly IMailHelper _mailHelper;
        private readonly IFlashMessage _flashMessage;

        public AccountController(DataContext context, IUserHelper userHelper, ICombosHelper combosHelper, IBlobHelper blobHelper, IMailHelper mailHelper, IFlashMessage flashMessage)
        {
            _context = context;
            _userHelper = userHelper;
            _combosHelper = combosHelper;
            _blobHelper = blobHelper;
            _mailHelper = mailHelper;
            _flashMessage = flashMessage;
        }

        [Authorize(Roles = "Admin, Teacher")]
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Admin"))
            {
                User user = await _userHelper.GetUserAsync(User.Identity.Name);
                ViewData["Members"] = UserType.Member;
            }

            if (User.IsInRole("Teacher"))
            {
                User user = await _userHelper.GetUserAsync(User.Identity.Name);
                ViewData["ChurchName"] = user.Church.Name;
            }

            return View(await _context.Users
                .Include(u => u.Church)
                .Include(p => p.Profession)
                .Where(p => p.UserType == UserType.Member)
                .ToListAsync());
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> IndexTeachers()
        {
            User user = await _userHelper.GetUserAsync(User.Identity.Name);
            ViewData["Teachers"] = UserType.Teacher;

            return View(await _context.Users
                .Include(u => u.Church)
                .Include(p => p.Profession)
                .ToListAsync());
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            AddUserViewModel model = new AddUserViewModel
            {
                Fields = _combosHelper.GetComboFields(),
                Districts = _combosHelper.GetComboDistricts(0),
                Churches = _combosHelper.GetComboChurches(0),
                Professions = _combosHelper.GetComboProfessions(),
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;

                if (model.ImageFile != null)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "users");
                }

                User user = await _userHelper.AddUserAsync(model, imageId, UserType.Teacher);
                if (user == null)
                {
                    _flashMessage.Danger("This email is already used.");
                    model.Fields = _combosHelper.GetComboFields();
                    model.Districts = _combosHelper.GetComboDistricts(model.FieldId);
                    model.Churches = _combosHelper.GetComboChurches(model.DistrictId);
                    model.Professions = _combosHelper.GetComboProfessions();
                    return View(model); 
                }

                string myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                string tokenLink = Url.Action("ConfirmEmail", "Account", new
                {
                    userid = user.Id,
                    token = myToken
                }, protocol: HttpContext.Request.Scheme);

                Response response = _mailHelper.SendMail(model.Username, "Email confirmation", $"<h1>Email Confirmation</h1>" +
                    $"To allow the user, " +
                    $"plase click in this link:</br></br><a href = \"{tokenLink}\">Confirm Email</a>");
                if (response.IsSuccess)
                {
                    _flashMessage.Confirmation("The instructions to allow your teacher has been sent to email.");
                    return View(model);
                }

                ModelState.AddModelError(string.Empty, response.Message);
            }

            model.Fields = _combosHelper.GetComboFields();
            model.Districts = _combosHelper.GetComboDistricts(model.FieldId);
            model.Churches = _combosHelper.GetComboChurches(model.DistrictId);
            model.Professions = _combosHelper.GetComboProfessions();
            return View(model);
        }
        
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _userHelper.LoginAsync(model);
                if (result.Succeeded)
                {
                    if (Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return Redirect(Request.Query["ReturnUrl"].First());
                    }

                    return RedirectToAction("Index", "Home");
                }

                _flashMessage.Danger("Email or password incorrect.");
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _userHelper.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult NotAuthorized()
        {
            return View();
        }

        public IActionResult Register()
        {
            AddUserViewModel model = new AddUserViewModel
            {
                Professions = _combosHelper.GetComboProfessions(),
                Fields = _combosHelper.GetComboFields(),
                Districts = _combosHelper.GetComboDistricts(0),
                Churches = _combosHelper.GetComboChurches(0),
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(AddUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;

                if (model.ImageFile != null)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "users");
                }

                User user = await _userHelper.AddUserAsync(model, imageId, UserType.Member);
                if (user == null)
                {
                    _flashMessage.Danger("This email is already used.");
                    model.Professions = _combosHelper.GetComboProfessions();
                    model.Fields = _combosHelper.GetComboFields();
                    model.Districts = _combosHelper.GetComboDistricts(model.FieldId);
                    model.Churches = _combosHelper.GetComboChurches(model.DistrictId);
                    return View(model);
                }

                string myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                string tokenLink = Url.Action("ConfirmEmail", "Account", new
                {
                    userid = user.Id,
                    token = myToken
                }, protocol: HttpContext.Request.Scheme);

                Response response = _mailHelper.SendMail(model.Username, "Email confirmation", $"<h1>Email Confirmation</h1>" +
                    $"To allow the user, " +
                    $"plase click in this link:<p><a href = \"{tokenLink}\">Confirm Email</a></p>");
                if (response.IsSuccess)
                {
                    _flashMessage.Confirmation("The instructions to allow your user has been sent to email.");
                    return View(model);
                }

                ModelState.AddModelError(string.Empty, response.Message);
            }

            model.Professions = _combosHelper.GetComboProfessions();
            model.Fields = _combosHelper.GetComboFields();
            model.Districts = _combosHelper.GetComboDistricts(model.FieldId);
            model.Churches = _combosHelper.GetComboChurches(model.DistrictId);
            return View(model);
        }


        public JsonResult GetDistricts(int fieldId)
        {
            Field field = _context.Fields
                .Include(f => f.Districts)
                .FirstOrDefault(c => c.Id == fieldId);
            if (field == null)
            {
                return null;
            }

            return Json(field.Districts.OrderBy(d => d.Name));
        }

        public JsonResult GetChurches(int districtId)
        {
            District district = _context.Districts
                .Include(d => d.Churches)
                .FirstOrDefault(d => d.Id == districtId);
            if (district == null)
            {
                return null;
            }

            return Json(district.Churches.OrderBy(c => c.Name));
        }

        public async Task<IActionResult> ChangeUser()
        {
            User user = await _userHelper.GetUserAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }

            District district = await _context.Districts.FirstOrDefaultAsync(d => d.Churches.FirstOrDefault(c => c.Id == user.Church.Id) != null);
            if (district == null)
            {
                district = await _context.Districts.FirstOrDefaultAsync();
            }

            Field field = await _context.Fields.FirstOrDefaultAsync(f => f.Districts.FirstOrDefault(d => d.Id == district.Id) != null);
            if (field == null)
            {
                field = await _context.Fields.FirstOrDefaultAsync();
            }

            EditUserViewModel model = new EditUserViewModel
            {
                Address = user.Address,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                ImageId = user.ImageId,
                Professions = _combosHelper.GetComboProfessions(),
                ProfessionId = user.Profession.Id,
                Churches = _combosHelper.GetComboChurches(district.Id),
                ChurchId = user.Church.Id,
                Districts = _combosHelper.GetComboDistricts(field.Id),
                DistrictId = district.Id,
                Fields = _combosHelper.GetComboFields(),
                FieldId = field.Id,
                Id = user.Id,
                Document = user.Document
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeUser(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = model.ImageId;

                if (model.ImageFile != null)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "users");
                }

                User user = await _userHelper.GetUserAsync(User.Identity.Name);

                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Address = model.Address;
                user.PhoneNumber = model.PhoneNumber;
                user.ImageId = imageId;
                user.Church = await _context.Churches.FindAsync(model.ChurchId);
                user.Profession = await _context.Professions.FindAsync(model.ProfessionId);
                user.Document = model.Document;

                await _userHelper.UpdateUserAsync(user);
                return RedirectToAction("Index", "Home");
            }

            model.Professions = _combosHelper.GetComboProfessions();
            model.Fields = _combosHelper.GetComboFields();
            model.Districts = _combosHelper.GetComboDistricts(model.FieldId);
            model.Churches = _combosHelper.GetComboChurches(model.DistrictId);
            return View(model);
        }

        public IActionResult ChangePasswordMVC()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePasswordMVC(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userHelper.GetUserAsync(User.Identity.Name);
                if (user != null)
                {
                    IdentityResult result = await _userHelper.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        _flashMessage.Confirmation("Password changed successfully");
                        return RedirectToAction("ChangeUser");
                    }
                    else
                    {
                        _flashMessage.Confirmation("Could not change password");
                    }
                }
                else
                {
                    _flashMessage.Danger("User no found.");
                }
            }

            return View(model);
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return NotFound();
            }

            User user = await _userHelper.GetUserAsync(new Guid(userId));
            if (user == null)
            {
                return NotFound();
            }

            IdentityResult result = await _userHelper.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return NotFound();
            }

            return View();
        }

        public IActionResult RecoverPasswordMVC()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RecoverPasswordMVC(RecoverPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userHelper.GetUserAsync(model.Email);
                if (user == null)
                {
                    _flashMessage.Danger("The email doesn't correspont to a registered user.");
                    return View(model);
                }

                string myToken = await _userHelper.GeneratePasswordResetTokenAsync(user);
                string link = Url.Action(
                    "ResetPassword",
                    "Account",
                    new { token = myToken }, protocol: HttpContext.Request.Scheme);
                _mailHelper.SendMail(model.Email, "Password Reset", $"<h1>Password Reset</h1>" +
                    $"To reset the password click in this link:</br></br>" +
                    $"<a href = \"{link}\">Reset Password</a>");
                _flashMessage.Confirmation("The instructions to recover your password has been sent to email.");
                return View();

            }

            return View(model);
        }

        public IActionResult ResetPassword(string token)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            User user = await _userHelper.GetUserAsync(model.UserName);
            if (user != null)
            {
                IdentityResult result = await _userHelper.ResetPasswordAsync(user, model.Token, model.Password);
                if (result.Succeeded)
                {
                    _flashMessage.Confirmation("Password reset successful.");
                    return View();
                }

                _flashMessage.Danger("Error while resetting the password.");
                return View(model);
            }

            _flashMessage.Danger("User not found.");
            return View(model);
        }
    }
}
