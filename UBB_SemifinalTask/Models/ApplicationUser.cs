using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace UBB_SemifinalTask.Models
{
    /// <summary>
    /// Основной класс ОРМ таблицы юзеров
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public override string Email { get; set; }
        [Required]
        public string Category { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            return userIdentity;
        }
    }
}