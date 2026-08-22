using EMS_Core.DTOs;
using FluentValidation;

namespace EMS_API.DTOValidations
{
    public class DesignationDTOValidation:AbstractValidator<DesignationAddDTO>
    {
        public DesignationDTOValidation()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Please enter designation title");
            RuleFor(x => x.DepartmentId).NotEmpty().WithMessage("Please enter DepartmentId");
        }
    }
    public class DesignationDTOUpdateValidation : AbstractValidator<DesignationDTO>
    {
        public DesignationDTOUpdateValidation()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Please enter designation title");
            RuleFor(x => x.DesignationId).NotEmpty().WithMessage("Please enter DesignationId");
            RuleFor(x => x.DepartmentId).NotEmpty().WithMessage("Please enter DepartmentId");
        }
    }
}
