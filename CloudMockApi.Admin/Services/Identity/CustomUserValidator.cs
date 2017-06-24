using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using CloudMockApi.Admin.Models;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;

namespace CloudMockApi.Services.Identity
{
    public class CustomUserValidator : UserValidator<ApplicationUser>
    {
        private readonly UserManager<ApplicationUser, string> manager;

        public CustomUserValidator(UserManager<ApplicationUser, string> manager, bool requireUniqueTenant) : base(manager)
        {
            RequireUniqueTenant = requireUniqueTenant;
            this.manager = manager;
        }

        public CustomUserValidator(UserManager<ApplicationUser, string> manager) : this(manager, true)
        {
            this.manager = manager;
        }

        public bool RequireUniqueTenant { get; set; }

        public override async Task<IdentityResult> ValidateAsync(ApplicationUser item)
        {



            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            var errors = new List<string>();

            if (RequireUniqueEmail)
            {
                await ValidateEmail(item, errors);
            }
            if (RequireUniqueTenant)
            {
                await ValidateTenant(item, errors);
            }
            if (errors.Count > 0)
            {
                return IdentityResult.Failed(errors.ToArray());
            }
            return IdentityResult.Success;
        }

        // make sure email is not empty, valid, and unique
        private async Task ValidateEmail(ApplicationUser user, List<string> errors)
        {
            if (!user.Email.IsNullOrWhiteSpace())
            {
                if (string.IsNullOrWhiteSpace(user.Email))
                {
                    errors.Add("Email needs to be unique");
                    return;
                }
                try
                {
                    var m = new MailAddress(user.Email);
                }
                catch (FormatException)
                {
                    errors.Add("Please specify properly formatted Email address");
                    return;
                }
            }
            var owner = await this.manager.FindByEmailAsync(user.Email);
            if (owner != null && !EqualityComparer<string>.Default.Equals(owner.Id, user.Id))
            {
                errors.Add("Email is already used. Try using another Email Address.");
            }
        }

        private async Task ValidateTenant(ApplicationUser user, List<string> errors)
        {
            if (string.IsNullOrWhiteSpace(user.Tenant))
            {
                errors.Add("Tennat cannot be empty.");
                return;
            }

            var owner = manager.Users.ToList().FirstOrDefault(x => x.Tenant == user.Tenant);
            if (owner != null && !EqualityComparer<string>.Default.Equals(owner.Id, user.Id))
            {
                errors.Add("Tenant must be unique. This tennat is already reserved. Try another one.");
            }
        }
    }
}