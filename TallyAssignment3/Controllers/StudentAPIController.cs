using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TallyAssignment3.Models;

namespace TallyAssignment3.Controllers
{
    [Route("api/StudentAPI")]
    [ApiController]
    public class StudentAPIController : ControllerBase
    {
        private readonly TestDb2Context _db;

        public StudentAPIController(TestDb2Context db)
        {
            _db = db;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudent()
        {
            return Ok(await _db.Students.ToListAsync());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Student>> GetStudentById(int id)
        {
            if(id == 0)
            {
                return BadRequest();
            }
            var stud = await _db.Students.FirstOrDefaultAsync(u => u.StudentId == id);
            if(stud == null)
            {
                return NotFound();
            }
            return Ok(stud);
           
        }

        [HttpPost]
        public async Task<ActionResult<Student>> CreateStudent(Student student)
        {
            _db.Students.Add(student);
            await _db.SaveChangesAsync();
            return CreatedAtAction("GetStudent", new { id = student.StudentId }, student);
             
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateStudentById(int id, Student student)
        {
            if(student == null || id != student.StudentId)
            {
                return BadRequest();
            }
            
            var stud = _db.Students.AsNoTracking().FirstOrDefault(u => u.StudentId == id);
            stud.Name = student.Name;
            stud.Address = student.Address;
            stud.Class = student.Class;

            _db.Students.Update(student);
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (IsProductExist(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok(stud);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeletStudentById(int id)
        {
            var dstud = await _db.Students.FirstOrDefaultAsync(u => u.StudentId == id);
            if(dstud == null)
            {
                return NotFound();
            }
            _db.Students.Remove(dstud);
            await _db.SaveChangesAsync();
            return NoContent();
        }
        private bool IsProductExist(int id)
        {
            return _db.Students.Any(e => e.StudentId == id);
        }
    }
}
