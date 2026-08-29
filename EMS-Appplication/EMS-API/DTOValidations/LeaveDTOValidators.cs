using EMS_Core.DTOs;
using FluentValidation;

namespace EMS_API.DTOValidations
{
    public class LeaveDTOValidators : AbstractValidator<LeaveDTO>
    {
        public LeaveDTOValidators()
        {
            RuleFor(x => x.EmployeeId).NotEmpty().WithMessage("Please enter employee Id");
            RuleFor(x => x.LeaveId).NotEmpty().WithMessage("Please enter leave Id");
            RuleFor(x => x.LeaveType).NotEmpty().WithMessage("Please enter valid Leave type");
            RuleFor(x => x.Status).NotEmpty().WithMessage("Please enter Leave status");
        }
    }
    public class LeaveAddDTOValidators : AbstractValidator<LeaveAddDTO>
    {
        public LeaveAddDTOValidators()
        {
            RuleFor(x => x.EmployeeId).NotEmpty().WithMessage("Please enter employee Id");
            RuleFor(x => x.LeaveType).NotEmpty().WithMessage("Please enter valid Leave type");
            RuleFor(x => x.Status).NotEmpty().WithMessage("Please enter Leave status");
        }
    }
}
