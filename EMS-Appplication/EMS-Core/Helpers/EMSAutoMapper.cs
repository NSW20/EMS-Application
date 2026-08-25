using EMS_Core.Domain.Entities;
using EMS_Core.DTOs;
using EMS_Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMS_Core.Helpers
{
    public class EMSAutoMapper:AutoMapper.Profile
    {
        public EMSAutoMapper()
        {
            // ---------- Attendance ----------
            CreateMap<Attendance, AttendanceDTO>()
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src => src.Status.HasValue ? src.Status.Value.ToString() : null));

            CreateMap<AttendanceDTO, Attendance>()
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src => ParseEnum<AttendenceStatus>(src.Status)));

            CreateMap<Attendance, AttendanceAddDTO>()
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src => src.Status.HasValue ? src.Status.Value.ToString() : null));

            CreateMap<AttendanceAddDTO, Attendance>()
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src => ParseEnum<AttendenceStatus>(src.Status)));

            // ---------- Employee ----------
            CreateMap<Employee, EmployeeDTO>()
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src => src.Status.HasValue ? src.Status.Value.ToString() : null));

            CreateMap<EmployeeDTO, Employee>()
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src => ParseEnum<EmployeeStatus>(src.Status)));

            CreateMap<Employee, EmployeeAddDTO>()
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src => src.Status.HasValue ? src.Status.Value.ToString() : null));

            CreateMap<EmployeeAddDTO, Employee>()
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src => ParseEnum<EmployeeStatus>(src.Status)));

            // ---------- Department ----------
            CreateMap<Department, DepartmentAddDTO>();
            CreateMap<DepartmentAddDTO, Department>();
            CreateMap<Department, DepartmentDTO>();
            CreateMap<DepartmentDTO, Department>();

            // ---------- Designation ----------
            CreateMap<Designation, DesignationAddDTO>();
            CreateMap<DesignationAddDTO, Designation>();
            CreateMap<Designation, DesignationDTO>();
            CreateMap<DesignationDTO, Designation>();

            // ---------- Leave ----------
            CreateMap<Leave, LeaveAddDTO>();
            CreateMap<LeaveAddDTO, Leave>();
            CreateMap<Leave, LeaveDTO>();
            CreateMap<LeaveDTO, Leave>();


        }
        public static TEnum? ParseEnum<TEnum>(string value) where TEnum : struct, Enum
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }
            return Enum.TryParse<TEnum>(value, ignoreCase: true, out var result) ? result :  (TEnum?)null;
        }
    }
}
