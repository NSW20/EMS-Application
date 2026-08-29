using EMS_Core.DTOs;
using FluentValidation;

namespace EMS_API.DTOValidations
{
    public class EmployeeDTOValidator:AbstractValidator<EmployeeDTO>
    {
        public EmployeeDTOValidator()
        {
            RuleFor(x => x.FullName).NotEmpty().WithMessage("Please enter your name");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Please enter your email");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Please enter valid email");
            RuleFor(x => x.Phone).NotEmpty().WithMessage("Please enter your phone number");
            RuleFor(x => x.DepartmentId).NotEmpty().WithMessage("Please enter your department Id");
            RuleFor(x => x.DesignationId).NotEmpty().WithMessage("Please enter your designation Id");
            RuleFor(x => x.Salary).NotEmpty().WithMessage("Please enter your salary");
            RuleFor(x => x.EmployeeId).NotEmpty().WithMessage("Please enter your employee Id");
        }
    }
    public class EmployeeAddDTOValidator : AbstractValidator<EmployeeAddDTO>
    {
        public EmployeeAddDTOValidator()
        {
            RuleFor(x => x.FullName).NotEmpty().WithMessage("Please enter your name");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Please enter your email");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Please enter valid email");
            RuleFor(x => x.Phone).NotEmpty().WithMessage("Please enter your phone number");
            RuleFor(x => x.DepartmentId).NotEmpty().WithMessage("Please enter your department Id");
            RuleFor(x => x.DesignationId).NotEmpty().WithMessage("Please enter your designation Id");
            RuleFor(x => x.Salary).NotEmpty().WithMessage("Please enter your salary");
        }
    }
}
