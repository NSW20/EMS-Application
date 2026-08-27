using EMS_Core.DTOs;
using FluentValidation;

namespace EMS_API.DTOValidations
{
    public class RegisterDTOValidator:AbstractValidator<RegisterDTO>
    {
        public RegisterDTOValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Please enter your name.");
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Please enter your username.");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Invalid Email.Please check your email.");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Please enter your email.");
            RuleFor(x => x.Phone).NotEmpty().WithMessage("Please enter your phone number.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Please enter password.");
            RuleFor(x => x.Password).MinimumLength(8).WithMessage("Password Should be atleast 8 character.");
            RuleFor(x=>x.ConfirmPassword).NotEmpty().WithMessage("Please enter your password again.");
            RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage("Password Should be atleast 8 character.");
            RuleFor(x => x.Password).Equal(x => x.ConfirmPassword).WithMessage("Your password do not match.");
        }
    }
    public class LoginDTOValidator : AbstractValidator<LoginDTO>
    {
        public LoginDTOValidator()
        {
            RuleFor(x => x.Email).EmailAddress().WithMessage("Invalid Email.Please check your email.");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Please enter your email.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Please enter password.");
            RuleFor(x => x.Password).MinimumLength(8).WithMessage("Password Should be atleast 8 character.");
        }
    }
}
