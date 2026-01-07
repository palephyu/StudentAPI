using Microsoft.AspNetCore.Mvc;
using StudentApi.Data;
using StudentApi.Models;
using Microsoft.EntityFrameworkCore;

namespace StudentApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly StudentDbContext _context;

        public StudentController(StudentDbContext context)
        {
            _context = context;
        }

        // GET: api/student
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var students = await _context.StudentTbs.ToListAsync();
            return Ok(new
            {
                status = 200,
                message = "Student list",
                data = students
            });
        }

        // GET: api/student/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudentById(int id)
        {
            var student = await _context.StudentTbs.FirstOrDefaultAsync(s => s.StudentPkid == id);

            if (student == null)
                return NotFound(new { status = 404, message = "Student not found" });

            return Ok(new { status = 200, data = student });
        }

        // POST: api/student
        [HttpPost]
        public async Task<IActionResult> CreateStudent(StudentTb student)
        {
            _context.StudentTbs.Add(student);
            await _context.SaveChangesAsync(); // ✅ Must await

            return CreatedAtAction(nameof(GetStudentById), new { id = student.StudentPkid }, new
            {
                status = 201,
                message = "Student created",
                data = student
            });
        }

        // PUT: api/student/1
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(int id, StudentTb student)
        {
            var oldStudent = await _context.StudentTbs.FirstOrDefaultAsync(s => s.StudentPkid == id);

            if (oldStudent == null)
                return NotFound(new { status = 404, message = "Student not found" });

            oldStudent.FullName = student.FullName;
            oldStudent.Email = student.Email;
            oldStudent.DateOfBirth = student.DateOfBirth;
            oldStudent.Gender = student.Gender;
            oldStudent.Phone = student.Phone;
            oldStudent.Address = student.Address;
            oldStudent.EnrollmentDate = student.EnrollmentDate;
            oldStudent.ImagePath = student.ImagePath;

            await _context.SaveChangesAsync(); // ✅ Save changes

            return Ok(new
            {
                status = 200,
                message = "Student updated",
                data = oldStudent
            });
        }

        // DELETE: api/student/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.StudentTbs.FirstOrDefaultAsync(s => s.StudentPkid == id);

            if (student == null)
                return NotFound(new { status = 404, message = "Student not found" });

            _context.StudentTbs.Remove(student);
            await _context.SaveChangesAsync(); // ✅ Must await

            return Ok(new
            {
                status = 200,
                message = "Student deleted"
            });
        }
    }
}
