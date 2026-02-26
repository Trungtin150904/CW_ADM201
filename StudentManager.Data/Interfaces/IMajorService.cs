using StudentManager.Common.DTOs;

namespace StudentManager.Data.Interfaces
{
    public interface IMajorService
    {
        //SelectList GetSelectList(int id);

        Task<MajorDTO[]?> GetAll();
    }
}
