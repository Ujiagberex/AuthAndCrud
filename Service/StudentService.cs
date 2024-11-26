using WebApplication1.Data;
using WebApplication1.DTO;
using WebApplication1.IService;
using WebApplication1.Models;

namespace WebApplication1.Service
{
    public class StudentService : IStudentService
    {
        private readonly StudentDbContext studentDbContext;

        public StudentService(StudentDbContext studentDbContext)
        {
            this.studentDbContext = studentDbContext;
        }

        public string CreateStudent(Student student)
        {
            studentDbContext.Students.Add(student);
            studentDbContext.SaveChanges();
            return "Student Successfully created";
        }

        public void DeleteStudent(Student student)
        {
            studentDbContext.Students.Remove(student);
            studentDbContext.SaveChanges();
        }

        public List<Student> GetAllStudents()
        {
            return studentDbContext.Students.ToList();
        }

        public Student GetStudentById(int id)
        {
            Student student = studentDbContext.Students.Find(id);
            return student;
        }

        public Student GetStudentByName(string name)
        {
            var student = studentDbContext.Students.Where(st => st.Name == name).FirstOrDefault();
            return student;
        }

        public void UpdateStudent(int id)
        {
            throw new NotImplementedException();
        }
    }
}
