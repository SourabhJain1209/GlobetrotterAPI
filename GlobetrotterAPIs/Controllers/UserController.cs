using GlobetrotterAPIs.Context;
using GlobetrotterAPIs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GlobetrotterAPIs.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly GlobetrotterContext _context;
        public UserController(GlobetrotterContext context)
        {
            _context = context;
        }

        [HttpPost("CreateUser")]
        public async Task<ApiResponse<int?>> CreateUser([FromBody] UserDto userDto)
        {
            var checkUser = _context.Users.Where(x => x.Username == userDto.Username).Any();
            if (checkUser == false)
            {
                try
                {
                    User user = new User();
                    user.Username = userDto.Username;
                    user.Score = 0;
                    user.CorrectAnswers = 0;
                    user.IncorrectAnswers = 0;
                    await _context.Users.AddAsync(user);
                    await _context.SaveChangesAsync();

                    return new ApiResponse<int?>
                    {
                        Data = user.Id,
                        IsSuccess = true
                    };
                }
                catch(Exception e) {
                    return new ApiResponse<int?>
                    {
                        Data = null,
                        IsSuccess = false,
                        Message = e.Message
                    };
                }
            }
            else
            {
                return new ApiResponse<int?>
                {
                    Data = null,
                    IsSuccess = false,
                    Message = "Enter unique user name"
                };
            }
        }

        [HttpPost("SaveUserScore")]
        public async Task<ApiResponse<int?>> SaveUserScore([FromBody] UserDto userDto)
        {
            try
            {
                var user = _context.Users.Where(x => x.Id == userDto.Id).FirstOrDefault();
                
                user.Score = userDto.Score;
                user.CorrectAnswers = userDto.CorrectAnswers;
                user.IncorrectAnswers = userDto.IncorrectAnswers;
                await _context.SaveChangesAsync();

                return new ApiResponse<int?>
                {
                    Data = user.Id,
                    IsSuccess = true
                };
            }
            catch (Exception e)
            {
                return new ApiResponse<int?>
                {
                    Data = null,
                    IsSuccess = false,
                    Message = e.Message
                };
            }
        }

        //[HttpPost]
        //public async Task<IActionResult> CreateUser([FromBody] UserDto userDto)
        //{
        //    var user = new User
        //    {
        //        Username = userDto.Username,
        //        Score = 0,
        //        CorrectAnswers = 0,
        //        IncorrectAnswers = 0
        //    };

        //    _context.Users.Add(user);
        //    await _context.SaveChangesAsync();

        //    return Ok(new { userId = user.Id, username = user.Username });
        //}

        //[HttpGet("{userId}")]
        //public async Task<IActionResult> GetUser(int userId)
        //{
        //    var user = await _context.Users.FindAsync(userId);
        //    if (user == null)
        //        return NotFound();

        //    return Ok(new
        //    {
        //        userId = user.Id,
        //        username = user.Username,
        //        score = user.Score
        //    });
        //}
    }
}