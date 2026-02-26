using StudentManager.Common.DTOs;

namespace StudentManager.Data.Interfaces
{
    public interface IStudentClassService
    {
        Task<StudentClassDTO[]?> GetAll();
    }
}
