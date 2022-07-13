using FYP.DatabaseModel;
using FYP.Models.AdminModel;
using FYP.Models.CandidateModel;
using FYP.Models.VotingModel;
using FYP.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BCryptNet = BCrypt.Net.BCrypt;

namespace FYP.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminRepository _adminRepo;
        private readonly IVotingRepository _voteRepo;
        private readonly ICandidateRepository _candidateRepo;

        public AdminController(IAdminRepository adminRepo, IVotingRepository voteRepo, ICandidateRepository candidateRepo)
        {
            _adminRepo = adminRepo;
            _voteRepo = voteRepo;
            _candidateRepo = candidateRepo;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(Register user)
        {
            if (ModelState.IsValid)
            {
                var checkEmail = _adminRepo.GetAdmin(user.Email);
                if (checkEmail == null)
                {
                    user.AdminID = Guid.NewGuid().ToString();
                    user.Password = BCryptNet.HashPassword(user.Password);
                    user.ConfirmationPassword = BCryptNet.HashPassword(user.ConfirmationPassword);
                    _adminRepo.RegisterAdmin(user.AdminID, user.Name, user.Email, user.Company, user.Password, user.PhoneNumber, user.DoB);

                    return RedirectToAction("Login", "Admin");
                }

                ViewBag.Message = "Email has been registered!";
                return View();
            }

            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginAsync(Login user)
        {
            if (ModelState.IsValid)
            {
                var checkUser = _adminRepo.GetAdmin(user.Email);
                if (checkUser != null)
                {
                    var verifyPassword = BCryptNet.Verify(user.Password, checkUser.admin_password);
                    if (verifyPassword)
                    {
                        var claims = new List<Claim>();
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, checkUser.admin_id));
                        claims.Add(new Claim(ClaimTypes.Name, checkUser.admin_email));
                        claims.Add(new Claim(ClaimTypes.Role, checkUser.admin_role));
                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                        await HttpContext.SignInAsync(claimsPrincipal);

                        return RedirectToAction("Dashboard", "Voting");
                    }
                    ViewBag.Message = "Email or Password is invalid!";
                }
                ViewBag.Message = "Email or Password is invalid!";
                return View();
            }
            return View();
        }

        [Authorize]
        public IActionResult Profile()
        {
            var adminID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (adminID == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                var profile = _adminRepo.GetAdminID(adminID);

                return View(profile);
            }
        }

        [Authorize]
        public IActionResult Edit(string adminID)
        {
            var profile = _adminRepo.GetAdminID(adminID);

            return View(profile);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EditProfile user)
        {
            if (ModelState.IsValid)
            {
                var adminID = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (adminID == null)
                {
                    return RedirectToAction("Login", "Admin");
                }
                else
                {
                    var admin = _adminRepo.GetAdminID(adminID);
                    if (admin == null)
                    {
                        return NotFound();
                    }
                    var checkEmail = _adminRepo.CheckEmail(user.admin_email);
                    if (checkEmail == null || admin.admin_email == user.admin_email && adminID == user.admin_id)
                    {
                        var checkPhone = _adminRepo.CheckPhone(user.admin_phone);
                        if (checkPhone == null || admin.admin_phone == user.admin_phone && adminID == user.admin_id)
                        {
                            _adminRepo.EditAdmin(user.admin_id, user.admin_name, user.admin_email, user.admin_company, user.admin_phone, user.admin_dob);

                            return RedirectToAction("Profile", "Admin");
                        }
                        ViewBag.Message = "Phone Number has been registered";
                        return View();
                    }
                    ViewBag.Message = "Email has been Registered";
                    return View();
                }
            }
            return View();
        }

        [Authorize]
        public IActionResult Password()
        {

            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Password(ChangePassword user)
        {
            if (ModelState.IsValid)
            {
                var adminID = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (adminID == null)
                {
                    return RedirectToAction("Login", "Admin");
                }

                var checkPassword = _adminRepo.CheckPassword(adminID);
                var verifyPassword = BCryptNet.Verify(user.OldPassword, checkPassword.admin_password);
                if (verifyPassword)
                {
                    user.NewPassword = BCryptNet.HashPassword(user.NewPassword);
                    user.ConfirmNewPassword = BCryptNet.HashPassword(user.ConfirmNewPassword);
                    _adminRepo.ChangePass(adminID, user.NewPassword);
                    return RedirectToAction("Profile", "Admin");
                }
                ViewBag.Message = "Your old password was entered incorrectly. Please enter it again.";
                return View();
            }
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            //SignOutAsync is Extension method for SignOut    
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return LocalRedirect("/");
        }
    }
}
