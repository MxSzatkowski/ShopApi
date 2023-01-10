using FluentValidation;
using ShopsApi.Entities;
using System.Linq;

namespace ShopsApi.Models.Validators
{
    public class RegisterUser : AbstractValidator<RegisterUserDto>
    {
        public RegisterUser(ShopDbContext dbContext)
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();
            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(8);
            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password);

            RuleFor(x => x.Email)
                .Custom((value, context) =>
                {
                    var emailInUse = dbContext.Users.Any(u => u.Email == value.ToString());
                    if (emailInUse)
                    {
                        context.AddFailure("Email", "That email is taken");
                    }
                });
        }
    }
}
