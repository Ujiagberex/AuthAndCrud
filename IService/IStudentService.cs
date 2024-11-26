using WebApplication1.DTO;
using WebApplication1.Models;

namespace WebApplication1.IService
{
    public interface IStudentService
    {
        //Create student
        string CreateStudent(Student student);
        //Get all students
        List<Student> GetAllStudents();
        //Get student by Id
        Student GetStudentById(int id);
        //Update student
        void UpdateStudent(int id);
        void DeleteStudent(Student student);
        //Get student by their name
        Student GetStudentByName(string name);

    }
}
