using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Library.Models.InputModels;
using System.Collections.Generic;
using System.Security.Claims;
using System.Linq;
using Library.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Library.Models.Entities;

namespace Library.Pages.Admin
{
    [Authorize(Roles = nameof(Role.Administrator))]
    public class UsersModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;

        public UsersModel(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        [BindProperty]
        public UserRoleInputModel Input { get; set; }
        public IList<ApplicationUser> Users { get; private set; }
        
        [BindProperty(SupportsGet = true)]
        public Role InRole { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            ViewData["Title"] = "Manage users";
            Claim claim = new Claim(ClaimTypes.Role, InRole.ToString());
            Users = await userManager.GetUsersForClaimAsync(claim);
            return Page();
        }

        public async Task<IActionResult> OnPostAssignAsync()
        {
            if (!ModelState.IsValid)
            {
                return await OnGetAsync();
            }
            ApplicationUser user = await userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                ModelState.AddModelError(nameof(Input.Email), $"The email {Input.Email} has not been registered");
                return await OnGetAsync();
            }
            IList<Claim> claims = await userManager.GetClaimsAsync(user);
            Claim roleClaim = new Claim(ClaimTypes.Role, Input.Role.ToString());
            if (claims.Any(claim => claim.Type == roleClaim.Type && claim.Value == roleClaim.Value))
            {
                ModelState.AddModelError(nameof(Input.Role), $"The role {Input.Role} is already assigned to the user with email {Input.Email}");
                return await OnGetAsync();
            }
            IdentityResult result = await userManager.AddClaimAsync(user, roleClaim);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, $"The operation has failed: {result.Errors.FirstOrDefault()?.Description}");
                return await OnGetAsync();
            }
            TempData["ConfirmationMessage"] = $"The role {Input.Role} has been assigned to the user with email {Input.Email}";
            return RedirectToPage(new { inrole = (int) InRole });
        }
        public async Task<IActionResult> OnPostRevokeAsync()
        {
            if (!ModelState.IsValid)
            {
                return await OnGetAsync();
            }
            ApplicationUser user = await userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                ModelState.AddModelError(nameof(Input.Email), $"The email {Input.Email} has not been registered");
                return await OnGetAsync();
            }
            IList<Claim> claims = await userManager.GetClaimsAsync(user);
            Claim roleClaim = new Claim(ClaimTypes.Role, Input.Role.ToString());
            if (!claims.Any(claim => claim.Type == roleClaim.Type && claim.Value == roleClaim.Value))
            {
                ModelState.AddModelError(nameof(Input.Role), $"The role {Input.Role} was not assigned to the user with email {Input.Email}");
                return await OnGetAsync();
            }
            IdentityResult result = await userManager.RemoveClaimAsync(user, roleClaim);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, $"The operation has failed: {result.Errors.FirstOrDefault()?.Description}");
                return await OnGetAsync();
            }
            TempData["ConfirmationMessage"] = $"The role {Input.Role} has been revoked from the user with email {Input.Email}";
            return RedirectToPage(new { inrole = (int)InRole });
        }

    }
}
