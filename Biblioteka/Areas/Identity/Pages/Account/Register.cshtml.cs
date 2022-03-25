using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Biblioteka.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Biblioteka.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Biblioteka.Areas.Identity.Pages.Account
{
    [Authorize(Roles ="Admin,Bibliotekar")]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<Korisnik> _signInManager;
        private readonly UserManager<Korisnik> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext _context;

        public RegisterModel(
            UserManager<Korisnik> userManager,
            SignInManager<Korisnik> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [StringLength(10,ErrorMessage ="{0} ne sme imati više od {1} karaktera",MinimumLength =3)]
            public string Ime { get; set; }
            [Required]
            [StringLength(20, ErrorMessage = "{0} ne sme imati više od {1} karaktera", MinimumLength = 3)]
            public string Prezime { get; set; }
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "{0} mora imati imeđu {2} i {1} karaktera.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Lozinka")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Potvrdi lozinku")]
            [Compare("Password", ErrorMessage = "Lozinke se ne poklapaju.")]
            public string ConfirmPassword { get; set; }
            [Display(Name ="Tip naloga")]
            public string TipNaloga { get; set; }
            public List<SelectListItem> TipoviKorisnika { get; set; }
            
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            var tipovi = _context.TipoviKorisnika.ToList().Select(tipovi => new SelectListItem
            {
                Value = tipovi.Name,
                Text = tipovi.Name,
            }).ToList();
            Input = new InputModel();
            Input.TipoviKorisnika = tipovi; 
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
           
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                if(User.IsInRole("Admin") || Input.TipNaloga == "Korisnik")
                {//samo admin i bibliotekar imaju pristup ovom delu,admin moze bilo koji nalog da napravi,a bibliotkar samo korisnika
                    var user = new Korisnik { UserName = Input.Email, Email = Input.Email, Ime = Input.Ime, Prezime = Input.Prezime };
                    var result = await _userManager.CreateAsync(user, Input.Password);
                    var roleResult = await _userManager.AddToRoleAsync(user, Input.TipNaloga);//dodaj unutar transakcije
                    if (result.Succeeded && roleResult.Succeeded)
                    {
                        _logger.LogInformation("Kreiran nov nalog.");

                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var callbackUrl = Url.Page(
                            "/Account/ConfirmEmail",
                            pageHandler: null,
                            values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                            protocol: Request.Scheme);

                        await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                            $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                        if (_userManager.Options.SignIn.RequireConfirmedAccount)
                        {
                            return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                        }
                        else
                        {
                            //await _signInManager.SignInAsync(user, isPersistent: false);//posto korisnik ne registruje samog sebe
                            return LocalRedirect(returnUrl);
                        }
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    foreach (var error in roleResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                else
                {
                    ModelState.AddModelError("Input.TipNaloga", "Nemate dozvolu da kreirate korisnika sa ovim tipom naloga.");
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
