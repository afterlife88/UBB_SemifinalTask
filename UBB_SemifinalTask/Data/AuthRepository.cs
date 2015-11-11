using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using UBB_SemifinalTask.Models;

namespace UBB_SemifinalTask.Data
{
    public class AuthRepository : IDisposable
    {
        private readonly ApplicationDbContext _ctx;

        private readonly UserManager<ApplicationUser> _userManager;

        public AuthRepository()
        {
            _ctx = new ApplicationDbContext();
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_ctx));
        }

        public async Task<IdentityResult> RegisterUser(UserBindingModel userModel)
        {
            var user = new ApplicationUser()
            {
                Name = userModel.Name,
                Email = userModel.Email,
                Category = userModel.Category,
                UserName = userModel.Name
            };
            var result = await _userManager.CreateAsync(user, userModel.Password);

            return result;
        }

        public async Task<IdentityUser> FindUser(string userName, string password)
        {
            IdentityUser user = await _userManager.FindAsync(userName, password);

            return user;
        }

        public async Task<ApplicationUser> FindByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }
        public void Dispose()
        {
            _ctx.Dispose();
            _userManager.Dispose();

        }
    }
}
