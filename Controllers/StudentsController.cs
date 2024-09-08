using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApiNZwalks.Controllers
{
    // GET: https://localhost:7017/api/students
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllStudents()
        {
            string[] studentNames = new string[] { "Naman", "Alex", "Bethany" };

            return Ok(studentNames);
        }
    }
}
