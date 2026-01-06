using Microsoft.AspNetCore.Mvc;
using StudentApi.Data;
using StudentApi.Models;

namespace StudentApi.Controllers
{
 
        [Route("api/[controller]")]
        [ApiController]
        public class StudentController : Controller
        {
          private readonly StudentDbContext _context;
            public StudentController(StudentDbContext context)
            {
                this._context = context;
            }

            //GET https://localhost:7097/api/student     
            [HttpGet]
            public IActionResult GetAll()
            {
                var getallstudent = _context.StudentTbs.ToList();
                return Ok(new
                {
                    status = 200,
                    message = "Student list",
                    data = getallstudent
                });
            }


            //GET https://localhost:7097/api/student/1
            [HttpGet("{id}")]
            public IActionResult GetStudentById(int id)
            {
                var student = _context.StudentTbs.FirstOrDefault(s => s.StudentPkid == id);

                if (student == null)
                {
                    return NotFound(new
                    {
                        status = 404,
                        message = "Student not found"
                    });
                }

                return Ok(new
                {
                    status = 200,
                    data = student
                });
            }



            private static List<Student> _students = new List<Student>()
            {
                //new Student{ StudentPkid = 1, FullName = "Aung Aung", DateOfBirth = 20 },
                //new Student{ StudentPkid = 2, FullName = "Su Su", DateOfBirth = 22 }
            };

            //POST https://localhost:7097/api/student
            [HttpPost]
            public IActionResult CreateStudent([FromBody] Student student)
            {
                student.StudentPkid = _students.Max(s => s.StudentPkid) + 1;
                _students.Add(student);

                return Ok(new
                {
                    status = 201,
                    message = "Student created",
                    data = student
                });
            }

        
            [HttpPut("{id}")]
            public IActionResult UpdateStudent(int id, Student student)
            {
                var oldStudent = _students.FirstOrDefault(s => s.StudentPkid == id);

                if (oldStudent == null)
                {
                    return NotFound();
                }

                oldStudent.FullName = student.FullName;
                oldStudent.Email = student.Email;
                oldStudent.DateOfBirth = student.DateOfBirth;

                return Ok(new
                {
                    status = 200,
                    message = "Student updated",
                    data = oldStudent
                });
            }


            [HttpDelete("{id}")]
            public IActionResult DeleteStudent(int id)
            {
                var student = _students.FirstOrDefault(s => s.StudentPkid == id);

                if (student == null)
                {
                    return NotFound();
                }

                _students.Remove(student);

                return Ok(new
                {
                    status = 200,
                    message = "Student deleted"
                });
            }





        }
}



    
