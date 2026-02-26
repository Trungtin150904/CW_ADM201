using StudentManager.Common.DTOs;
using StudentManager.Data.Entities;

namespace StudentManager.Data.Interfaces
{
    public interface IStudentService
    {
        Task<bool> Create(StudentDTO dtoStudent);
        Task<StudentDTO?> GetById(Guid idStudent);
        Task<StudentDTO[]?> GetAll();
        Task<StudentDTO?> GetByIdWithMajor(Guid idStudent);
        Task<bool> Update(StudentDTO studentDTO);
        Task<bool> Delete(Guid idStudent);
    }
}
