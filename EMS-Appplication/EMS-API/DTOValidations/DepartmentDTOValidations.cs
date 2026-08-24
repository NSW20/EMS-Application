using EMS_Core.DTOs;
using FluentValidation;
using System.Data;

namespace EMS_API.DTOValidations
{
    public class DepartmentDTOValidations:AbstractValidator<DepartmentAddDTO>
    {
        public DepartmentDTOValidations()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Please provide the department name");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Please provide the department description");
          
        }
    }
    public class DepartmentUpdateDTOValidations : AbstractValidator<DepartmentDTO>
    {
        public DepartmentUpdateDTOValidations()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Please provide the department name");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Please provide the department description");
            RuleFor(x => x.DepartmentId).NotEmpty().WithMessage("please provide the department Id");

        }
    }
}
