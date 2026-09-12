using EMS_Core.DTOs;
using FluentValidation;

namespace EMS_API.DTOValidations
{
    public class AttendaceDTOValidations:AbstractValidator<AttendanceDTO>
    {
        public AttendaceDTOValidations()
        {
            RuleFor(x => x.CheckIn).NotEmpty().WithMessage("Please provide the checkin time");
            RuleFor(x => x.EmployeeId).NotEmpty().WithMessage("Please provide the employee id");
            RuleFor(x => x.AttendanceId).NotEmpty().WithMessage("Please provide the attendace id");
        }
    }
    public class AttendaceAddDTOValidations : AbstractValidator<AttendanceAddDTO>
    {
        public AttendaceAddDTOValidations()
        {
            RuleFor(x => x.EmployeeId).NotEmpty().WithMessage("Please provide the employee id");
        }
    }
}
