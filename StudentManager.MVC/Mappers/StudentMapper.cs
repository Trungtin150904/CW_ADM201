using StudentManager.Common.DTOs;
using StudentManager.MVC.Models;

namespace StudentManager.MVC.Mappers
{
    public static class StudentMapper
    {
        // DTO -> ViewModel
        public static StudentVM ToViewModel(this StudentDTO dto)
        {
            if (dto == null) return null;

            return new StudentVM
            {
                Id = dto.Id,
                FullName = dto.FullName,
                Email = dto.Email,
                DateOfBirth = dto.DateOfBirth,
                Gender = dto.Gender,
                Address = dto.Address,
                Phone = dto.Phone,
                Hobby = dto.Hobby,
                Major = dto.Major,
                MajorId = dto.MajorId,
                StudentClassId = dto.StudentClassId,
                AvatarPath = dto.AvatarPath
            };
        }

        // ViewModel -> DTO
        public static StudentDTO ToDTO(this StudentVM vm)
        {
            if (vm == null) return null;

            return new StudentDTO
            {
                Id = vm.Id,
                FullName = vm.FullName,
                Email = vm.Email,
                DateOfBirth = vm.DateOfBirth,
                Gender = vm.Gender,
                Address = vm.Address,
                Phone = vm.Phone,
                Hobby = vm.Hobby,
                MajorId = vm.MajorId,
                Major = vm.Major,
                StudentClassId = vm.StudentClassId,
                AvatarPath = vm.AvatarPath
            };
        }
    }
}
