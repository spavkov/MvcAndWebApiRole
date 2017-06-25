using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;
using CloudMockApi.Models;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;

namespace CloudMockApi.Admin.Services.Identity
{
    public class CustomUserValidator : UserValidator<ApplicationUser>
    {
        private readonly UserManager<ApplicationUser, string> manager;

        public CustomUserValidator(UserManager<ApplicationUser, string> manager) : base(manager)
        {
            this.manager = manager;
        }

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
    }
}