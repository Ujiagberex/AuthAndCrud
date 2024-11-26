using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTO;
using WebApplication1.IService;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpPost]
        [Route("CreateNewStudent")]
        [Authorize(Roles ="User")]
        public IActionResult CreateStudent([FromBody] Student addstudent)
        {
            var res = _studentService.CreateStudent(addstudent);
            return Ok(res);
        }

        [HttpGet("GetStudentBy{id}")]
        public IActionResult GetStudentById(int id)
        {
            if (id == null)
            {
                return BadRequest("This Id does not exist");      
            }
            var student = _studentService.GetStudentById(id);
            if (student == null)
            {
                return NotFound("The Id is wrong");
            }
            return Ok(student);
        }

        [HttpGet("GetAllStudents")]
        [Authorize(Roles ="Admin")]
        public IActionResult GetAllStudents()
        {
            List<Student> students = _studentService.GetAllStudents();
            return Ok(students);
        }

        [HttpDelete("DeleteStudentBy{id}")]
        [Authorize(Roles ="Admin")]
        public IActionResult DeleteStudent(int id)
        {
            var getStudent = _studentService.GetStudentById(id);
            if (getStudent == null)
            {
                return NotFound("Id not found");
            }
            _studentService.DeleteStudent(getStudent);
            return Ok("Student successfully deleted");
        }

        [HttpGet("GetStudentByName")]
        public IActionResult GetStudentByName([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("This name doesn't exist or there is a gap between the name");
            }
            var response = _studentService.GetStudentByName(name);
            if (response == null)
            {
                return NotFound("The person was not found");
            }
            return Ok(response);

        }


    }
}
