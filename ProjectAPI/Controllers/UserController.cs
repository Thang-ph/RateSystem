using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectAPI.Models;

namespace ProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public ProjectPRN231Context context=new ProjectPRN231Context();
        [HttpGet]
        public IActionResult Login(string username, string password)
        {
            User user = context.Users.FirstOrDefault(x => x.Username.Equals(username));

            if (user == null)
            {
                return NotFound();
            }

            if (user.Password.Equals(password))
            {
                Account.CurrentUser = user;

                return Ok(user);
            }
            else
                return NotFound();
        }
        [HttpPost("Register")]
        public IActionResult Register([FromBody] User user)
        {
            User u = context.Users.FirstOrDefault(x => x.Username.Equals(user.Username));

            if (u != null)
            {
                return NotFound("User name da ton tai");
            }
            user.RoleId = 1;
            context.Users.Add(user);
            context.SaveChanges();
                return Ok(user);
        }
        [HttpGet("Logout")]
        public IActionResult Logout()
        {

            Account.CurrentUser = null;

                return Ok();
           
        }

        [HttpGet("User")]
        public IActionResult GetCurrentUser()
        {
            if (Account.CurrentUser == null)
            {
                return Unauthorized("User not logged in");  // Trả về lỗi nếu không có người dùng đã đăng nhập
            }

            return Ok(Account.CurrentUser);  // Trả về thông tin người dùng đã đăng nhập
        }

    }
}
