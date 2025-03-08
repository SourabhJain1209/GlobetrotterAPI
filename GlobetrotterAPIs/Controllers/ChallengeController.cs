using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GlobetrotterAPIs.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using GlobetrotterAPIs.Models;
using GlobetrotterAPIs.Context;

namespace GlobetrotterAPI.Controllers
{
    [Route("api/challenges")]
    [ApiController]
    public class ChallengeController : ControllerBase
    {
        private readonly GlobetrotterContext _context;

        public ChallengeController(GlobetrotterContext context)
        {
            _context = context;
        }

        [HttpPost("CreateChallenge")]
        public async Task<ApiResponse<int?>> CreateChallenge(CreateChallenge challenge)
        {
            try
            {
                User user = new User
                {
                    Username = challenge.challengeName,
                    Score = 0,
                    CorrectAnswers = 0,
                    IncorrectAnswers = 0
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync(); // Await to ensure user is saved

                Challenge c = new Challenge
                {
                    ChallengerId = challenge.challengerId,
                    ChallengedTo = user.Id // Ensure user.Id is available
                };

                _context.Challenges.Add(c);
                await _context.SaveChangesAsync(); // Await for challenges

                return new ApiResponse<int?>
                {
                    IsSuccess = true,
                    Data = c.InviteCode
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<int?>
                {
                    IsSuccess = false,
                    Message = "An error occurred: " + ex.Message
                };
            }
        }

        [HttpGet("GetChallengeData/{id}")]
        public async Task<ApiResponse<ChallengeDataDTO>> GetChallengeData(int id)
        { 
            var res = _context.Challenges.Where(x => x.InviteCode == id).Select(x => new ChallengeDataDTO
            {
                ChallengerId = x.ChallengerId,
                ChallengerUsername = x.Challenger.Username,
                ChallengerScore = x.Challenger.Score,
                ChallengedToId = x.ChallengedTo,
                ChallengeToUsername = x.ChallengedToNavigation.Username
            }).FirstOrDefault();

            return new ApiResponse<ChallengeDataDTO>
            {
                IsSuccess = true,
                Data = res
            };
        }
    }
}

public class ChallengeDataDTO
{
    public int? ChallengerId { get; set; }
    public int? ChallengerScore { get; set; }
    public int? ChallengedToId { get; set; }
    public string ChallengerUsername { get; set; }
    public string ChallengeToUsername { get; set; }
}
public class CreateChallenge
{
    public int? challengerId { get; set; }
    public string challengeName { get; set;}
}
public class UserDto
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public int? Score { get; set; }
    public int? CorrectAnswers { get; set; }
    public int? IncorrectAnswers { get; set; }
}

public class VerifyAnswerDto
{
    public int UserId { get; set; }
    public int DestinationId { get; set; }
    public string Answer { get; set; }
}

public class CreateChallengeDto
{
    public int UserId { get; set; }
}